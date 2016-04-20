namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca salę.
	/// </summary>
	public class Classroom
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt i wypełniający go podanymi danymi.</summary>
		/// <param name="id">Numer identyfikujący sali.</param>
		/// <param name="name">Nazwa sali.</param>
		/// <param name="administratorPesel">Numer PESEL opiekuna sali.</param>
		public Classroom(int id, string name, Code.Pesel administratorPesel)
		{
			this.Id = id;
			this.Name = name;
			this.AdministratorPesel = administratorPesel;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Numer identyfikacyjny sali.</summary>
		public int Id { get; set; }

		/// <summary>
		/// Nazwa sali.</summary>
		public string Name { get; set; }

		/// <summary>
		/// Numer PESEL opiekuna sali.</summary>
		public Code.Pesel AdministratorPesel { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
