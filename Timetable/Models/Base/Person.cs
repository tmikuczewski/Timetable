namespace Timetable.Models.Base
{
	/// <summary>
	/// Abstrakcyjna klasa bazowa osoby posiadająca podstawowe informacje.</summary>
	public abstract class Person
	{
		#region Constructors

		/// <summary>
		/// Konstruktor wypełniający danymi wszystkie właściwości obiektu.</summary>
		/// <param name="pesel">Numer identyfikujący PESEL.</param>
		/// <param name="firstName">Imię osoby.</param>
		/// <param name="lastName">Nazwisko osoby.</param>
		public Person(string pesel, string firstName, string lastName)
		{
			this.PESEL = pesel;
			this.FirstName = firstName;
			this.LastName = lastName;
		}

		#endregion

		#region Overridden methods

		/// <summary>
		/// Metoda reprezentująca obiekt w postaci przyjaznego w odczycie string'a.</summary>
		/// <returns>Reprezentacja obiektu w postaci obiektu typu <c>string</c>.</returns>
		public override string ToString() => $"{this.PESEL} {this.FirstName} {this.LastName}";
		/// <summary>
		/// Metoda zwracająca Hash obiektu.</summary>
		/// <returns>Reprezentacja obiektu w postaci liczby całkowitej typu <c>int</c>.</returns>
		public override int GetHashCode() => this.ToString().GetHashCode();
		/// <summary>
		/// Metoda porównująca dwa obiekty ze sobą.</summary>
		/// <param name="obj">Obiekt, z którym należy wykonać porównanie.</param>
		/// <returns>Wartość <c>true</c> jeżeli oba obiekty są sobie równe. Wartość <c>false</c> w przeciwnym wypadku.</returns>
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

		/// <summary>
		/// Unikalny numer identyfikujący obywatela polskiego.</summary>
		public string PESEL { get; set; }
		/// <summary>
		/// Imię osoby.</summary>
		public string FirstName { get; set; }
		/// <summary>
		/// Nazwisko osoby.</summary>
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
