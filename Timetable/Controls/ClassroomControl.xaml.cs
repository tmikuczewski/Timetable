using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Timetable.Utilities;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for ClassroomControl.xaml
	/// </summary>
	public partial class ClassroomControl : UserControl
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
		///     Konstruktor tworzący obiekt typu <c>Controls.ClassroomControl</c>.
		/// </summary>
		public ClassroomControl()
		{
			InitializeComponent();

			checkBox.Visibility = Visibility.Hidden;
			textBlockId.FontWeight = FontWeights.Bold;
			textBlockName.FontWeight = FontWeights.Bold;
			textBlockAdministrator.FontWeight = FontWeights.Bold;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.ClassroomControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="classroomRow">Obiekt typu <c>TimetableDataSet.ClassroomsRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public ClassroomControl(TimetableDataSet.ClassroomsRow classroomRow)
		{
			InitializeComponent();

			textBlockId.Text = classroomRow.Id.ToString();
			textBlockName.Text = classroomRow.Name ?? string.Empty;
			textBlockAdministrator.Text = classroomRow.TeachersRow?.ToFriendlyString();
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
		///     Sprawdza, czy wybrana sala jest zaznaczona.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return checkBox.IsChecked ?? false;
		}

		/// <summary>
		///     Zwraca numer ID sali w kontrolce.
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
