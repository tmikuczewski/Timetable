﻿using System;
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
				yield return new Teacher(new Pesel(row.Pesel), row.FirstName, row.LastName);
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
				Pesel pesel = (row.TutorPesel == null) ? null : new Pesel(row.TutorPesel);
				yield return new Class(row.Id, row.Year, row.CodeName, pesel);
			}
			yield break;
		}

		/// <summary>
		/// Przykładowa implementacja metody wykonującej jedno z zapytań.</summary>
		/// <returns>Lista wszystkich klas (<c>IEnumerable&lt;Models.Subject&gt;</c>)</returns>
		public static IEnumerable<Subject> GetSubjects()
		{
			foreach (var row in SubjectsTable)
			{
				yield return new Subject(row.Id, row.Name);
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
			DataTable st = StudentsTable;

			if (existingStudentRow == null)
			{
				throw new EntityDoesNotExistException();
			}
			Console.WriteLine("pesel " + pesel);
			Console.WriteLine("firstName " + firstName);
			Console.WriteLine("lastName " + lastName);
			existingStudentRow.BeginEdit();
			existingStudentRow.FirstName = firstName;
			existingStudentRow.LastName = lastName;
			existingStudentRow.EndEdit();


			StudentsTable.AcceptChanges();

			//StudentsTableAdapter.Update()
			StudentsTableAdapter.Update(pesel, firstName, lastName, existingStudentRow.ClassId, existingStudentRow.Pesel, existingStudentRow.ClassId);
			//return 1;
			return StudentsTableAdapter.Update(StudentsTable);
		}

		/// <summary>
		/// Usunięcie ucznia z bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int DeleteStudent(string pesel)
		{
			TimetableDataSet.StudentsRow existingStudentRow = StudentsTable.FindByPesel(pesel);

			if (existingStudentRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			StudentsTableAdapter.Delete(existingStudentRow.Pesel, existingStudentRow.ClassId);

			existingStudentRow.Delete();
			StudentsTable.AcceptChanges();
			return StudentsTableAdapter.Update(StudentsTable);
		}

		/// <summary>
		/// Pobranie danych nauczyciela z bazy danych.</summary>
		/// <returns>Obiekt typu <c>Teacher</c>.</returns>
		public static Teacher GetTeacherByPesel(string pesel)
		{
			TimetableDataSet.TeachersRow existingTeacherRow = TeachersTable.FindByPesel(pesel);

			if (existingTeacherRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return new Teacher(new Pesel(existingTeacherRow.Pesel), existingTeacherRow.FirstName, existingTeacherRow.LastName);
		}

		/// <summary>
		/// Dodanie nowego nauczyciela do bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int AddTeacher(string pesel, string firstName, string lastName)
		{
			TimetableDataSet.TeachersRow newTeacherRow = TeachersTable.NewTeachersRow();
			newTeacherRow.Pesel = pesel;
			newTeacherRow.FirstName = firstName;
			newTeacherRow.LastName = lastName;

			TimetableDataSet.TeachersRow existingTeacherRow = TeachersTable.FindByPesel(pesel);

			if (existingTeacherRow != null)
			{
				throw new DuplicateEntityException();
			}

			TeachersTable.Rows.Add(newTeacherRow);

			return TeachersTableAdapter.Update(TeachersTable);
		}

		/// <summary>
		/// Edycja danych nauczyciela w bazie danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int EditTeacher(string pesel, string firstName, string lastName)
		{
			TimetableDataSet.TeachersRow existingTeacherRow = TeachersTable.FindByPesel(pesel);

			if (existingTeacherRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			existingTeacherRow.FirstName = firstName;
			existingTeacherRow.LastName = lastName;
			TeachersTable.AcceptChanges();
			TeachersTableAdapter.Update(existingTeacherRow.Pesel, firstName, lastName, existingTeacherRow.Pesel);

			return TeachersTableAdapter.Update(TeachersTable);
		}

		/// <summary>
		/// Usunięcie nauczyciela z bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int DeleteTeacher(string pesel)
		{
			TimetableDataSet.TeachersRow existingTeacherRow = TeachersTable.FindByPesel(pesel);

			if (existingTeacherRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			TeachersTableAdapter.Delete(existingTeacherRow.Pesel);
			existingTeacherRow.Delete();
			TeachersTable.AcceptChanges();
			
			return TeachersTableAdapter.Update(TeachersTable);
		}

		/// <summary>
		/// Pobranie danych klasy z bazy danych.</summary>
		/// <returns>Obiekt typu <c>Class</c>.</returns>
		public static Class GetClassById(int id)
		{
			TimetableDataSet.ClassesRow existingClassRow = ClassesTable.FindById(id);

			if (existingClassRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return new Class(existingClassRow.Id, existingClassRow.Year, existingClassRow.CodeName, null);
		}

		/// <summary>
		/// Dodanie nowej klasy do bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int AddClass(int year, string codeName)
		{
			TimetableDataSet.ClassesRow newClassRow = ClassesTable.NewClassesRow();
			newClassRow.Year = year;
			newClassRow.CodeName = codeName;
			newClassRow.SetTutorPeselNull();

			ClassesTable.Rows.Add(newClassRow);

			return ClassesTableAdapter.Update(ClassesTable);
		}

		/// <summary>
		/// Edycja danych klasy w bazie danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int EditClass(int id, int year, string codeName)
		{
			TimetableDataSet.ClassesRow existingClassRow = ClassesTable.FindById(id);

			if (existingClassRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			existingClassRow.Year = year;
			existingClassRow.CodeName = codeName;
			ClassesTable.AcceptChanges();
			ClassesTableAdapter.Update(year, codeName, existingClassRow.TutorPesel, existingClassRow.Id, existingClassRow.Year, existingClassRow.TutorPesel);
			return ClassesTableAdapter.Update(ClassesTable);
		}

		/// <summary>
		/// Usunięcie klasy z bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int DeleteClass(int id)
		{
			TimetableDataSet.ClassesRow existingClassRow = ClassesTable.FindById(id);

			if (existingClassRow == null)
			{
				throw new EntityDoesNotExistException();
			}
			ClassesTableAdapter.Delete(existingClassRow.Id, existingClassRow.Year, existingClassRow.TutorPesel ?? "");

			existingClassRow.Delete();
			ClassesTable.AcceptChanges();

			return ClassesTableAdapter.Update(ClassesTable);
		}

		/// <summary>
		/// Pobranie danych przedmiotu z bazy danych.</summary>
		/// <returns>Obiekt typu <c>Subject</c>.</returns>
		public static Subject GetSubjectById(int id)
		{
			TimetableDataSet.SubjectsRow existingSubjectRow = SubjectsTable.FindById(id);

			if (existingSubjectRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return new Subject(existingSubjectRow.Id, existingSubjectRow.Name);
		}

		/// <summary>
		/// Dodanie nowego przedmiotu do bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int AddSubject(string name)
		{
			TimetableDataSet.SubjectsRow newSubjectRow = SubjectsTable.NewSubjectsRow();
			newSubjectRow.Name = name;

			SubjectsTable.Rows.Add(newSubjectRow);

			return SubjectsTableAdapter.Update(SubjectsTable);
		}

		/// <summary>
		/// Edycja danych przedmiotu w bazie danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int EditSubject(int id, string name)
		{
			TimetableDataSet.SubjectsRow existingSubjectRow = SubjectsTable.FindById(id);

			if (existingSubjectRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			existingSubjectRow.Name = name;
			SubjectsTable.AcceptChanges();
			SubjectsTableAdapter.Update(name, existingSubjectRow.Id);
			return SubjectsTableAdapter.Update(SubjectsTable);
		}

		/// <summary>
		/// Usunięcie przedmiotu z bazy danych.</summary>
		/// <returns>Ilość zmienionych wierszy.</returns>
		public static int DeleteSubject(int id)
		{
			TimetableDataSet.SubjectsRow existingSubjectRow = SubjectsTable.FindById(id);

			if (existingSubjectRow == null)
			{
				throw new EntityDoesNotExistException();
			}
			SubjectsTableAdapter.Delete(existingSubjectRow.Id);
			existingSubjectRow.Delete();
			SubjectsTable.AcceptChanges();

			return SubjectsTableAdapter.Update(SubjectsTable);
		}

		/// <summary>
		/// Pobranie danych sali z bazy danych.</summary>
		/// <returns>Obiekt typu <c>Classroom</c>.</returns>
		public static Classroom GetClassroomById(int id)
		{
			TimetableDataSet.ClassroomsRow existingClassroomRow = ClassroomsTable.FindById(id);
		
			if (existingClassroomRow == null)
			{
				throw new EntityDoesNotExistException();
			}
		
			return new Classroom(existingClassroomRow.Id,
				existingClassroomRow.Name,
				new Pesel(existingClassroomRow.AdministratorPesel));
		}
		
		/// <summary>
		/// Pobranie danych dnia tygodnia z bazy danych.</summary>
		/// <returns>Obiekt typu <c>Day</c>.</returns>
		public static Day GetDayById(int id)
		{
			TimetableDataSet.DaysRow existingDayRow = DaysTable.FindById(id);
		
			if (existingDayRow == null)
			{
				throw new EntityDoesNotExistException();
			}
		
			return new Day(existingDayRow.Id, existingDayRow.Name);
		}
		
		/// <summary>
		/// Pobranie danych bloku godzinowego z bazy danych.</summary>
		/// <returns>Obiekt typu <c>Hour</c>.</returns>
		public static Hour GetHourById(int id)
		{
			TimetableDataSet.HoursRow existingHourRow = HoursTable.FindById(id);
		
			if (existingHourRow == null)
			{
				throw new EntityDoesNotExistException();
			}
		
			return new Hour(existingHourRow.Id, existingHourRow.Hour);
		}
		
		/// <summary>
		/// Przykładowa implementacja metody wykonującej jedno z zapytań.</summary>
		/// <returns>Lista przedmiotów dla danej klasy (<c>IEnumerable&lt;Models.Lesson&gt;</c>)</returns>
		public static IEnumerable<Lesson> GetLessonsForClass(int classId)
		{
			foreach (var row in LessonsTable.Where(l => l.ClassId == classId))
			{
				Lesson lesson = new Lesson();
				lesson.Id = row.Id;
				lesson.Teacher = GetTeacherByPesel(row.TeacherPesel);
				lesson.Subject = GetSubjectById(row.SubjectId);
				lesson.Class = GetClassById(row.ClassId);
				yield return lesson;
			}
			yield break;
		}
		
		/// <summary>
		/// Przykładowa implementacja metody wykonującej jedno z zapytań.</summary>
		/// <returns>Lista lekcji dla danej klasy (<c>IEnumerable&lt;Models.LessonPlace&gt;</c>)</returns>
		public static IEnumerable<LessonPlace> GetTimetableForClass(int classId)
		{
			var lessonsList = GetLessonsForClass(classId);
			var lessonsIdList = lessonsList.Select(row => row.Id);
		
			foreach (var row in LessonsPlacesTable.Where(l => lessonsIdList.Contains(l.LessonId)))
			{
				LessonPlace lp = new LessonPlace();
				lp.Lesson = lessonsList.FirstOrDefault(l => l.Id == row.LessonId);
				lp.Classroom = GetClassroomById(row.ClassroomId);
				lp.Day = GetDayById(row.DayId);
				lp.Hour = GetHourById(row.HourId);
				yield return lp;
			}
		
			yield break;
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
