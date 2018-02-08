using System;
using System.Runtime.Serialization;
using Timetable.DAL.Model;

namespace Timetable.Service.ViewModels
{
	[DataContract]
	public class HoursViewModel
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public TimeSpan Begin { get; set; }

		[DataMember]
		public TimeSpan End { get; set; }

		[DataMember]
		public int Number { get; set; }

		public HoursViewModel(hours hourRow)
		{
			Id = hourRow.id;
			Begin = hourRow.begin;
			End = hourRow.end;
			Number = hourRow.number;
		}
	}
}
