using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class TimetableViewModel
	{
		#region Fields

		[DataMember]
		public IList<ClassViewModel> Classes { get; set; }

		[DataMember]
		public IList<ClassroomViewModel> Classrooms { get; set; }

		[DataMember]
		public IList<DayViewModel> Days { get; set; }

		[DataMember]
		public IList<HourViewModel> Hours { get; set; }

		[DataMember]
		public IList<LessonViewModel> Lessons { get; set; }

		[DataMember]
		public IList<LessonsPlaceViewModel> LessonsPlaces { get; set; }

		[DataMember]
		public IList<StudentViewModel> Students { get; set; }

		[DataMember]
		public IList<SubjectViewModel> Subjects { get; set; }

		[DataMember]
		public IList<TeacherViewModel> Teachers { get; set; }

		[DataMember]
		public ClassViewModel CurrentClass { get; set; }

		[DataMember]
		public ClassroomViewModel CurrentClassroom { get; set; }

		[DataMember]
		public TeacherViewModel CurrentTeacher { get; set; }

		[DataMember]
		public IList<LessonsPlaceViewModel> CurrentLessonsPlaces { get; set; }

		#endregion


		#region Constructors

		#endregion
	}
}
