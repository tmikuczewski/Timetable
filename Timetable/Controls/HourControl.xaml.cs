using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for HourControl.xaml
	/// </summary>
	public partial class HourControl : UserControl
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
		///     Konstruktor tworzący obiekt typu <c>Controls.HourControl</c>.
		/// </summary>
		public HourControl()
		{
			InitializeComponent();

			checkBox.Visibility = Visibility.Hidden;
			textBlockId.FontWeight = FontWeights.Bold;
			textBlockNumber.FontWeight = FontWeights.Bold;
			textBlockBegin.FontWeight = FontWeights.Bold;
			textBlockEnd.FontWeight = FontWeights.Bold;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.HourControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="hourRow">Obiekt typu <c>TimetableDataSet.HourControl</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public HourControl(TimetableDataSetMySql.HoursRow hourRow)
		{
			InitializeComponent();

			textBlockId.Text = hourRow.Id.ToString();
			textBlockNumber.Text = hourRow.Number.ToString();
			textBlockBegin.Text = hourRow.Begin.ToString(@"hh\:mm");
			textBlockEnd.Text = hourRow.End.ToString(@"hh\:mm");
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
		///     Sprawdza, czy wybrana godzina jest zaznaczona.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return checkBox.IsChecked ?? false;
		}

		/// <summary>
		///     Zwraca numer ID godziny w kontrolce.
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
