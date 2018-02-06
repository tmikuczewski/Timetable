using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for LessonControl.xaml
	/// </summary>
	public partial class LessonControl : UserControl
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
		///     Konstruktor tworzący obiekt typu <c>Controls.LessonControl</c>.
		/// </summary>
		public LessonControl()
		{
			InitializeComponent();

			checkBox.Visibility = Visibility.Hidden;
			textBlockId.FontWeight = FontWeights.Bold;
			textBlockSubject.FontWeight = FontWeights.Bold;
			textBlockClass.FontWeight = FontWeights.Bold;
			textBlockTeacher.FontWeight = FontWeights.Bold;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.LessonControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="lessonRow">Obiekt typu <c>TimetableDataSet.LessonRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public LessonControl(TimetableDataSetMySql.LessonsRow lessonRow)
		{
			InitializeComponent();

			textBlockId.Text = lessonRow.Id.ToString();
			textBlockSubject.Text = lessonRow.SubjectsRow.Name;
			textBlockClass.Text = lessonRow.ClassesRow.Year + " " + lessonRow.ClassesRow.CodeName;
			textBlockTeacher.Text = lessonRow.TeachersRow.FirstName[0] + ". " + lessonRow.TeachersRow.LastName;
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
		///     Sprawdza, czy wybrana lekcja jest zaznaczona.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return checkBox.IsChecked ?? false;
		}

		/// <summary>
		///     Zwraca numer ID lekcji w kontrolce.
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
