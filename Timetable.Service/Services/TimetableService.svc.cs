using System.Collections.Generic;
using System.Linq;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.ViewModels;
using Timetable.Service.Interfaces;

namespace Timetable.Service.Services
{
	public class TimetableService : ITimetableService
	{
		public TimetableViewModel GetTimetableEntities()
		{
			var timetableViewModel = new TimetableViewModel
			{
				Classes = new List<ClassViewModel>(),
				Teachers = new List<TeacherViewModel>(),
				Classrooms = new List<ClassroomViewModel>()
			};

			using (var db = new TimetableModel())
			{
				db.Classes
					.OrderBy(c => c.Year)
					.ThenBy(c => c.CodeName)
					.ToList()
					.ForEach(c => timetableViewModel.Classes.Add(new ClassViewModel(c)));

				db.Teachers
					.OrderBy(t => t.LastName)
					.ThenBy(t => t.FirstName)
					.ToList()
					.ForEach(t => timetableViewModel.Teachers.Add(new TeacherViewModel(t)));

				db.Classrooms
					.OrderBy(cr => cr.Name)
					.ToList()
					.ForEach(cr => timetableViewModel.Classrooms.Add(new ClassroomViewModel(cr)));
			}

			return timetableViewModel;
		}

		public TimetableViewModel GetTimetableForClass(int id)
		{
			TimetableViewModel timetableViewModel;

			using (var db = new TimetableModel())
			{
				timetableViewModel = GetPrefilledTimetableViewModel(db);

				var currentClass = db.Classes
					.FirstOrDefault(c => c.Id == id);

				timetableViewModel.CurrentClass = new ClassViewModel(currentClass);

				db.LessonsPlaces
					.Where(lp => lp.Lesson.ClassId == id)
					.ToList()
					.ForEach(lp => timetableViewModel.CurrentLessonsPlaces.Add(new LessonsPlaceViewModel(lp)));
			}

			return timetableViewModel;
		}

		public TimetableViewModel GetTimetableForTeacher(string pesel)
		{
			TimetableViewModel timetableViewModel;

			using (var db = new TimetableModel())
			{
				timetableViewModel = GetPrefilledTimetableViewModel(db);

				var currentTeacher = db.Teachers
					.FirstOrDefault(t => t.Pesel == pesel);

				timetableViewModel.CurrentTeacher = new TeacherViewModel(currentTeacher);

				db.LessonsPlaces
					.Where(lp => lp.Lesson.TeacherPesel == pesel)
					.ToList()
					.ForEach(lp => timetableViewModel.CurrentLessonsPlaces.Add(new LessonsPlaceViewModel(lp)));
			}

			return timetableViewModel;
		}

		public TimetableViewModel GetTimetableForClassroom(int id)
		{
			TimetableViewModel timetableViewModel;

			using (var db = new TimetableModel())
			{
				timetableViewModel = GetPrefilledTimetableViewModel(db);

				var currentClassroom = db.Classrooms
					.FirstOrDefault(cr => cr.Id == id);

				timetableViewModel.CurrentClassroom = new ClassroomViewModel(currentClassroom);

				db.LessonsPlaces
					.Where(lp => lp.ClassroomId == id)
					.ToList()
					.ForEach(lp => timetableViewModel.CurrentLessonsPlaces.Add(new LessonsPlaceViewModel(lp)));
			}

			return timetableViewModel;
		}

		private TimetableViewModel GetPrefilledTimetableViewModel(TimetableModel db)
		{
			var timetableViewModel = new TimetableViewModel
			{
				Days = new List<DayViewModel>(),
				Hours = new List<HourViewModel>(),
				CurrentLessonsPlaces = new List<LessonsPlaceViewModel>()
			};

			db.Days
				.OrderBy(d => d.Number)
				.ToList()
				.ForEach(d => timetableViewModel.Days.Add(new DayViewModel(d)));

			db.Hours
				.OrderBy(h => h.Number)
				.ToList()
				.ForEach(h => timetableViewModel.Hours.Add(new HourViewModel(h)));

			return timetableViewModel;
		}
	}
}
