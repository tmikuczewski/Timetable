using System;
using Timetable.Client.DaysServiceReference;
using Timetable.Client.HoursServiceReference;

namespace Timetable.Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			Console.WriteLine("##### Timetable #####");


			var daysServiceClient = new DaysServiceClient();

			Console.WriteLine("\nDays in database:");

			try
			{
				foreach (var day in daysServiceClient.GetAllDays())
					Console.WriteLine(day.Name);
			}
			catch (Exception)
			{
				// ignored
			}

			daysServiceClient.Close();


			var hourServiceClient = new HoursServiceClient();

			Console.WriteLine("\nHours in database:");

			try
			{
				foreach (var hour in hourServiceClient.GetAllHours())
					Console.WriteLine(hour.Number + ") " + hour.Begin.ToString(@"hh\:mm") + " - " + hour.End.ToString(@"hh\:mm"));
			}
			catch (Exception)
			{
				// ignored
			}

			hourServiceClient.Close();

			Console.ReadKey();
		}
	}
}
