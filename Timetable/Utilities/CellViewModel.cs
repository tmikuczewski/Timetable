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
		public int HourId { get; set; }

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
		/// <param name="lessonPlaceRow"></param>
		public CellViewModel(TimetableDataSet.LessonsPlacesRow lessonPlaceRow)
		{
			if (lessonPlaceRow == null)
				return;

			DayId = lessonPlaceRow.DayId;
			HourId = lessonPlaceRow.HourId;

			if (lessonPlaceRow.ClassroomsRow != null)
			{
				ClassroomId = lessonPlaceRow.ClassroomId;
				ClassroomName = lessonPlaceRow.ClassroomsRow?.Name;
			}

			if (lessonPlaceRow.LessonsRow != null)
			{
				ClassId = lessonPlaceRow.LessonsRow.ClassId;
				ClassCodeName = lessonPlaceRow.LessonsRow.ClassesRow?.CodeName;
				ClassFriendlyName = lessonPlaceRow.LessonsRow.ClassesRow?.ToFriendlyString();
				ClassYear = lessonPlaceRow.LessonsRow.ClassesRow?.Year;
				LessonId = lessonPlaceRow.LessonId;
				TeacherFirstName = lessonPlaceRow.LessonsRow.TeachersRow?.FirstName;
				TeacherFriendlyName = lessonPlaceRow.LessonsRow.TeachersRow?.ToFriendlyString();
				TeacherLastName = lessonPlaceRow.LessonsRow.TeachersRow?.LastName;
				TeacherPesel = lessonPlaceRow.LessonsRow.TeacherPesel;
				SubjectId = lessonPlaceRow.LessonsRow.SubjectId;
				SubjectName = lessonPlaceRow.LessonsRow.SubjectsRow?.Name;
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
