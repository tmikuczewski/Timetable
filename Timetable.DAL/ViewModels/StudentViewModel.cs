using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.Utilities;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class StudentViewModel
	{
		#region Fields

		[DataMember]
		public string Pesel { get; set; }

		[DataMember]
		public string FirstName { get; set; }

		[DataMember]
		public string FriendlyName { get; set; }

		[DataMember]
		public string LastName { get; set; }

		[DataMember]
		public string ClassCodeName { get; set; }

		[DataMember]
		public string ClassFriendlyName { get; set; }

		[DataMember]
		public int? ClassId { get; set; }

		[DataMember]
		public int ClassYear { get; set; }

		#endregion


		#region Constructors

		public StudentViewModel()
		{
		}

		public StudentViewModel(TimetableDataSet.StudentsRow studentRow)
		{
			Pesel = studentRow.Pesel;
			FirstName = studentRow.FirstName;
			FriendlyName = studentRow.ToFriendlyString();
			LastName = studentRow.LastName;

			if (studentRow.ClassesRow != null)
			{
				ClassCodeName = studentRow.ClassesRow.CodeName;
				ClassFriendlyName = studentRow.ClassesRow.ToFriendlyString();
				ClassId = studentRow.ClassId;
				ClassYear = studentRow.ClassesRow.Year;
			}
		}

		public StudentViewModel(StudentsRow studentRow)
		{
			Pesel = studentRow.Pesel;
			FirstName = studentRow.FirstName;
			FriendlyName = studentRow.ToFriendlyString();
			LastName = studentRow.LastName;

			if (studentRow.Class != null)
			{
				ClassCodeName = studentRow.Class.CodeName;
				ClassFriendlyName = studentRow.Class.ToFriendlyString();
				ClassId = studentRow.ClassId;
				ClassYear = studentRow.Class.Year;
			}
		}

		#endregion
	}
}
