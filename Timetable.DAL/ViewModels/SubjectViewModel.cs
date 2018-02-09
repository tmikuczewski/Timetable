using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.Models.MySql;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class SubjectViewModel
	{
		#region Fields

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		#endregion


		#region Constructors

		public SubjectViewModel()
		{
		}

		public SubjectViewModel(TimetableDataSet.SubjectsRow subjectRow)
		{
			Id = subjectRow.Id;
			Name = subjectRow.Name;
		}

		public SubjectViewModel(SubjectsRow subjectRow)
		{
			Id = subjectRow.Id;
			Name = subjectRow.Name;
		}

		#endregion
	}
}
