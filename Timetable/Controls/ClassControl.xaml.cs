using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Timetable.DAL.DataSet.MySql;
using Timetable.Utilities;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for ClassControl.xaml
	/// </summary>
	public partial class ClassControl : UserControl
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
		///     Konstruktor tworzący obiekt typu <c>Controls.ClassControl</c>.
		/// </summary>
		public ClassControl()
		{
			InitializeComponent();

			checkBox.Visibility = Visibility.Hidden;
			textBlockId.FontWeight = FontWeights.Bold;
			textBlockYear.FontWeight = FontWeights.Bold;
			textBlockCodeName.FontWeight = FontWeights.Bold;
			textBlockTutor.FontWeight = FontWeights.Bold;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.ClassControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="classRow">Obiekt typu <c>TimetableDataSet.ClassesRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public ClassControl(TimetableDataSet.ClassesRow classRow)
		{
			InitializeComponent();

			textBlockId.Text = classRow.Id.ToString();
			textBlockYear.Text = classRow.Year.ToString();
			textBlockCodeName.Text = classRow.CodeName ?? string.Empty;
			textBlockTutor.Text = classRow.TeachersRow?.ToFriendlyString();
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
		///     Sprawdza, czy wybrana klasa jest zaznaczona.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return checkBox.IsChecked ?? false;
		}

		/// <summary>
		///     Zwraca numer ID klasy w kontrolce.
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
