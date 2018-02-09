using System.Linq;
using Timetable.DAL.DataSet.MySql;
using Timetable.DAL.Models.MySql;

namespace Timetable.DAL.Utilities
{
	/// <summary>
	///     Klasa wyposażająca obiekty danych w dodatkowe metody.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		///     Metoda zwracająca informacje opisujące ucznia.
		/// </summary>
		/// <param name="studentRow"></param>
		/// <param name="showPesel"></param>
		/// <returns></returns>
		public static string ToFriendlyString(this TimetableDataSet.StudentsRow studentRow, bool showPesel = false)
		{
			return $"{studentRow.FirstName.First()}. {studentRow.LastName}" +
				   $"{((showPesel) ? " (" + studentRow.Pesel + ")" : string.Empty)}";
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
		public static string ToFriendlyString(this TimetableDataSet.TeachersRow teacherRow, bool showPesel = false)
		{
			return $"{teacherRow.FirstName.First()}. {teacherRow.LastName}" +
				   $"{((showPesel) ? " (" + teacherRow.Pesel + ")" : string.Empty)}";
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
		public static string ToFriendlyString(this TimetableDataSet.ClassesRow classRow, bool showCodeName = true)
		{
			return $"{((classRow.Year >= 0) ? classRow.Year.ToString() : string.Empty)}" +
				   $"{((!string.IsNullOrEmpty(classRow.CodeName)) ? " " + classRow.CodeName : string.Empty)}";
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
