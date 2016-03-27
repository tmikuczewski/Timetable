using Timetable.Models.Base;

namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca nauczyciela.</summary>
	public class Teacher : Person
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt i wypełniający go podanymi danymi.</summary>
		/// <param name="pesel">Numer identyfikujący PESEL.</param>
		/// <param name="firstName">Imię osoby.</param>
		/// <param name="lastName">Nazwisko osoby.</param>
		public Teacher(string pesel, string firstName, string lastName)
			: base(pesel, firstName, lastName)
		{

		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Kolekcja numerów lekcji, na których uczy nauczyciel.</summary>
		public System.Collections.Generic.IEnumerable<long> Lessons { get; set; }
		/// <summary>
		/// Kolekcja numerów sal, którymi opiekuje się nauczyciel.</summary>
		public System.Collections.Generic.IEnumerable<long> Classrooms { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
