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
		///     Konstruktor tworzący obiekt typu <c>Controls.LessonControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="lessonRow">Obiekt typu <c>TimetableDataSet.LessonRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public LessonControl(TimetableDataSet.LessonsRow lessonRow)
		{
			InitializeComponent();

			textBlockID.Text = lessonRow.Id.ToString();
			textBlockTeacher.Text = lessonRow.TeachersRow.FirstName[0] + ". " + lessonRow.TeachersRow.LastName;
			textBlockClass.Text = lessonRow.ClassesRow.Year + " " + lessonRow.ClassesRow.CodeName;
			textBlockSubject.Text = lessonRow.SubjectsRow.Name;
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
		///     Sprawdza, czy wybrana osoba jest zaznaczona.
		/// </summary>
		/// <returns></returns>
		public bool IsChecked()
		{
			return checkBox.IsChecked ?? false;
		}

		/// <summary>
		///     Zwraca numer id klasy w kontrolce.
		/// </summary>
		/// <returns></returns>
		public string GetId()
		{
			return textBlockID.Text;
		}

		#endregion


		#region Private methods

		#endregion
	}
}
