namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca przedmiot.
	/// </summary>
	public class Subject
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt i wypełniający go podanymi danymi.</summary>
		/// <param name="id">Numer identyfikujący klasy.</param>
		/// <param name="name">Nazwa przedmiotu.</param>
		public Subject(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		#endregion

		#region Overridden methods

		/// <summary>
		/// Przesłonięcie metody ToString().
		/// </summary>
		public override string ToString() => $"{this.Id} {this.Name ?? string.Empty}";

		/// <summary>
		/// Przesłonięcie metody GetHashCode().
		/// </summary>
		public override int GetHashCode() => (this.ToString().GetHashCode());

		/// <summary>
		/// Przesłonięcie metody Equals().
		/// </summary>
		public override bool Equals(object obj)
		{
			return ((obj is Subject) && ((obj as Subject).ToString() == this.ToString()));
		}

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Numer identyfikacyjny przedmiotu.</summary>
		public int Id { get; set; }
		/// <summary>
		/// Nazwa przedmiotu.</summary>
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
