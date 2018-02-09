using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.Models.MySql;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class HourViewModel
	{
		#region Fields

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public TimeSpan Begin { get; set; }

		[DataMember]
		public TimeSpan End { get; set; }

		[DataMember]
		public int Number { get; set; }

		#endregion


		#region Constructors

		public HourViewModel()
		{
		}

		public HourViewModel(TimetableDataSet.HoursRow hourRow)
		{
			Id = hourRow.Id;
			Begin = hourRow.Begin;
			End = hourRow.End;
			Number = hourRow.Number;
		}

		public HourViewModel(HoursRow hourRow)
		{
			Id = hourRow.Id;
			Begin = hourRow.Begin;
			End = hourRow.End;
			Number = hourRow.Number;
		}

		#endregion
	}
}
