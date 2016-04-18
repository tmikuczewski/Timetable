namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca klasę.
	/// </summary>
	public class Class
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt i wypełniający go podanymi danymi.</summary>
		/// <param name="id">Numer identyfikujący klasy.</param>
		/// <param name="year">Numer roku.</param>
		/// <param name="codeName">Nazwa kodowa klasy.</param>
		/// <param name="tutorPesel">Numer PESEL wychowawcy.</param>
		public Class(int id, int year, string codeName, Code.Pesel tutorPesel)
		{
			this.Id = id;
			this.Year = year;
			this.CodeName = codeName;
			this.TutorPesel = tutorPesel;
		}

		#endregion

		#region Overridden methods

		/// <summary>
		/// Przesłonięcie metody ToString().
		/// </summary>
		public override string ToString() => $"{this.Id} {this.Year} {this.CodeName ?? string.Empty} {this.TutorPesel}";

		/// <summary>
		/// Przesłonięcie metody GetHashCode().
		/// </summary>
		public override int GetHashCode() => (this.ToString().GetHashCode());

		/// <summary>
		/// Przesłonięcie metody Equals().
		/// </summary>
		public override bool Equals(object obj)
		{
			return ((obj is Class) && ((obj as Class).ToString() == this.ToString()));
		}

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Numer identyfikacyjny klasy.</summary>
		public int Id { get; set; }
		/// <summary>
		/// Numer roku.</summary>
		public int Year { get; set; }
		/// <summary>
		/// Nazwa kodowa klasy.</summary>
		public string CodeName { get; set; }
		/// <summary>
		/// Numer PESEL wychowawcy.</summary>
		public Code.Pesel TutorPesel { get; set; }
		/// <summary>
		/// Kolekcja lekcji, na ktróre muszą uczęszczać uczniowe.</summary>
		public System.Collections.Generic.ICollection<Lesson> Lessons { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
