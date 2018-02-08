using System.Collections.Generic;
using System.Linq;
using Timetable.DAL.Model.MySql;
using Timetable.Service.Interfaces;
using Timetable.Service.ViewModels;

namespace Timetable.Service.Services
{
	public class DaysService : IDaysService
	{
		public IList<DaysViewModel> GetAllDays()
		{
			using (var db = new TimetableModel())
			{
				var days = new List<DaysViewModel>();

				foreach (var day in db.days.OrderBy(d => d.number))
					days.Add(new DaysViewModel(day));

				return days;
			}
		}
	}
}
