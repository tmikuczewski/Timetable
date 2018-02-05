using System;
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

			WriteTimeTableForClass(classRow);

			SetHeader($"Klasa {classRow.ToFriendlyString()}");

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

			WriteTimeTableForTeacher(teacherRow);

			SetHeader($"{teacherRow.LastName} {teacherRow.FirstName}");

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

			WriteTimeTableForClassroom(classroomRow);

			SetHeader($"Sala {classroomRow.Name}");

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

			_daysTableAdapter.Fill(_timetableDataSet.Days);
			_hoursTableAdapter.Fill(_timetableDataSet.Hours);
		}

		private static void RefreshDatabaseObjects()
		{
			_classesTableAdapter.Fill(_timetableDataSet.Classes);
			_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
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

			var dayId = 1;

			try
			{
				for (dayId = 1; dayId <= _timetableDataSet.Days.Count; dayId++)
				{
					range = _xlWorkSheet.Cells[3, 1 + dayId];
					_xlWorkSheet.Cells[3, 1 + dayId] = _timetableDataSet.Days.FirstOrDefault(d => d.Id == dayId)?.Name;
					range.BorderAround(XlLineStyle.xlContinuous);
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
				}
			}
			catch (Exception)
			{
				throw new EntityDoesNotExistException("Day with id=" + dayId + " does not exist.");
			}

			var hourId = 1;

			try
			{
				for (hourId = 1; hourId <= _timetableDataSet.Hours.Count; hourId++)
				{
					range = _xlWorkSheet.Range["A" + (1 + hourId * 3), "A" + (1 + hourId * 3 + 2)];
					range.Merge();
					var beginHour = _timetableDataSet.Hours.FirstOrDefault(h => h.Id == hourId)?.Hour;
					_xlWorkSheet.Cells[1 + hourId * 3, 1] = beginHour.Value.ToString(@"hh\:mm") + " - " +
														   beginHour.Value.Add(TimeSpan.FromMinutes(45)).ToString(@"hh\:mm");
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					range.BorderAround(XlLineStyle.xlContinuous);
				}
			}
			catch (Exception)
			{
				throw new EntityDoesNotExistException("Hour with id=" + hourId + " does not exist.");
			}

			range = _xlWorkSheet.Range["B4", "F27"];
			range.BorderAround(XlLineStyle.xlContinuous);
		}

		private void SetHeader(string header)
		{
			_xlWorkSheet.Cells[1, 1] = header;
			var range = _xlWorkSheet.Range["A1", "F2"];
			range.Merge();
			range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
			range.VerticalAlignment = XlVAlign.xlVAlignCenter;
			range.Font.Size = range.Font.Size + 5;
		}

		private void WriteTimeTableForClass(TimetableDataSet.ClassesRow classRow)
		{
			for (var day = 0; day < _timetableDataSet.Days.Count; day++)
			{
				for (var hour = 0; hour < _timetableDataSet.Hours.Count; hour++)
				{
					var lessonPlace = _timetableDataSet.LessonsPlaces
						.FirstOrDefault(lp => lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.LessonsRow.ClassId == classRow.Id);

					if (lessonPlace == null)
						continue;

					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = lessonPlace.LessonsRow?.SubjectsRow?.Name;
					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = lessonPlace.LessonsRow?.TeachersRow?.ToFriendlyString();
					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "s. " + lessonPlace.ClassroomsRow?.Name;

					var range = _xlWorkSheet.Range["" + (char) ('B' + day) + (4 + hour * 3),
						"" + (char) ('B' + day) + (4 + hour * 3 + 2)];
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					range.BorderAround(XlLineStyle.xlContinuous);
				}
			}
		}

		private void WriteTimeTableForTeacher(TimetableDataSet.TeachersRow teacherRow)
		{
			for (var day = 0; day < _timetableDataSet.Days.Count; day++)
			{
				for (var hour = 0; hour < _timetableDataSet.Hours.Count; hour++)
				{
					var lessonPlace = _timetableDataSet.LessonsPlaces
						.FirstOrDefault(lp => lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.LessonsRow.TeacherPesel == teacherRow.Pesel);

					if (lessonPlace == null)
						continue;

					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = lessonPlace.LessonsRow?.SubjectsRow?.Name;
					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = "kl. " + lessonPlace.LessonsRow?.ClassesRow?.ToFriendlyString();
					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "s. " + lessonPlace.ClassroomsRow?.Name;

					var range = _xlWorkSheet.Range["" + (char) ('B' + day) + (4 + hour * 3),
						"" + (char) ('B' + day) + (4 + hour * 3 + 2)];
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					range.BorderAround(XlLineStyle.xlContinuous);
				}
			}
		}


		private void WriteTimeTableForClassroom(TimetableDataSet.ClassroomsRow classroomRow)
		{
			for (var day = 0; day < _timetableDataSet.Days.Count; day++)
			{
				for (var hour = 0; hour < _timetableDataSet.Hours.Count; hour++)
				{
					var lessonPlace = _timetableDataSet.LessonsPlaces
						.FirstOrDefault(lp => lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.ClassroomId == classroomRow.Id);

					if (lessonPlace == null)
						continue;

					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = lessonPlace.LessonsRow?.SubjectsRow?.Name;
					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = "kl. " + lessonPlace.LessonsRow?.ClassesRow?.ToFriendlyString();
					_xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = lessonPlace.LessonsRow?.TeachersRow?.ToFriendlyString();

					var range = _xlWorkSheet.Range["" + (char) ('B' + day) + (4 + hour * 3),
						"" + (char) ('B' + day) + (4 + hour * 3 + 2)];
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					range.BorderAround(XlLineStyle.xlContinuous);
				}
			}
		}

		private double MaxWidth()
		{
			double max = 0;

			for (var i = 1; i < 10; i++)
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

			for (var i = 2; i < 7; i++)
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
