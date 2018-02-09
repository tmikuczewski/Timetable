using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSets.MySql;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.Utilities;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class LessonsPlaceViewModel
	{
		#region Fields

		[DataMember]
		public int? Id { get; set; }

		[DataMember]
		public int? ClassId { get; set; }

		[DataMember]
		public string ClassCodeName { get; set; }

		[DataMember]
		public string ClassFriendlyName { get; set; }

		[DataMember]
		public int? ClassYear { get; set; }

		[DataMember]
		public int? ClassroomId { get; set; }

		[DataMember]
		public string ClassroomName { get; set; }

		[DataMember]
		public int DayId { get; set; }

		[DataMember]
		public int DayNumber { get; set; }

		[DataMember]
		public string DayName { get; set; }

		[DataMember]
		public int HourId { get; set; }

		[DataMember]
		public int HourNumber { get; set; }

		[DataMember]
		public TimeSpan HourBegin { get; set; }

		[DataMember]
		public TimeSpan HourEnd { get; set; }

		[DataMember]
		public int? LessonId { get; set; }

		[DataMember]
		public string TeacherFirstName { get; set; }

		[DataMember]
		public string TeacherFriendlyName { get; set; }

		[DataMember]
		public string TeacherLastName { get; set; }

		[DataMember]
		public string TeacherPesel { get; set; }

		[DataMember]
		public int? SubjectId { get; set; }

		[DataMember]
		public string SubjectName { get; set; }

		[DataMember]
		public string SubjectClass
		{
			get { return $"{SubjectName}\n-- {ClassFriendlyName}"; }
			private set { }
		}

		[DataMember]
		public string SubjectTeacher
		{
			get { return $"{SubjectName}\n-- {TeacherFriendlyName}"; }
			private set { }
		}
		#endregion


		#region Constructors

		public LessonsPlaceViewModel()
		{
		}

		public LessonsPlaceViewModel(TimetableDataSet.LessonsPlacesRow lessonsPlaceRow)
		{
			if (lessonsPlaceRow == null)
			{
				return;
			}

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
				ClassroomId = lessonsPlaceRow.ClassroomsRow.Id;
				ClassroomName = lessonsPlaceRow.ClassroomsRow.Name;
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

		public LessonsPlaceViewModel(LessonsPlacesRow lessonsPlaceRow)
		{
			if (lessonsPlaceRow == null)
			{
				return;
			}

			if (lessonsPlaceRow.Day != null)
			{
				DayId = lessonsPlaceRow.Day.Id;
				DayNumber = lessonsPlaceRow.Day.Number;
				DayName = lessonsPlaceRow.Day.Name;
			}

			if (lessonsPlaceRow.Hour != null)
			{
				HourId = lessonsPlaceRow.Hour.Id;
				HourNumber = lessonsPlaceRow.Hour.Number;
				HourBegin = lessonsPlaceRow.Hour.Begin;
				HourEnd = lessonsPlaceRow.Hour.End;
			}

			if (lessonsPlaceRow.Classroom != null)
			{
				ClassroomId = lessonsPlaceRow.Classroom.Id;
				ClassroomName = lessonsPlaceRow.Classroom.Name;
			}

			if (lessonsPlaceRow.Lesson != null)
			{
				ClassId = lessonsPlaceRow.Lesson.ClassId;
				ClassCodeName = lessonsPlaceRow.Lesson.Class?.CodeName;
				ClassFriendlyName = lessonsPlaceRow.Lesson.Class?.ToFriendlyString();
				ClassYear = lessonsPlaceRow.Lesson.Class?.Year;
				LessonId = lessonsPlaceRow.LessonId;
				TeacherFirstName = lessonsPlaceRow.Lesson.Teacher?.FirstName;
				TeacherFriendlyName = lessonsPlaceRow.Lesson.Teacher?.ToFriendlyString();
				TeacherLastName = lessonsPlaceRow.Lesson.Teacher?.LastName;
				TeacherPesel = lessonsPlaceRow.Lesson.TeacherPesel;
				SubjectId = lessonsPlaceRow.Lesson.SubjectId;
				SubjectName = lessonsPlaceRow.Lesson.Subject?.Name;
			}
		}

		#endregion
	}
}
