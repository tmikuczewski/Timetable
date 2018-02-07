using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timetable.Client.DaysServiceReference;
using Timetable.Client.HoursServiceReference;

namespace Timetable.Client
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("##### Timetable #####");


			DayServiceClient dayServiceClient = new DayServiceClient();

			Console.WriteLine("\nDays in database:");

			try
			{
				foreach (var day in dayServiceClient.GetAllDays())
					Console.WriteLine(day.number + ") " + day.name);
			}
			catch (Exception)
			{
			}

			dayServiceClient.Close();


			HourServiceClient hourServiceClient = new HourServiceClient();

			Console.WriteLine("\nHours in database:");

			try
			{
				foreach (var hour in hourServiceClient.GetAllHours())
					Console.WriteLine(hour.number + ") " + hour.begin.ToString(@"hh\:mm") + " - " + hour.end.ToString(@"hh\:mm"));

			}
			catch (Exception)
			{
			}

			hourServiceClient.Close();

			Console.ReadKey();
		}
	}
}
