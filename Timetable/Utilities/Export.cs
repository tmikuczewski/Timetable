using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Timetable.TimetableDataSetTableAdapters;

namespace Timetable.Utilities
{
	//http://csharp.net-informations.com/excel/csharp-format-excel.htm
	public class Export
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private static TimetableDataSet timetableDataSet;
		private static ClassesTableAdapter classesTableAdapter;
		private static ClassroomsTableAdapter classroomsTableAdapter;
		private static DaysTableAdapter daysTableAdapter;
		private static HoursTableAdapter hoursTableAdapter;
		private static LessonsTableAdapter lessonsTableAdapter;
		private static LessonsPlacesTableAdapter lessonsPlacesTableAdapter;
		private static StudentsTableAdapter studentsTableAdapter;
		private static SubjectsTableAdapter subjectsTableAdapter;
		private static TeachersTableAdapter teachersTableAdapter;

		private Application xlApp;
		private Workbook xlWorkBook;
		private Worksheet xlWorkSheet;
		private readonly object misValue = Missing.Value;

		#endregion


		#region Properties

		#endregion


		#region Delegates

		public delegate void ExportFinishedDelegate();

		public event ExportFinishedDelegate ExportFinishedEvent;

		#endregion


		#region Constructors

		/// <summary>
		///     Klasa eksportująca plan z bazy do Excelowego formatu XLS
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
			PrepareExcel();

			WriteTimeTableForClassroom(classroomRow);

			SetHeader($"Sala {classroomRow.Name}");

			Save(filePath, fileType);
		}

		#endregion


		#region Private methods

		private static void InitDatabaseObjects()
		{
			timetableDataSet = new TimetableDataSet();

			classesTableAdapter = new ClassesTableAdapter();
			classroomsTableAdapter = new ClassroomsTableAdapter();
			daysTableAdapter = new DaysTableAdapter();
			hoursTableAdapter = new HoursTableAdapter();
			lessonsTableAdapter = new LessonsTableAdapter();
			lessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			studentsTableAdapter = new StudentsTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			classroomsTableAdapter.Fill(timetableDataSet.Classrooms);
			daysTableAdapter.Fill(timetableDataSet.Days);
			hoursTableAdapter.Fill(timetableDataSet.Hours);
			lessonsTableAdapter.Fill(timetableDataSet.Lessons);
			lessonsPlacesTableAdapter.Fill(timetableDataSet.LessonsPlaces);
			studentsTableAdapter.Fill(timetableDataSet.Students);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
		}

		private void PrepareExcel()
		{
			xlApp = new Application();

			if (xlApp == null)
			{
				throw new ExcelApplicationException("Cannot initialize Microsoft.Office.Interop.Excel.Application.");
			}

			xlWorkBook = xlApp.Workbooks.Add(misValue);
			xlWorkSheet = (Worksheet) xlWorkBook.Worksheets.Item[1];

			PrepareTable();
		}

		private void PrepareTable()
		{
			Range range;

			var dayId = 1;

			try
			{
				for (dayId = 1; dayId <= timetableDataSet.Days.Count; dayId++)
				{
					range = xlWorkSheet.Cells[3, 1 + dayId];
					xlWorkSheet.Cells[3, 1 + dayId] = timetableDataSet.Days.FirstOrDefault(d => d.Id == dayId)?.Name;
					range.BorderAround(XlLineStyle.xlContinuous);
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
				}
			}
			catch (Exception e1)
			{
				throw new EntityDoesNotExistException("Day with id=" + dayId + " does not exist.");
			}

			var hourId = 1;

			try
			{
				for (hourId = 1; hourId <= timetableDataSet.Hours.Count; hourId++)
				{
					range = xlWorkSheet.Range["A" + (1 + hourId * 3), "A" + (1 + hourId * 3 + 2)];
					range.Merge();
					var beginHour = timetableDataSet.Hours.FirstOrDefault(h => h.Id == hourId)?.Hour;
					xlWorkSheet.Cells[1 + hourId * 3, 1] = beginHour.Value.ToString(@"hh\:mm") + " - " +
														   beginHour.Value.Add(TimeSpan.FromMinutes(45)).ToString(@"hh\:mm");
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					range.BorderAround(XlLineStyle.xlContinuous);
				}
			}
			catch (Exception e2)
			{
				throw new EntityDoesNotExistException("Hour with id=" + hourId + " does not exist.");
			}

			range = xlWorkSheet.Range["B4", "F27"];
			range.BorderAround(XlLineStyle.xlContinuous);
		}

		private void SetHeader(string header)
		{
			xlWorkSheet.Cells[1, 1] = header;
			var range = xlWorkSheet.Range["A1", "F2"];
			range.Merge();
			range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
			range.VerticalAlignment = XlVAlign.xlVAlignCenter;
			range.Font.Size = range.Font.Size + 5;
		}

		private void WriteTimeTableForClass(TimetableDataSet.ClassesRow classRow)
		{
			for (var day = 0; day < timetableDataSet.Days.Count; day++)
			{
				for (var hour = 0; hour < timetableDataSet.Hours.Count; hour++)
				{
					var lessonPlace = timetableDataSet.LessonsPlaces
						.FirstOrDefault(lp => lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.LessonsRow.ClassId == classRow.Id);

					if (lessonPlace == null)
						continue;

					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = lessonPlace.LessonsRow?.SubjectsRow?.Name;
					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = lessonPlace.LessonsRow?.TeachersRow?.ToFriendlyString();
					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "s. " + lessonPlace.ClassroomsRow?.Name;

					var range = xlWorkSheet.Range["" + (char) ('B' + day) + (4 + hour * 3),
						"" + (char) ('B' + day) + (4 + hour * 3 + 2)];
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					range.BorderAround(XlLineStyle.xlContinuous);
				}
			}
		}

		private void WriteTimeTableForTeacher(TimetableDataSet.TeachersRow teacherRow)
		{
			for (var day = 0; day < timetableDataSet.Days.Count; day++)
			{
				for (var hour = 0; hour < timetableDataSet.Hours.Count; hour++)
				{
					var lessonPlace = timetableDataSet.LessonsPlaces
						.FirstOrDefault(lp => lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.LessonsRow.TeacherPesel == teacherRow.Pesel);

					if (lessonPlace == null)
						continue;

					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = lessonPlace.LessonsRow?.SubjectsRow?.Name;
					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = "kl. " + lessonPlace.LessonsRow?.ClassesRow?.ToFriendlyString();
					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "s. " + lessonPlace.ClassroomsRow?.Name;

					var range = xlWorkSheet.Range["" + (char) ('B' + day) + (4 + hour * 3),
						"" + (char) ('B' + day) + (4 + hour * 3 + 2)];
					range.HorizontalAlignment = XlHAlign.xlHAlignCenter;
					range.VerticalAlignment = XlVAlign.xlVAlignCenter;
					range.BorderAround(XlLineStyle.xlContinuous);
				}
			}
		}


		private void WriteTimeTableForClassroom(TimetableDataSet.ClassroomsRow classroomRow)
		{
			for (var day = 0; day < timetableDataSet.Days.Count; day++)
			{
				for (var hour = 0; hour < timetableDataSet.Hours.Count; hour++)
				{
					var lessonPlace = timetableDataSet.LessonsPlaces
						.FirstOrDefault(lp => lp.DayId == day + 1 && lp.HourId == hour + 1 && lp.ClassroomId == classroomRow.Id);

					if (lessonPlace == null)
						continue;

					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 0] = lessonPlace.LessonsRow?.SubjectsRow?.Name;
					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 1] = lessonPlace.LessonsRow?.TeachersRow?.ToFriendlyString();
					xlWorkSheet.Cells[2 + day][4 + 3 * hour + 2] = "kl. " + lessonPlace.LessonsRow?.ClassesRow?.ToFriendlyString();

					var range = xlWorkSheet.Range["" + (char) ('B' + day) + (4 + hour * 3),
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
				Range column = xlWorkSheet.Columns[i];
				max = column.ColumnWidth > max ? column.ColumnWidth : max;
			}

			return max;
		}

		private void Save(string path, ExportFileType fileType)
		{
			xlWorkSheet.Columns.AutoFit();

			var max = MaxWidth();

			for (var i = 2; i < 7; i++)
				xlWorkSheet.Columns[i].ColumnWidth = max;

			if (fileType.Equals(ExportFileType.XLS))
				xlWorkBook.SaveAs(path, XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
					XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

			if (fileType.Equals(ExportFileType.PDF))
			{
				xlWorkSheet.PageSetup.Orientation = XlPageOrientation.xlLandscape;
				xlWorkSheet.PageSetup.Zoom = false;
				xlWorkSheet.PageSetup.FitToPagesTall = 1;
				xlWorkSheet.PageSetup.FitToPagesWide = 1;
				xlWorkSheet.PageSetup.PaperSize = XlPaperSize.xlPaperA4;
				xlWorkBook.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, path);
			}

			xlWorkBook.Close(false, misValue, misValue);
			xlApp.Quit();

			ReleaseObject(xlWorkSheet);
			ReleaseObject(xlWorkBook);
			ReleaseObject(xlApp);

			ExportFinishedEvent?.Invoke();
		}

		private void ReleaseObject(object obj)
		{
			try
			{
				Marshal.ReleaseComObject(obj);
				obj = null;
			}
			catch (Exception ex)
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
