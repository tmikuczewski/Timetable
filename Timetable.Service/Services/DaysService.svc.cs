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
			var days = new List<DayViewModel>();

			using (var db = new TimetableModel())
			{
				db.Days
					.OrderBy(d => d.Number)
					.ToList()
					.ForEach(d => days.Add(new DayViewModel(d)));
			}

			return days;
		}
	}
}
