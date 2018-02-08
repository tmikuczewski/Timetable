using System.Runtime.Serialization;
using Timetable.DAL.Model.MySql;

namespace Timetable.Service.ViewModels
{
	[DataContract]
	public class DaysViewModel
	{
		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public int Number { get; set; }

		public DaysViewModel(days dayRow)
		{
			Id = dayRow.id;
			Name = dayRow.name;
			Number = dayRow.number;
		}
	}
}
