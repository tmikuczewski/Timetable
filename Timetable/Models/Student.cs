using Timetable.Models.Base;

namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca ucznia.</summary>
	public class Student : Person
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt i wypełniający go podanymi danymi.</summary>
		/// <param name="pesel">Numer identyfikujący PESEL.</param>
		/// <param name="firstName">Imię osoby.</param>
		/// <param name="lastName">Nazwisko osoby.</param>
		/// <param name="classId">Klasa, do której należy.</param>
		public Student(Code.Pesel pesel, string firstName, string lastName, long? classId = null)
			: base(pesel, firstName, lastName)
		{
			this.ClassId = classId;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Numer klasy, do której należy uczeń.</summary>
		public long? ClassId { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
