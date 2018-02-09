using System;
using System.Web.Mvc;
using Timetable.DAL.ViewModels;
using Timetable.Web.TimetableServiceReference;

namespace Timetable.Web.Controllers
{
	public class TimetableController : Controller
	{
		public ActionResult Class(int id)
		{
			TimetableViewModel timetableViewModel = null;

			try
			{
				var timetableServiceClient = new TimetableServiceClient();
				timetableViewModel = timetableServiceClient.GetTimetableForClass(id);
				timetableServiceClient.Close();

			}
			catch (Exception)
			{
				// ignored
			}

			if (timetableViewModel?.CurrentClass == null)
			{
				return HttpNotFound();
			}

			return View(timetableViewModel);
		}

		public ActionResult Teacher(string pesel)
		{
			TimetableViewModel timetableViewModel = null;

			try
			{
				var timetableServiceClient = new TimetableServiceClient();
				timetableViewModel = timetableServiceClient.GetTimetableForTeacher(pesel);
				timetableServiceClient.Close();
			}
			catch (Exception)
			{
				// ignored
			}

			if (timetableViewModel?.CurrentTeacher == null)
			{
				return HttpNotFound();
			}

			return View(timetableViewModel);
		}

		public ActionResult Classroom(int id)
		{
			TimetableViewModel timetableViewModel = null;

			try
			{
				var timetableServiceClient = new TimetableServiceClient();
				timetableViewModel = timetableServiceClient.GetTimetableForClassroom(id);
				timetableServiceClient.Close();
			}
			catch (Exception)
			{
				// ignored
			}

			if (timetableViewModel?.CurrentClassroom == null)
			{
				return HttpNotFound();
			}

			return View(timetableViewModel);
		}
	}
}
