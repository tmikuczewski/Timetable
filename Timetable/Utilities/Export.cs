using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Timetable.TimetableDataSetTableAdapters;

namespace Timetable.Utilities
{
	/// <summary>
	///     Klasa do eksportowania planu lekcji za pomocą programu Microsoft Excel.
	///     http://csharp.net-informations.com/excel/csharp-format-excel.htm
	/// </summary>
	public class Export
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private static TimetableDataSet _timetableDataSet;
		private static ClassesTableAdapter _classesTableAdapter;
		private static ClassroomsTableAdapter _classroomsTableAdapter;
		private static DaysTableAdapter _daysTableAdapter;
		private static HoursTableAdapter _hoursTableAdapter;
		private static LessonsTableAdapter _lessonsTableAdapter;
		private static LessonsPlacesTableAdapter _lessonsPlacesTableAdapter;
		private static SubjectsTableAdapter _subjectsTableAdapter;
		private static TeachersTableAdapter _teachersTableAdapter;

		private Application _xlApp;
		private Workbook _xlWorkBook;
		private Worksheet _xlWorkSheet;
		private readonly object _misValue = Missing.Value;

		#endregion


		#region Properties

		private IList<TimetableDataSet.DaysRow> DaysList => _timetableDataSet.Days
			.OrderBy(d => d.Number)
			.ToList();

		private IList<TimetableDataSet.HoursRow> HoursList => _timetableDataSet.Hours
			.OrderBy(d => d.Number)
			.ToList();

		#endregion


		#region Delegates

		/// <summary>
		///     Delegat dla zdarzeń zakończenia eksportowania danych.
		/// </summary>
		public delegate void ExportFinishedDelegate();

		/// <summary>
		///     Zdarzenie zakończenia eksportowania danych.
		/// </summary>
		public event ExportFinishedDelegate ExportFinishedEvent;

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworząy obiekt typu <c>Utilities.Export</c>.
		/// </summary>
		public Export()
		{
			InitDatabaseObjects();
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		/// <summary>
		///     Metoda zapisująca plan lekcji dla danej klasy.
		/// </summary>
		/// <param name="classRow">Obiekt klasy.</param>
		/// <param name="filePath">Ścieżka do zapisu pliku.</param>
		/// <param name="fileType">Typ zapisywanego pliku.</param>
		public void SaveTimeTableForClass(TimetableDataSet.ClassesRow classRow, string filePath, ExportFileType fileType)
		{
			RefreshDatabaseObjects();

			PrepareExcel();

			SetHeader($"Klasa {classRow.ToFriendlyString()}");

			WriteTimetableForClass(classRow);

			Save(filePath, fileType);
		}

		/// <summary>
		///     Metoda zapisująca plan lekcji dla danego nauczyciela.
		/// </summary>
		/// <param name="teacherRow">Obiekt nauczyciela.</param>
		/// <param name="filePath">Ścieżka do zapisu pliku.</param>
		/// <param name="fileType">Typ zapisywanego pliku.</param>
		public void SaveTimeTableForTeacher(TimetableDataSet.TeachersRow teacherRow, string filePath, ExportFileType fileType)
		{
			RefreshDatabaseObjects();

			PrepareExcel();

			SetHeader($"{teacherRow.LastName} {teacherRow.FirstName}");

			WriteTimetableForTeacher(teacherRow);

			Save(filePath, fileType);
		}

		/// <summary>
		///     Metoda zapisująca plan lekcji dla danej sali.
		/// </summary>
		/// <param name="classroomRow">Obiekt sali.</param>
		/// <param name="filePath">Ścieżka do zapisu pliku.</param>
		/// <param name="fileType">Typ zapisywanego pliku.</param>
		public void SaveTimeTableForClassroom(TimetableDataSet.ClassroomsRow classroomRow, string filePath, ExportFileType fileType)
		{
			RefreshDatabaseObjects();

			PrepareExcel();

			SetHeader($"Sala {classroomRow.Name}");

			WriteTimetableForClassroom(classroomRow);

			Save(filePath, fileType);
		}

		#endregion


		#region Private methods

		private static void InitDatabaseObjects()
		{
			_timetableDataSet = new TimetableDataSet();

			_classesTableAdapter = new ClassesTableAdapter();
			_classroomsTableAdapter = new ClassroomsTableAdapter();
			_daysTableAdapter = new DaysTableAdapter();
			_hoursTableAdapter = new HoursTableAdapter();
			_lessonsTableAdapter = new LessonsTableAdapter();
			_lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			_subjectsTableAdapter = new SubjectsTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();
		}

		private static void RefreshDatabaseObjects()
		{
			_classesTableAdapter.Fill(_timetableDataSet.Classes);
			_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
			_daysTableAdapter.Fill(_timetableDataSet.Days);
			_hoursTableAdapter.Fill(_timetableDataSet.Hours);
			_lessonsTableAdapter.Fill(_timetableDataSet.Lessons);
			_lessonsPlacesTableAdapter.Fill(_timetableDataSet.LessonsPlaces);
			_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
			_teachersTableAdapter.Fill(_timetableDataSet.Teachers);
		}

		private void PrepareExcel()
		{
			_xlApp = new Application();

			if (_xlApp == null)
			{
				throw new ExcelApplicationException("Cannot initialize Microsoft.Office.Interop.Excel.Application.");
			}

			_xlWorkBook = _xlApp.Workbooks.Add(_misValue);
			_xlWorkSheet = (Worksheet) _xlWorkBook.Worksheets.Item[1];

			PrepareTable();
		}

		private void PrepareTable()
		{
			Range range;

			var dayIndex = 0;

			try
			{
				for (dayIndex = 0; dayIndex < DaysList.Count; dayIndex++)
				{
					range = _xlWorkSheet.Range["" + (char) ('B' + dayIndex) + "4", "" + (char) ('B' + dayIndex) + "6"];
					range.Merge();
					_xlWorkSheet.Cells[4, 2 + dayIndex] = DaysList.ElementAt(dayIndex).Name;
					range.BorderAround(XlLineStyle.xlContinuous);
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
				}
			}
			catch (Exception)
			{
				throw new EntityDoesNotExistException("Day with ID = " + DaysList.ElementAt(dayIndex).Id + " does not exist.");
			}

			var hourIndex = 0;

			try
			{
				for (hourIndex = 0; hourIndex < HoursList.Count; hourIndex++)
				{
					range = _xlWorkSheet.Range["A" + (4 + (1 + hourIndex) * 3), "A" + (4 + (1 + hourIndex) * 3 + 2)];
					range.Merge();
					var beginHour = HoursList.ElementAt(hourIndex).Begin;
					var endHour = HoursList.ElementAt(hourIndex).End;
					_xlWorkSheet.Cells[4 + (1 + hourIndex) * 3, 1] =
						beginHour.ToString(@"hh\:mm") + " – " + endHour.ToString(@"hh\:mm");
					range.BorderAround(XlLineStyle.xlContinuous);
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
				}
			}
			catch (Exception)
			{
				throw new EntityDoesNotExistException("Hour with ID = " + HoursList.ElementAt(hourIndex).Id + " does not exist.");
			}

			try
			{
				for (dayIndex = 0; dayIndex < DaysList.Count; dayIndex++)
				{
					for (hourIndex = 0; hourIndex < HoursList.Count; hourIndex++)
					{
						range = _xlWorkSheet.Range["" + (char) ('B' + dayIndex) + (7 + hourIndex * 3),
							"" + (char) ('B' + dayIndex) + (7 + hourIndex * 3 + 2)];
						range.BorderAround(XlLineStyle.xlContinuous);
						range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
						range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					}
				}
			}
			catch (Exception)
			{
				throw new EntityDoesNotExistException();
			}
		}

		private void SetHeader(string header)
		{
			_xlWorkSheet.Cells[1, 1] = header;
			var range = _xlWorkSheet.Range["A1", (char) ('A' + DaysList.Count) + "3"];
			range.Merge();
			range.Font.Size = range.Font.Size + 6;
			range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
			range.VerticalAlignment = XlVAlign.xlVAlignCenter;
		}

		private void WriteTimetableForClass(TimetableDataSet.ClassesRow classRow)
		{
			IEnumerable<TimetableDataSet.LessonsPlacesRow> lessonsPlaces = _timetableDataSet.LessonsPlaces
				.Where(lp => lp.LessonsRow.ClassId == classRow.Id);

			foreach (var lessonsPlace in lessonsPlaces)
			{
				var dayIndex = DaysList.IndexOf(DaysList.FirstOrDefault(d => d.Id == lessonsPlace.DayId));
				var hourIndex = HoursList.IndexOf(HoursList.FirstOrDefault(h => h.Id == lessonsPlace.HourId));

				string firstRow = lessonsPlace.LessonsRow?.SubjectsRow?.Name;
				string secondRow = lessonsPlace.LessonsRow?.TeachersRow?.ToFriendlyString();
				string thirdRow = "s. " + lessonsPlace.ClassroomsRow?.Name;

				WriteLessonsPlaceCell(dayIndex, hourIndex, firstRow, secondRow, thirdRow);
			}
		}

		private void WriteTimetableForTeacher(TimetableDataSet.TeachersRow teacherRow)
		{
			IEnumerable<TimetableDataSet.LessonsPlacesRow> lessonsPlaces = _timetableDataSet.LessonsPlaces
				.Where(lp => lp.LessonsRow.TeacherPesel == teacherRow.Pesel);

			foreach (var lessonsPlace in lessonsPlaces)
			{
				var dayIndex = DaysList.IndexOf(DaysList.FirstOrDefault(d => d.Id == lessonsPlace.DayId));
				var hourIndex = HoursList.IndexOf(HoursList.FirstOrDefault(h => h.Id == lessonsPlace.HourId));

				string firstRow = lessonsPlace.LessonsRow?.SubjectsRow?.Name;
				string secondRow = "kl. " + lessonsPlace.LessonsRow?.ClassesRow?.ToFriendlyString();
				string thirdRow = "s. " + lessonsPlace.ClassroomsRow?.Name;

				WriteLessonsPlaceCell(dayIndex, hourIndex, firstRow, secondRow, thirdRow);
			}
		}


		private void WriteTimetableForClassroom(TimetableDataSet.ClassroomsRow classroomRow)
		{
			IEnumerable<TimetableDataSet.LessonsPlacesRow> lessonsPlaces = _timetableDataSet.LessonsPlaces
				.Where(lp => lp.ClassroomId == classroomRow.Id);

			foreach (var lessonsPlace in lessonsPlaces)
			{
				var dayIndex = DaysList.IndexOf(DaysList.FirstOrDefault(d => d.Id == lessonsPlace.DayId));
				var hourIndex = HoursList.IndexOf(HoursList.FirstOrDefault(h => h.Id == lessonsPlace.HourId));

				string firstRow = lessonsPlace.LessonsRow?.SubjectsRow?.Name;
				string secondRow = "kl. " + lessonsPlace.LessonsRow?.ClassesRow?.ToFriendlyString();
				string thirdRow = lessonsPlace.LessonsRow?.TeachersRow?.ToFriendlyString();

				WriteLessonsPlaceCell(dayIndex, hourIndex, firstRow, secondRow, thirdRow);
			}
		}

		private void WriteLessonsPlaceCell(int dayIndex, int hourIndex, string firstRow, string secondRow, string thirdRow)
		{
			_xlWorkSheet.Cells[2 + dayIndex][7 + 3 * hourIndex + 0] = firstRow;
			_xlWorkSheet.Cells[2 + dayIndex][7 + 3 * hourIndex + 1] = secondRow;
			_xlWorkSheet.Cells[2 + dayIndex][7 + 3 * hourIndex + 2] = thirdRow;
		}

		private double MaxWidth()
		{
			double max = 0;

			for (var i = 1; i <= 1 + DaysList.Count; i++)
			{
				Range column = _xlWorkSheet.Columns[i];
				max = (column.ColumnWidth > max) ? column.ColumnWidth : max;
			}

			return max;
		}

		private void Save(string path, ExportFileType fileType)
		{
			_xlWorkSheet.Columns.AutoFit();

			var max = MaxWidth();

			for (var i = 1; i <= 1 + DaysList.Count; i++)
				_xlWorkSheet.Columns[i].ColumnWidth = max;

			if (fileType.Equals(ExportFileType.XLS))
				_xlWorkBook.SaveAs(path, XlFileFormat.xlWorkbookNormal, _misValue, _misValue, _misValue, _misValue,
					XlSaveAsAccessMode.xlExclusive, _misValue, _misValue, _misValue, _misValue, _misValue);

			if (fileType.Equals(ExportFileType.PDF))
			{
				_xlWorkSheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
				_xlWorkSheet.PageSetup.Zoom = false;
				_xlWorkSheet.PageSetup.FitToPagesTall = 1;
				_xlWorkSheet.PageSetup.FitToPagesWide = 1;
				_xlWorkSheet.PageSetup.PaperSize = XlPaperSize.xlPaperA4;
				_xlWorkBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, path);
			}

			_xlWorkBook.Close(false, _misValue, _misValue);
			_xlApp.Quit();

			ReleaseObject(_xlWorkSheet);
			ReleaseObject(_xlWorkBook);
			ReleaseObject(_xlApp);

			ExportFinishedEvent?.Invoke();
		}

		private void ReleaseObject(object obj)
		{
			try
			{
				Marshal.ReleaseComObject(obj);
				obj = null;
			}
			catch (Exception)
			{
				obj = null;
			}
			finally
			{
				GC.Collect();
			}
		}

		#endregion
	}
}
