using System.Collections.Generic;
using System.Linq;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.ViewModels;
using Timetable.Service.Interfaces;

namespace Timetable.Service.Services
{
	public class DaysService : IDaysService
	{
		public IList<DayViewModel> GetAllDays()
		{
			using (var db = new TimetableModel())
			{
				var days = new List<DayViewModel>();

				foreach (var day in db.Days.OrderBy(d => d.Number))
					days.Add(new DayViewModel(day));

				return days;
			}
		}
	}
}
