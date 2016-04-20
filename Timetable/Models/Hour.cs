namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca blok godzinowy.
	/// </summary>
	public class Hour
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt i wypełniający go podanymi danymi.</summary>
		/// <param name="id">Numer identyfikujący bloku godzinowego.</param>
		/// <param name="beginHour">Godzina rozpoczęcia bloku godzinowego.</param>
		public Hour(int id, System.TimeSpan beginHour)
		{
			this.Id = id;
			this.BeginHour = beginHour;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Numer identyfikacyjny bloku godzinowego.</summary>
		public int Id { get; set; }

		/// <summary>
		/// Godzina rozpoczęcia bloku godzinowego.</summary>
		public System.TimeSpan BeginHour { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
