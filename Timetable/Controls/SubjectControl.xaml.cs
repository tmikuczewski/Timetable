using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Timetable.DAL.DataSet.MySql;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for SubjectControl.xaml
	/// </summary>
	public partial class SubjectControl : UserControl
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
		///     Konstruktor tworzący obiekt typu <c>Controls.SubjectControl</c>.
		/// </summary>
		public SubjectControl()
		{
			InitializeComponent();

			checkBox.Visibility = Visibility.Hidden;
			textBlockId.FontWeight = FontWeights.Bold;
			textBlockName.FontWeight = FontWeights.Bold;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.SubjectControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="subjectRow">Obiekt typu <c>TimetableDataSet.SubjectsRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public SubjectControl(TimetableDataSet.SubjectsRow subjectRow)
		{
			InitializeComponent();

			textBlockId.Text = subjectRow.Id.ToString();
			textBlockName.Text = subjectRow.Name ?? string.Empty;
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
		///     Sprawdza, czy wybrany przedmiot jest zaznaczony.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return checkBox.IsChecked ?? false;
		}

		/// <summary>
		///     Zwraca numer ID przedmiotu w kontrolce.
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
