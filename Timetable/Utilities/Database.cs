using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Timetable.Code;
using Timetable.Models;
using Timetable.Models.DataSet;
using Timetable.Models.DataSet.TimetableDataSetTableAdapters;

namespace Timetable.Utilities
{
	/// <summary>
	/// Statyczna klasa odpowiadająca za wszystkie operacje odczytu/zapisu na danych zapisanych w bazie danych.</summary>
	public static class Database
	{
		#region Constructors

		static Database()
		{
			ClassesTable = new TimetableDataSet.ClassesDataTable();
			ClassroomsTable = new TimetableDataSet.ClassroomsDataTable();
			DaysTable = new TimetableDataSet.DaysDataTable();
			HoursTable = new TimetableDataSet.HoursDataTable();
			LessonsTable = new TimetableDataSet.LessonsDataTable();
			LessonsPlacesTable = new TimetableDataSet.LessonsPlacesDataTable();
			StudentsTable = new TimetableDataSet.StudentsDataTable();
			SubjectsTable = new TimetableDataSet.SubjectsDataTable();
			TeachersTable = new TimetableDataSet.TeachersDataTable();

			ClassesTableAdapter = new ClassesTableAdapter();
			ClassroomsTableAdapter = new ClassroomsTableAdapter();
			DaysTableAdapter = new DaysTableAdapter();
			HoursTableAdapter = new HoursTableAdapter();
			LessonsTableAdapter = new LessonsTableAdapter();
			LessonsPlacesTableAdapter = new LessonsPlacesTableAdapter();
			StudentsTableAdapter = new StudentsTableAdapter();
			SubjectsTableAdapter = new SubjectsTableAdapter();
			TeachersTableAdapter = new TeachersTableAdapter();

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
		/// Przykładowa implementacja metody wykonującej jedno z zapytań.</summary>
		/// <returns>Lista wszystkich uczniów (<c>IEnumerable&lt;Models.Student&gt;</c>)</returns>
		public static IEnumerable<Student> GetStudents()
		{
			foreach (var row in StudentsTable)
			{
				yield return new Student(new Code.Pesel(row.Pesel), row.FirstName, row.LastName);
			}
			yield break;
		}

		/// <summary>
		/// Przykładowa implementacja metody wykonującej jedno z zapytań.</summary>
		/// <returns>Lista wszystkich nauczycieli (<c>IEnumerable&lt;Models.Teacher&gt;</c>)</returns>
		public static IEnumerable<Teacher> GetTeachers()
		{
			foreach (var row in TeachersTable)
			{
				yield return new Teacher(new Code.Pesel(row.Pesel), row.FirstName, row.LastName);
			}
			yield break;
		}

		/// <summary>
		/// Przykładowa implementacja metody wykonującej jedno z zapytań.</summary>
		/// <returns>Lista wszystkich klas (<c>IEnumerable&lt;Models.Class&gt;</c>)</returns>
		public static IEnumerable<Class> GetClasses()
		{
			foreach (var row in ClassesTable)
			{
				yield return new Class(row.Id, row.Year, row.CodeName, new Code.Pesel(row.TutorPesel));
			}
			yield break;
		}

		/// <summary>
		/// Pobranie danych ucznia z bazy danych.</summary>
		/// <returns>Obiekt typu <c>Student</c>.</returns>
		public static Student GetStudentByPesel(string pesel)
		{
			TimetableDataSet.StudentsRow existingStudentRow = StudentsTable.FindByPesel(pesel);

			if (existingStudentRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return new Student(new Pesel(existingStudentRow.Pesel), existingStudentRow.FirstName, existingStudentRow.LastName);
		}

		/// <summary>
		/// Dodanie nowego ucznia do bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int AddStudent(string pesel, string firstName, string lastName)
		{
			TimetableDataSet.StudentsRow newStudentRow = StudentsTable.NewStudentsRow();
			newStudentRow.Pesel = pesel;
			newStudentRow.FirstName = firstName;
			newStudentRow.LastName = lastName;
			newStudentRow.SetClassIdNull();

			TimetableDataSet.StudentsRow existingStudentRow = StudentsTable.FindByPesel(pesel);

			if (existingStudentRow != null)
			{
				throw new DuplicateEntityException();
			}

			StudentsTable.Rows.Add(newStudentRow);

			return StudentsTableAdapter.Update(StudentsTable);
		}

		/// <summary>
		/// Edycja danych ucznia w bazie danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int EditStudent(string pesel, string firstName, string lastName)
		{
			TimetableDataSet.StudentsRow existingStudentRow = StudentsTable.FindByPesel(pesel);

			if (existingStudentRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			existingStudentRow.FirstName = firstName;
			existingStudentRow.LastName = lastName;
			StudentsTable.AcceptChanges();

			int rows = StudentsTableAdapter.Update(StudentsTable);
			return rows;
		}

		#endregion

		#region Properties



		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private static TimetableDataSet.ClassesDataTable ClassesTable;
		private static TimetableDataSet.ClassroomsDataTable ClassroomsTable;
		private static TimetableDataSet.DaysDataTable DaysTable;
		private static TimetableDataSet.HoursDataTable HoursTable;
		private static TimetableDataSet.LessonsDataTable LessonsTable;
		private static TimetableDataSet.LessonsPlacesDataTable LessonsPlacesTable;
		private static TimetableDataSet.StudentsDataTable StudentsTable;
		private static TimetableDataSet.SubjectsDataTable SubjectsTable;
		private static TimetableDataSet.TeachersDataTable TeachersTable;

		private static ClassesTableAdapter ClassesTableAdapter;
		private static ClassroomsTableAdapter ClassroomsTableAdapter;
		private static DaysTableAdapter DaysTableAdapter;
		private static HoursTableAdapter HoursTableAdapter;
		private static LessonsTableAdapter LessonsTableAdapter;
		private static LessonsPlacesTableAdapter LessonsPlacesTableAdapter;
		private static StudentsTableAdapter StudentsTableAdapter;
		private static SubjectsTableAdapter SubjectsTableAdapter;
		private static TeachersTableAdapter TeachersTableAdapter;

		#endregion
	}
}
