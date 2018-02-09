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
			using (var db = new TimetableModel())
			{
				var hours = new List<HourViewModel>();

				foreach (var hour in db.Hours.OrderBy(d => d.Number))
					hours.Add(new HourViewModel(hour));

				return hours;
			}
		}
	}
}
