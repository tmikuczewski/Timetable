using Timetable.Models.Base;

namespace Timetable.Models
{
	public class Student : Person
	{
		#region Constructors

		public Student(string pesel, string firstName, string lastName)
			: base(pesel, firstName, lastName)
		{
			
		}
		public Student(string pesel, string firstName, string lastName, long classId)
			: this(pesel, firstName, lastName)
		{
			this.Class = classId;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		public long Class { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Enums

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
