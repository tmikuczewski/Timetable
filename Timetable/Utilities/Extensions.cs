using System.Linq;

using static Timetable.TimetableDataSet;

namespace Timetable.Utilities
{
	public static class Extensions
	{
		public static bool IsBetweenAnd(this int value, int leftBoundary, int rightBoundary) => ((value >= leftBoundary) && (value <= rightBoundary));

		public static string ToFriendlyString(this TeachersRow teacher, bool showPesel = false)
		{
			return $"{teacher.FirstName.First()}.{teacher.LastName}{(showPesel ? (" (" + teacher.Pesel + ")") : string.Empty)}";
		}

		public static string ToFriendlyString(this ClassesRow oClass, bool showCodeName = false)
		{
			return (oClass.Year.ToString()) + (string.IsNullOrEmpty(oClass.CodeName) ? string.Empty : $" ({oClass.CodeName})");
		}
	}
}
