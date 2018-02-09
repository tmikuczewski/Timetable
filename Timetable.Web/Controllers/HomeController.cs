using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Timetable.DAL.ViewModels;
using Timetable.Web.TimetableServiceReference;

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

			try
			{
				var timetableServiceClient = new TimetableServiceClient();
				timetableViewModel = timetableServiceClient.GetTimetableEntities();
				timetableServiceClient.Close();
			}
			catch (Exception)
			{
				// ignored
			}

			return View(timetableViewModel);
		}
	}
}
