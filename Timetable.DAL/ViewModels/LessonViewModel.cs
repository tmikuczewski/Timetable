using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.Utilities;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class LessonViewModel
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

		#endregion


		#region Constructors

		public LessonViewModel()
		{
		}

		public LessonViewModel(TimetableDataSet.LessonsRow lessonRow)
		{
			Id = lessonRow.Id;
			ClassId = lessonRow.ClassId;
			ClassCodeName = lessonRow.ClassesRow?.CodeName;
			ClassFriendlyName = lessonRow.ClassesRow?.ToFriendlyString();
			ClassYear = lessonRow.ClassesRow?.Year;
			TeacherFirstName = lessonRow.TeachersRow?.FirstName;
			TeacherFriendlyName = lessonRow.TeachersRow?.ToFriendlyString();
			TeacherLastName = lessonRow.TeachersRow?.LastName;
			TeacherPesel = lessonRow.TeacherPesel;
			SubjectId = lessonRow.SubjectId;
			SubjectName = lessonRow.SubjectsRow?.Name;
		}

		public LessonViewModel(LessonsRow lessonRow)
		{
			Id = lessonRow.Id;
			ClassId = lessonRow.ClassId;
			ClassCodeName = lessonRow.Class?.CodeName;
			ClassFriendlyName = lessonRow.Class?.ToFriendlyString();
			ClassYear = lessonRow.Class?.Year;
			TeacherFirstName = lessonRow.Teacher?.FirstName;
			TeacherFriendlyName = lessonRow.Teacher?.ToFriendlyString();
			TeacherLastName = lessonRow.Teacher?.LastName;
			TeacherPesel = lessonRow.TeacherPesel;
			SubjectId = lessonRow.SubjectId;
			SubjectName = lessonRow.Subject?.Name;
		}

		#endregion
	}
}
