using System.Collections.Generic;
using System.Linq;
using Timetable.DAL.Model;
using Timetable.Service.Interfaces;
using Timetable.Service.ViewModels;

namespace Timetable.Service.Services
{
	public class HoursService : IHoursService
	{
		public IList<HoursViewModel> GetAllHours()
		{
			using (var db = new TimetableModel())
			{
				var hours = new List<HoursViewModel>();

				foreach (var hour in db.hours.OrderBy(d => d.number))
					hours.Add(new HoursViewModel(hour));

				return hours;
			}
		}
	}
}
