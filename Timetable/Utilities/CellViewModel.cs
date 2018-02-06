using System;

namespace Timetable.Utilities
{
	/// <summary>
	///     Klasa przechowująca informacje o zaplanowanej lekcji.
	/// </summary>
	public class CellViewModel
	{
		#region Constants and Statics

		#endregion


		#region Fields

		/// <summary>
		///     
		/// </summary>
		public int? Id { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int? ClassId { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string ClassCodeName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string ClassFriendlyName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int? ClassYear { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int? ClassroomId { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string ClassroomName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int DayId { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int DayNumber { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string DayName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int HourId { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int HourNumber { get; set; }

		/// <summary>
		///     
		/// </summary>
		public TimeSpan HourBegin { get; set; }

		/// <summary>
		///     
		/// </summary>
		public TimeSpan HourEnd { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int? LessonId { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string TeacherFirstName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string TeacherFriendlyName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string TeacherLastName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string TeacherPesel { get; set; }

		/// <summary>
		///     
		/// </summary>
		public int? SubjectId { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string SubjectName { get; set; }

		/// <summary>
		///     
		/// </summary>
		public string SubjectClass => $"{SubjectName}\n-- {ClassFriendlyName}";

		/// <summary>
		///     
		/// </summary>
		public string SubjectTeacher => $"{SubjectName}\n-- {ClassFriendlyName}";

		#endregion


		#region Properties


		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>CellViewModel</c>.
		/// </summary>
		public CellViewModel()
		{
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>CellViewModel</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="lessonsPlaceRow"></param>
		public CellViewModel(TimetableDataSet.LessonsPlacesRow lessonsPlaceRow)
		{
			if (lessonsPlaceRow == null)
				return;

			if (lessonsPlaceRow.DaysRow != null)
			{
				DayId = lessonsPlaceRow.DaysRow.Id;
				DayNumber = lessonsPlaceRow.DaysRow.Number;
				DayName = lessonsPlaceRow.DaysRow.Name;
			}

			if (lessonsPlaceRow.HoursRow != null)
			{
				HourId = lessonsPlaceRow.HoursRow.Id;
				HourNumber = lessonsPlaceRow.HoursRow.Number;
				HourBegin = lessonsPlaceRow.HoursRow.Begin;
				HourEnd = lessonsPlaceRow.HoursRow.End;
			}

			if (lessonsPlaceRow.ClassroomsRow != null)
			{
				ClassroomId = lessonsPlaceRow.ClassroomId;
				ClassroomName = lessonsPlaceRow.ClassroomsRow?.Name;
			}

			if (lessonsPlaceRow.LessonsRow != null)
			{
				ClassId = lessonsPlaceRow.LessonsRow.ClassId;
				ClassCodeName = lessonsPlaceRow.LessonsRow.ClassesRow?.CodeName;
				ClassFriendlyName = lessonsPlaceRow.LessonsRow.ClassesRow?.ToFriendlyString();
				ClassYear = lessonsPlaceRow.LessonsRow.ClassesRow?.Year;
				LessonId = lessonsPlaceRow.LessonId;
				TeacherFirstName = lessonsPlaceRow.LessonsRow.TeachersRow?.FirstName;
				TeacherFriendlyName = lessonsPlaceRow.LessonsRow.TeachersRow?.ToFriendlyString();
				TeacherLastName = lessonsPlaceRow.LessonsRow.TeachersRow?.LastName;
				TeacherPesel = lessonsPlaceRow.LessonsRow.TeacherPesel;
				SubjectId = lessonsPlaceRow.LessonsRow.SubjectId;
				SubjectName = lessonsPlaceRow.LessonsRow.SubjectsRow?.Name;
			}
		}

		#endregion


		#region Constructors

		#endregion


		#region Events

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		#endregion

	}
}
