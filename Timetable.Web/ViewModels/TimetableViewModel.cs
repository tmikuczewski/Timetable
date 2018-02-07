using System.Collections.Generic;
using Timetable.DAL.Model;

namespace Timetable.Web.ViewModels
{
	public class TimetableViewModel
	{
		public IList<CellViewModel> LessonsPlaces { get; set; }
		public IList<days> Days { get; set; }
		public IList<hours> Hours { get; set; }
		public classes CurrentClass { get; set; }
		public teachers CurrentTeacher { get; set; }
		public classrooms CurrentClassroom { get; set; }
	}
}
