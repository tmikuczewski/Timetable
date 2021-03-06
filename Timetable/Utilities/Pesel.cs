﻿using System;
using System.Text.RegularExpressions;

namespace Timetable.Utilities
{
	/// <summary>
	///     Klasa walidującaca i przechowująca informację o jednym poprawnym numerze PESEL.
	/// </summary>
	public class Pesel
	{
		#region Constants and Statics

		/// <summary>
		///     Stała odpowiadająca odpowiedniej ilości znaków w numerze PESEL.
		/// </summary>
		private const int PESEL_VALID_LENGTH = 11;

		/// <summary>
		///     Stała odpowiadająca odpowiedniej masce wyrażenia regularnego dla numeru PESEL.
		/// </summary>
		private const string PESEL_REGEX = @"^\d{11}$";

		#endregion


		#region Fields

		#endregion


		#region Properties

		/// <summary>
		///     Reprezentacja numeru PESEL w postaci <c>System.String</c>.
		/// </summary>
		public string StringRepresentation { get; }

		/// <summary>
		///     Data urodzin osoby posiadającej dany numer PESEL.
		/// </summary>
		public DateTime BirthDate { get; }

		/// <summary>
		///     Płeć osoby posiadającej dany numer PESEL.
		/// </summary>
		public SexType Sex { get; }

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworząy obiekt typu <c>Utilities.Pesel</c> na podstawie parametru.
		/// </summary>
		/// <param name="pesel">Numer PESEL w postaci <c>System.String</c>, na podstawie którego ma zostać utworzony obiekt.</param>
		public Pesel(string pesel)
		{
			if (IsValid(pesel))
			{
				var tempPesel = pesel.Trim();

				StringRepresentation = tempPesel;
				BirthDate = GetBirthDate(StringRepresentation);
				Sex = GetSex(StringRepresentation);
			}
			else
			{
				throw new InvalidPeselException();
			}
		}

		#endregion


		#region Overridden methods

		/// <summary>
		///     Przesłonięta metoda zwracająca reprezentację obiektu w postaci obiektu <c>System.String</c>.
		/// </summary>
		/// <returns>Reprezentację obiektu w postaci obiektu <c>System.String</c>.</returns>
		public override string ToString()
		{
			return StringRepresentation;
		}

		/// <summary>
		///     Przesłonięta metoda zwracająca HashCode obiektu.
		/// </summary>
		/// <returns>Wartość HashCode w postacji obiektu typu <c>System.Int32</c>.</returns>
		public override int GetHashCode()
		{
			return StringRepresentation.GetHashCode();
		}

		/// <summary>
		///     Przesłonięta metoda porównująca czy obiekt jest równy obiektowi podanymy za pomocą parametru.
		/// </summary>
		/// <param name="obj">Obiekt, z którym należy wykonać porównywanie.</param>
		/// <returns>Wartość <c>true</c> lub <c>false</c> w zależności, czy podany obiekt jest równy temu obiektowi, czy nie.</returns>
		public override bool Equals(object obj)
		{
			return obj is Pesel && ((Pesel) obj).ToString() == ToString();
		}

		#endregion


		#region Public methods

		/// <summary>
		///     Metoda sprawdzająca, czy dana wartość zawiera się w danym zakresie.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="leftBoundary"></param>
		/// <param name="rightBoundary"></param>
		/// <returns></returns>
		public static bool IsBetweenAnd(int value, int leftBoundary, int rightBoundary)
		{
			return value >= leftBoundary && value <= rightBoundary;
		}

		/// <summary>
		///     Metoda sprawdzająca czy podany numer PESEL jest poprawny.
		/// </summary>
		/// <param name="pesel">Numer PESEL w postaci <c>System.String</c>, który ma zostać poddany sprawdzeniu.</param>
		/// <returns>Wartość <c>true</c> lub <c>false</c> w zależności, czy podany numer PESEL jest poprawny, czy nie.</returns>
		public static bool IsValid(string pesel)
		{
			var tempPesel = pesel.Trim();

			if (tempPesel.Length == PESEL_VALID_LENGTH
			    && Regex.Match(pesel, PESEL_REGEX).Success
			    && IsCheckDigitValid(tempPesel))
				return true;

			return false;
		}

		/// <summary>
		///     Metoda zwracająca datę urodzenia osoby posiadającej podany numer PESEL.
		/// </summary>
		/// <param name="pesel">Numer PESEL w postaci <c>System.String</c>, na podstawie którego ma zostać określona data urodzin.</param>
		/// <returns>Obiekt typu <c>System.DateTime</c> z ustawioną datą urodzin osoby posiadającej podany numer PESEL.</returns>
		public static DateTime GetBirthDate(string pesel)
		{
			if (IsValid(pesel))
			{
				int year = int.Parse(pesel.Trim().Substring(0, 2)),
					month = int.Parse(pesel.Trim().Substring(2, 2)),
					day = int.Parse(pesel.Trim().Substring(4, 2));

				if (IsBetweenAnd(month, 1, 12)) // ((month >= 1) && (month <= 12))
					return new DateTime(1900 + year, month, day);

				if (IsBetweenAnd(month, 21, 32)) // ((month >= 21) && (month <= 32))
					return new DateTime(2000 + year, month - 20, day);

				if (IsBetweenAnd(month, 41, 52)) // ((month >= 41) && (month <= 52))
					return new DateTime(2100 + year, month - 40, day);

				if (IsBetweenAnd(month, 61, 72)) // ((month >= 61) && (month <= 72))
					return new DateTime(2200 + year, month - 60, day);

				if (IsBetweenAnd(month, 81, 92)) // ((month >= 81) && (month <= 92))
					return new DateTime(1800 + year, month - 80, day);

				throw new InvalidPeselException();
			}

			throw new InvalidPeselException();
		}

		/// <summary>
		///     Metoda zwracająca płeć osoby posiadającej podany numer PESEL.
		/// </summary>
		/// <param name="pesel">Numer PESEL w postaci <c>System.String</c>, na podstawie którego ma zostać określona płeć.</param>
		/// <returns>Wartość wyliczeniową typu <c>Enums.SexType</c> określającą płeć osoby o podanym numerze PESEL.</returns>
		public static SexType GetSex(string pesel)
		{
			if (IsValid(pesel))
				return (SexType) ((int.Parse(pesel.Trim().Substring(9, 1)) % 2 == 0) ? 0 : 1);

			throw new InvalidPeselException();
		}

		#endregion


		#region Private methods

		private static bool IsCheckDigitValid(string pesel)
		{
			var i = 0;
			var digits = new int[11];

			foreach (var digit in pesel.ToCharArray())
				digits[i++] = int.Parse(digit.ToString());

			var sum = 1 * digits[0]
				  + 3 * digits[1]
				  + 7 * digits[2]
				  + 9 * digits[3]
				  + 1 * digits[4]
				  + 3 * digits[5]
				  + 7 * digits[6]
				  + 9 * digits[7]
				  + 1 * digits[8]
				  + 3 * digits[9];

			int sumModulo = sum % 10,
				calculatedCheckDigit = (sumModulo > 0) ? 10 - sumModulo : 0;

			return digits[10] == calculatedCheckDigit;
		}

		#endregion
	}
}
