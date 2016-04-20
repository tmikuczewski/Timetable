﻿namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca lekcję.
	/// </summary>
	public class Lesson
	{
		#region Constructors

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Numer identyfikacyjny lekcji.</summary>
		public int Id { get; set; }

		/// <summary>
		/// Nauczyciel prowadzący przedmiot.</summary>
		public Teacher Teacher { get; set; }

		/// <summary>
		/// Rodzaj przedmiotu.</summary>
		public Subject Subject { get; set; }

		/// <summary>
		/// Klasa, do której przypisany jest przedmiot.</summary>
		public Class Class { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}