using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.Utilities;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class TeacherViewModel
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

		#endregion


		#region Constructors

		public TeacherViewModel()
		{
		}

		public TeacherViewModel(TimetableDataSet.TeachersRow teacherRow)
		{
			Pesel = teacherRow.Pesel;
			FirstName = teacherRow.FirstName;
			FriendlyName = teacherRow.ToFriendlyString();
			LastName = teacherRow.LastName;
		}

		public TeacherViewModel(TeachersRow teacherRow)
		{
			Pesel = teacherRow.Pesel;
			FirstName = teacherRow.FirstName;
			FriendlyName = teacherRow.ToFriendlyString();
			LastName = teacherRow.LastName;
		}

		#endregion
	}
}
