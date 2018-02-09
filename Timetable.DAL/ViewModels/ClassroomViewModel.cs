using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSets.MySql;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.Utilities;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class ClassroomViewModel
	{
		#region Fields

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string AdministratorFirstName { get; set; }

		[DataMember]
		public string AdministratorFriendlyName { get; set; }

		[DataMember]
		public string AdministratorPesel { get; set; }

		[DataMember]
		public string AdministratorLastName { get; set; }

		#endregion


		#region Constructors

		public ClassroomViewModel()
		{
		}

		public ClassroomViewModel(TimetableDataSet.ClassroomsRow classroomRow)
		{
			Id = classroomRow.Id;
			Name = classroomRow.Name;

			if (classroomRow.TeachersRow != null)
			{
				AdministratorFirstName = classroomRow.TeachersRow.FirstName;
				AdministratorFriendlyName = classroomRow.TeachersRow.ToFriendlyString();
				AdministratorPesel = classroomRow.AdministratorPesel;
				AdministratorLastName = classroomRow.TeachersRow.LastName;
			}
		}

		public ClassroomViewModel(ClassroomsRow classroomRow)
		{
			Id = classroomRow.Id;
			Name = classroomRow.Name;

			if (classroomRow.Administrator != null)
			{
				AdministratorFirstName = classroomRow.Administrator.FirstName;
				AdministratorFriendlyName = classroomRow.Administrator.ToFriendlyString();
				AdministratorPesel = classroomRow.AdministratorPesel;
				AdministratorLastName = classroomRow.Administrator.LastName;
			}
		}

		#endregion
	}
}
