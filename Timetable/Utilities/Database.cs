using System.Collections.Generic;
using System.Linq;

using Timetable.Models;

namespace Timetable.Utilities
{
	public static class Database
	{
		#region Constructors

		static Database()
		{
			ClassesTable = new Models.TimetableDataSet.classesDataTable();
			ClassroomsTable = new Models.TimetableDataSet.classroomsDataTable();
			DaysTable = new Models.TimetableDataSet.daysDataTable();
			HoursTable = new Models.TimetableDataSet.hoursDataTable();
			LessonsTable = new Models.TimetableDataSet.lessonsDataTable();
			LessonsPlacesTable = new Models.TimetableDataSet.lessons_placesDataTable();
			StudentsTable = new Models.TimetableDataSet.studentsDataTable();
			SubjectsTable = new Models.TimetableDataSet.subjectsDataTable();
			TeachersTable = new Models.TimetableDataSet.teachersDataTable();

			ClassesTableAdapter = new Models.TimetableDataSetTableAdapters.classesTableAdapter();
			ClassroomsTableAdapter = new Models.TimetableDataSetTableAdapters.classroomsTableAdapter();
			DaysTableAdapter = new Models.TimetableDataSetTableAdapters.daysTableAdapter();
			HoursTableAdapter = new Models.TimetableDataSetTableAdapters.hoursTableAdapter();
			LessonsTableAdapter = new Models.TimetableDataSetTableAdapters.lessonsTableAdapter();
			LessonsPlacesTableAdapter = new Models.TimetableDataSetTableAdapters.lessons_placesTableAdapter();
			StudentsTableAdapter = new Models.TimetableDataSetTableAdapters.studentsTableAdapter();
			SubjectsTableAdapter = new Models.TimetableDataSetTableAdapters.subjectsTableAdapter();
			TeachersTableAdapter = new Models.TimetableDataSetTableAdapters.teachersTableAdapter();

			ClassesTableAdapter.Fill(ClassesTable);
			ClassroomsTableAdapter.Fill(ClassroomsTable);
			DaysTableAdapter.Fill(DaysTable);
			HoursTableAdapter.Fill(HoursTable);
			LessonsTableAdapter.Fill(LessonsTable);
			LessonsPlacesTableAdapter.Fill(LessonsPlacesTable);
			StudentsTableAdapter.Fill(StudentsTable);
			SubjectsTableAdapter.Fill(SubjectsTable);
			TeachersTableAdapter.Fill(TeachersTable);
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		/// <summary>
		/// Przykładowa implementacja metody wykonującej jedno z zapytań.
		/// </summary>
		/// <returns>Lista wszystkich nauczycieli (<code>Models.Teacher</code>)</returns>
		public static IEnumerable<Teacher> GetTeachers()
		{
			foreach (var row in TeachersTable)
			{
				yield return new Teacher(row.pesel, row.first_name, row.last_name);
			}
			yield break;
		}

		#endregion

		#region Properties

		public static Models.TimetableDataSet.classesDataTable ClassesTable { get; }
		public static Models.TimetableDataSet.classroomsDataTable ClassroomsTable { get; }
		public static Models.TimetableDataSet.daysDataTable DaysTable { get; }
		public static Models.TimetableDataSet.hoursDataTable HoursTable { get; }
		public static Models.TimetableDataSet.lessonsDataTable LessonsTable { get; }
		public static Models.TimetableDataSet.lessons_placesDataTable LessonsPlacesTable { get; }
		public static Models.TimetableDataSet.studentsDataTable StudentsTable { get; }
		public static Models.TimetableDataSet.subjectsDataTable SubjectsTable { get; }
		public static Models.TimetableDataSet.teachersDataTable TeachersTable { get; }

		public static Models.TimetableDataSetTableAdapters.classesTableAdapter ClassesTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.classroomsTableAdapter ClassroomsTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.daysTableAdapter DaysTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.hoursTableAdapter HoursTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.lessonsTableAdapter LessonsTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.lessons_placesTableAdapter LessonsPlacesTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.studentsTableAdapter StudentsTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.subjectsTableAdapter SubjectsTableAdapter { get; }
		public static Models.TimetableDataSetTableAdapters.teachersTableAdapter TeachersTableAdapter { get; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
