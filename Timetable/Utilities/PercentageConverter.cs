using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Timetable.Utilities
{
	/// <summary>
	///     Klasa obliczająca nową wartość na podstawie przekazanych parametrów.
	/// </summary>
	public class PercentageConverter : IValueConverter
	{
		/// <summary>
		///     Metoda obliczająca nową wartość na podstawie mnożnej i mnożnika.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object Convert(object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			try
			{
				return System.Convert.ToDouble(value, CultureInfo.InvariantCulture) *
				       System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
			}
			catch (Exception)
			{
				return Double.NaN;
			}
		}

		/// <summary>
		///     Metoda obliczająca nową wartość na podstawie mnożnej i mnożnika.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value,
			Type targetType,
			object parameter,
			CultureInfo culture)
		{
			return Double.NaN;
		}
	}
}
