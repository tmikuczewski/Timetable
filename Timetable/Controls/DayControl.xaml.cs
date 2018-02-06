using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Timetable.Utilities;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for DayControl.xaml
	/// </summary>
	public partial class DayControl : UserControl
	{
		#region Constants and Statics

		/// <summary>
		///     Wysokość kontrolki.
		/// </summary>
		public const int HEIGHT = 30;

		#endregion


		#region Fields

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.DayControl</c>.
		/// </summary>
		public DayControl()
		{
			InitializeComponent();

			checkBox.Visibility = Visibility.Hidden;
			textBlockId.FontWeight = FontWeights.Bold;
			textBlockNumber.FontWeight = FontWeights.Bold;
			textBlockName.FontWeight = FontWeights.Bold;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.DayControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="dayRow">Obiekt typu <c>TimetableDataSet.DaysRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public DayControl(TimetableDataSetMySql.DaysRow dayRow)
		{
			InitializeComponent();

			textBlockId.Text = dayRow.Id.ToString();
			textBlockNumber.Text = dayRow.Number.ToString();
			textBlockName.Text = dayRow.Name ?? string.Empty;
		}

		#endregion


		#region Events

		private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{
			checkBox.IsChecked = !checkBox.IsChecked;
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		/// <summary>
		///     Sprawdza, czy wybrany dzień jest zaznaczony.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return checkBox.IsChecked ?? false;
		}

		/// <summary>
		///     Zwraca numer ID dnia w kontrolce.
		/// </summary>
		/// <returns></returns>
		public string GetId()
		{
			return textBlockId.Text;
		}

		#endregion


		#region Private methods

		#endregion
	}
}
