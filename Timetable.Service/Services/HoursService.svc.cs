using System;
using System.Collections.Generic;
using System.Linq;
using Timetable.DAL.Model;

namespace Timetable.Service.Services
{
	public class HoursService : IHourService
	{
		public IList<hours> GetAllHours()
		{
			using (var db = new TimetableModel())
			{
				List<hours> allHours = db.hours.OrderBy(h => h.number).ToList();
				return allHours;
			}
		}
	}
}
