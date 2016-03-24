namespace Timetable.Models.Base
{
	public abstract class Person
	{
		#region Constructors

		public Person(string pesel, string firstName, string lastName)
		{
			this.PESEL = pesel;
			this.FirstName = firstName;
			this.LastName = lastName;
		}

		#endregion

		#region Overridden methods

		public override string ToString() => $"{this.PESEL} {this.FirstName} {this.LastName}";
		public override int GetHashCode() => this.ToString().GetHashCode();
		public override bool Equals(object obj)
		{
			return (obj is Person
				&& ((obj as Person).PESEL == this.PESEL
					&& (obj as Person).FirstName.ToLower() == this.FirstName.ToLower()
					&& (obj as Person).LastName.ToLower() == this.LastName.ToLower()));
		}

		#endregion

		#region Public methods

		#endregion

		#region Properties

		public string PESEL { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
