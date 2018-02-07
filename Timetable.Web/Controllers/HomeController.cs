using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Timetable.DAL.Model;
using Timetable.Web.ViewModels;

namespace Timetable.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{

			HomeViewModel home;

			using (var db = new TimetableModel())
			{

				home = new HomeViewModel
				{
					Classess = db.classes
						.OrderBy(c => c.year)
						.ThenBy(c => c.code_name)
						.ToList(),

					Teachers = db.teachers
						.OrderBy(t => t.last_name)
						.ThenBy(t => t.first_name)
						.ToList(),

					Classrooms = db.classrooms
						.OrderBy(cr => cr.name)
						.ToList()
				};
			}

			return View(home);
		}
	}
}
