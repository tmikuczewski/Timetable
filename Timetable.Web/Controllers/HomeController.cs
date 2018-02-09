using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.ViewModels;

namespace Timetable.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
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

			return View(timetableViewModel);
		}
	}
}
