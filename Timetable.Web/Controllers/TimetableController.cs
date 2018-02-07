using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Timetable.DAL.Model;
using Timetable.Web.ViewModels;

namespace Timetable.Web.Controllers
{
	public class TimetableController : Controller
	{
		public ActionResult Class(int id)
		{
			TimetableViewModel timetable;

			using (var db = new TimetableModel())
			{
				var currentClass = db.classes
					.FirstOrDefault(c => c.id == id);

				if (currentClass == null)
					return HttpNotFound();

				var lessonsPlaces = db.lessons_places
					.Where(lp => lp.lessons._class == id)
					.ToList();

				var cells = new List<CellViewModel>();

				foreach (var lp in lessonsPlaces)
					cells.Add(new CellViewModel(lp));

				timetable = new TimetableViewModel
				{
					LessonsPlaces = cells,
					Days = db.days.OrderBy(d => d.number).ToList(),
					Hours = db.hours.OrderBy(h => h.number).ToList(),
					CurrentClass = currentClass
				};
			}

			return View(timetable);
		}

		public ActionResult Teacher(string pesel)
		{
			TimetableViewModel timetable;

			using (var db = new TimetableModel())
			{
				var currentTeacher = db.teachers
					.FirstOrDefault(t => t.pesel == pesel);

				if (currentTeacher == null)
					return HttpNotFound();

				var lessonsPlaces = db.lessons_places
					.Where(lp => lp.lessons.teacher == pesel)
					.ToList();

				var cells = new List<CellViewModel>();

				foreach (var lp in lessonsPlaces)
					cells.Add(new CellViewModel(lp));

				timetable = new TimetableViewModel
				{
					LessonsPlaces = cells,
					Days = db.days.OrderBy(d => d.number).ToList(),
					Hours = db.hours.OrderBy(h => h.number).ToList(),
					CurrentTeacher = currentTeacher
				};
			}

			return View(timetable);
		}

		public ActionResult Classroom(int id)
		{
			TimetableViewModel timetable;

			using (var db = new TimetableModel())
			{
				var currentClassroom = db.classrooms
					.FirstOrDefault(cr => cr.id == id);

				if (currentClassroom == null)
					return HttpNotFound();

				var lessonsPlaces = db.lessons_places
					.Where(lp => lp.classroom == id)
					.ToList();

				var cells = new List<CellViewModel>();

				foreach (var lp in lessonsPlaces)
					cells.Add(new CellViewModel(lp));

				timetable = new TimetableViewModel
				{
					LessonsPlaces = cells,
					Days = db.days.OrderBy(d => d.number).ToList(),
					Hours = db.hours.OrderBy(h => h.number).ToList(),
					CurrentClassroom = currentClassroom
				};
			}

			return View(timetable);
		}
	}
}
