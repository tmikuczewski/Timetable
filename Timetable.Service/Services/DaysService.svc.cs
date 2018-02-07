using System;
using System.Collections.Generic;
using System.Linq;
using Timetable.DAL.Model;

namespace Timetable.Service.Services
{
	public class DaysService : IDayService
	{
		public IList<days> GetAllDays()
		{
			using (var db = new TimetableModel())
			{
				List<days> allDays = db.days.OrderBy(d => d.number).ToList();
				return allDays;
			}
		}
	}
}
