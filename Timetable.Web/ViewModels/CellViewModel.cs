using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Timetable.DAL.Model;

namespace Timetable.Web.ViewModels
{
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

		#endregion


		#region Constructors

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
		public CellViewModel(lessons_places lessonsPlaceRow)
		{
			if (lessonsPlaceRow == null)
				return;

			if (lessonsPlaceRow.days != null)
			{
				DayId = lessonsPlaceRow.days.id;
				DayNumber = lessonsPlaceRow.days.number;
				DayName = lessonsPlaceRow.days.name;
			}

			if (lessonsPlaceRow.hours != null)
			{
				HourId = lessonsPlaceRow.hours.id;
				HourNumber = lessonsPlaceRow.hours.number;
				HourBegin = lessonsPlaceRow.hours.begin;
				HourEnd = lessonsPlaceRow.hours.end;
			}

			if (lessonsPlaceRow.classrooms != null)
			{
				ClassroomId = lessonsPlaceRow.classrooms.id;
				ClassroomName = lessonsPlaceRow.classrooms.name;
			}

			if (lessonsPlaceRow.lessons != null)
			{
				ClassId = lessonsPlaceRow.lessons._class;
				ClassCodeName = lessonsPlaceRow.lessons.classes?.code_name;
				ClassFriendlyName = lessonsPlaceRow.lessons.classes?.year + " " + lessonsPlaceRow.lessons.classes?.code_name;
				ClassYear = lessonsPlaceRow.lessons.classes?.year;
				LessonId = lessonsPlaceRow.lesson;
				TeacherFirstName = lessonsPlaceRow.lessons.teachers?.first_name;
				TeacherFriendlyName = lessonsPlaceRow.lessons.teachers?.first_name + " " + lessonsPlaceRow.lessons.teachers?.last_name;
				TeacherLastName = lessonsPlaceRow.lessons.teachers?.last_name;
				TeacherPesel = lessonsPlaceRow.lessons.teacher;
				SubjectId = lessonsPlaceRow.lessons.subject;
				SubjectName = lessonsPlaceRow.lessons.subjects?.name;
			}
		}

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
