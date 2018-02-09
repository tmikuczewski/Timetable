using System.Collections.Generic;
using System.Linq;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.ViewModels;
using Timetable.Service.Interfaces;

namespace Timetable.Service.Services
{
	public class HoursService : IHoursService
	{
		public IList<HourViewModel> GetAllHours()
		{
			var hours = new List<HourViewModel>();

			using (var db = new TimetableModel())
			{
				db.Hours
					.OrderBy(h => h.Number)
					.ToList()
					.ForEach(h => hours.Add(new HourViewModel(h)));
			}

			return hours;
		}
	}
}
