using System;

namespace Timetable.Utilities
{
	/// <summary>
	///     Klasa dziedzicząca po <c>System.Exception</c>, która reprezentuje błąd występujący przy próbie
	///     zatwierdzenia formularza, w którym nie wypełniono wszystkich wymaganych pól.
	/// </summary>
	public class FieldsNotFilledException : Exception
	{
		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.FieldsNotFilledException</c>.
		/// </summary>
		public FieldsNotFilledException()
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.FieldsNotFilledException</c> ze zdefiniowaną informacją zwrotną.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		public FieldsNotFilledException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.FieldsNotFilledException</c> ze zdefiniowaną informacją zwrotną
		///     oraz obiektem wyjątku, który wywołał wystąpienie tego wyjątku.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		/// <param name="inner">Obiekt wyjątku, który wywołał wystąpienie tego wyjątku.</param>
		public FieldsNotFilledException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	/// <summary>
	///     Klasa dziedzicząca po <c>System.Exception</c>, która reprezentuje błąd występujący przy próbie
	///     utworzenia obiektu typu <c>Code.Pesel</c> na podstawie nieprawidłowego numeru PESEL podanego parametrem.
	/// </summary>
	public class InvalidPeselException : Exception
	{
		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c>.
		/// </summary>
		public InvalidPeselException()
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c> ze zdefiniowaną informacją zwrotną.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		public InvalidPeselException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c> ze zdefiniowaną informacją zwrotną
		///     oraz obiektem wyjątku, który wywołał wystąpienie tego wyjątku.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		/// <param name="inner">Obiekt wyjątku, który wywołał wystąpienie tego wyjątku.</param>
		public InvalidPeselException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	/// <summary>
	///     Klasa dziedzicząca po <c>System.Exception</c>, która reprezentuje błąd występujący przy próbie
	///     dodania wiersza z kluczem głównym, który już istnieje w danej tabeli bazy danych.
	/// </summary>
	public class DuplicateEntityException : Exception
	{
		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c>.
		/// </summary>
		public DuplicateEntityException()
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c> ze zdefiniowaną informacją zwrotną.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		public DuplicateEntityException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c> ze zdefiniowaną informacją zwrotną
		///     oraz obiektem wyjątku, który wywołał wystąpienie tego wyjątku.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		/// <param name="inner">Obiekt wyjątku, który wywołał wystąpienie tego wyjątku.</param>
		public DuplicateEntityException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	/// <summary>
	///     Klasa dziedzicząca po <c>System.Exception</c>, która reprezentuje błąd występujący przy próbie
	///     pobrania wiersza z kluczem głównym, który nie istnieje w danej tabeli bazy danych.
	/// </summary>
	public class EntityDoesNotExistException : Exception
	{
		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c>.
		/// </summary>
		public EntityDoesNotExistException()
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c> ze zdefiniowaną informacją zwrotną.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		public EntityDoesNotExistException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.InvalidPeselException</c> ze zdefiniowaną informacją zwrotną
		///     oraz obiektem wyjątku, który wywołał wystąpienie tego wyjątku.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		/// <param name="inner">Obiekt wyjątku, który wywołał wystąpienie tego wyjątku.</param>
		public EntityDoesNotExistException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}

	/// <summary>
	///     Klasa dziedzicząca po <c>System.Exception</c>, która reprezentuje błąd występujący przy próbie
	///     wyeksportowania danych do pliku XLS.
	/// </summary>
	public class ExcelApplicationException : Exception
	{
		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.ExcelApplicationException</c>.
		/// </summary>
		public ExcelApplicationException()
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.ExcelApplicationException</c> ze zdefiniowaną informacją
		///     zwrotną.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		public ExcelApplicationException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Konstruktor tworzący nowy obiekt typu <c>Utilities.ExcelApplicationException</c> ze zdefiniowaną informacją zwrotną
		///     oraz obiektem wyjątku, który wywołał wystąpienie tego wyjątku.
		/// </summary>
		/// <param name="message">Wiadomość opisująca wystąpiony wyjątek.</param>
		/// <param name="inner">Obiekt wyjątku, który wywołał wystąpienie tego wyjątku.</param>
		public ExcelApplicationException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
