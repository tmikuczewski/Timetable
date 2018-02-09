using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.ViewModels;

namespace Timetable.Web.Controllers
{
	public class TimetableController : Controller
	{
		public ActionResult Class(int id)
		{
			TimetableViewModel timetableViewModel;

			using (var db = new TimetableModel())
			{
				var currentClass = db.Classes
					.FirstOrDefault(c => c.Id == id);

				if (currentClass == null)
					return HttpNotFound();

				timetableViewModel = GetPrefilledTimetableViewModel(db);
				timetableViewModel.CurrentClass = new ClassViewModel(currentClass);

				db.LessonsPlaces
					.Where(lp => lp.Lesson.ClassId == id)
					.ToList()
					.ForEach(lp => timetableViewModel.CurrentLessonsPlaces.Add(new LessonsPlaceViewModel(lp)));
			}

			return View(timetableViewModel);
		}

		public ActionResult Teacher(string pesel)
		{
			TimetableViewModel timetableViewModel;

			using (var db = new TimetableModel())
			{
				var currentTeacher = db.Teachers
					.FirstOrDefault(t => t.Pesel == pesel);

				if (currentTeacher == null)
					return HttpNotFound();

				timetableViewModel = GetPrefilledTimetableViewModel(db);
				timetableViewModel.CurrentTeacher = new TeacherViewModel(currentTeacher);

				db.LessonsPlaces
					.Where(lp => lp.Lesson.TeacherPesel == pesel)
					.ToList()
					.ForEach(lp => timetableViewModel.CurrentLessonsPlaces.Add(new LessonsPlaceViewModel(lp)));
			}

			return View(timetableViewModel);
		}

		public ActionResult Classroom(int id)
		{
			TimetableViewModel timetableViewModel;

			using (var db = new TimetableModel())
			{
				var currentClassroom = db.Classrooms
					.FirstOrDefault(cr => cr.Id == id);

				if (currentClassroom == null)
					return HttpNotFound();

				timetableViewModel = GetPrefilledTimetableViewModel(db);
				timetableViewModel.CurrentClassroom = new ClassroomViewModel(currentClassroom);

				db.LessonsPlaces
					.Where(lp => lp.ClassroomId == id)
					.ToList()
					.ForEach(lp => timetableViewModel.CurrentLessonsPlaces.Add(new LessonsPlaceViewModel(lp)));
			}

			return View(timetableViewModel);
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
