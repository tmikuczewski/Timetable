using System.Linq;
using static Timetable.TimetableDataSetMySql;

namespace Timetable.Utilities
{
	/// <summary>
	///     Klasa wyposażająca obiekty danych w dodatkowe metody.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		///     Metoda sprawdzająca, czy dana wartość zawiera się w danym zakresie.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="leftBoundary"></param>
		/// <param name="rightBoundary"></param>
		/// <returns></returns>
		public static bool IsBetweenAnd(this int value, int leftBoundary, int rightBoundary)
		{
			return value >= leftBoundary && value <= rightBoundary;
		}

		/// <summary>
		///     Metoda zwracająca informacje opisujące ucznia.
		/// </summary>
		/// <param name="studentRow"></param>
		/// <param name="showPesel"></param>
		/// <returns></returns>
		public static string ToFriendlyString(this StudentsRow studentRow, bool showPesel = false)
		{
			return $"{studentRow.FirstName.First()}. {studentRow.LastName}" +
			       $"{((showPesel) ? " (" + studentRow.Pesel + ")" : string.Empty)}";
		}

		/// <summary>
		///     Metoda zwracająca informacje opisujące nauczyciela.
		/// </summary>
		/// <param name="teacherRow"></param>
		/// <param name="showPesel"></param>
		/// <returns></returns>
		public static string ToFriendlyString(this TeachersRow teacherRow, bool showPesel = false)
		{
			return $"{teacherRow.FirstName.First()}. {teacherRow.LastName}" +
			       $"{((showPesel) ? " (" + teacherRow.Pesel + ")" : string.Empty)}";
		}

		/// <summary>
		///     Metoda zwracająca informacje opisujące klasę.
		/// </summary>
		/// <param name="classRow"></param>
		/// <param name="showCodeName"></param>
		/// <returns></returns>
		public static string ToFriendlyString(this ClassesRow classRow, bool showCodeName = true)
		{
			return $"{((classRow.Year >= 0) ? classRow.Year.ToString() : string.Empty)}" +
			       $"{((!string.IsNullOrEmpty(classRow.CodeName)) ? " " + classRow.CodeName : string.Empty)}";
		}
	}
}
