using System;
using System.Runtime.Serialization;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.Models.MySql;
using Timetable.DAL.Utilities;

namespace Timetable.DAL.ViewModels
{
	[DataContract]
	public class ClassViewModel
	{
		#region Fields

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public int Year { get; set; }

		[DataMember]
		public string CodeName { get; set; }

		[DataMember]
		public string FriendlyName { get; set; }

		[DataMember]
		public string TutorFirstName { get; set; }

		[DataMember]
		public string TutorFriendlyName { get; set; }

		[DataMember]
		public string TutorPesel { get; set; }

		[DataMember]
		public string TutorLastName { get; set; }

		#endregion


		#region Constructors

		public ClassViewModel()
		{
		}

		public ClassViewModel(TimetableDataSet.ClassesRow classRow)
		{
			Id = classRow.Id;
			Year = classRow.Year;
			CodeName = classRow.CodeName;
			FriendlyName = classRow.ToFriendlyString();

			if (classRow.TeachersRow != null)
			{
				TutorFirstName = classRow.TeachersRow.FirstName;
				TutorFriendlyName = classRow.TeachersRow.ToFriendlyString();
				TutorPesel = classRow.TutorPesel;
				TutorLastName = classRow.TeachersRow.LastName;
			}
		}

		public ClassViewModel(ClassesRow classRow)
		{
			Id = classRow.Id;
			Year = classRow.Year;
			CodeName = classRow.CodeName;
			FriendlyName = classRow.ToFriendlyString();

			if (classRow.Tutor != null)
			{
				TutorFirstName = classRow.Tutor.FirstName;
				TutorFriendlyName = classRow.Tutor.ToFriendlyString();
				TutorPesel = classRow.TutorPesel;
				TutorLastName = classRow.Tutor.LastName;
			}
		}

		#endregion
	}
}
