using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSets.MySql;
using Timetable.DAL.Models.MySql;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class DayViewModel
	{
		#region Fields

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int Number { get; set; }

		#endregion


		#region Constructors

		public DayViewModel()
		{
		}

		public DayViewModel(TimetableDataSet.DaysRow dayRow)
		{
			Id = dayRow.Id;
			Name = dayRow.Name;
			Number = dayRow.Number;
		}

		public DayViewModel(DaysRow dayRow)
		{
			Id = dayRow.Id;
			Name = dayRow.Name;
			Number = dayRow.Number;
		}

		#endregion
	}
}
