namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca dzień tygodnia.
	/// </summary>
	public class Day
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt i wypełniający go podanymi danymi.</summary>
		/// <param name="id">Numer identyfikujący dnia tygodnia.</param>
		/// <param name="name">Nazwa dnia tygodnia.</param>
		public Day(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Numer identyfikacyjny dnia tygodnia.</summary>
		public int Id { get; set; }

		/// <summary>
		/// Nazwa dnia tygodnia.</summary>
		public string Name { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
