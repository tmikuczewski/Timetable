using System.Linq;

namespace Timetable.Controls
{
    /// <summary>
    /// Interaction logic for LessonControl.xaml
    /// </summary>
    public partial class LessonControl : System.Windows.Controls.UserControl
    {
        #region Constructors

        /// <summary>
        /// Konstruktor tworzący obiekt typu <c>Controls.LessonControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
        /// <param name="lessonRow">Obiekt typu <c>TimetableDataSet.LessonRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
        public LessonControl(TimetableDataSet.LessonsRow lessonRow)
        {
            InitializeComponent();

            this.textBlockID.Text = lessonRow.Id.ToString();
            this.textBlockTeacher.Text = lessonRow.TeachersRow.FirstName[0] + ". " + lessonRow.TeachersRow.LastName;
            this.textBlockClass.Text = lessonRow.ClassesRow.Year.ToString() + " " + lessonRow.ClassesRow.CodeName;
            this.textBlockSubject.Text = lessonRow.SubjectsRow.Name;
        }

        #endregion

        #region Overridden methods

        #endregion

        #region Public methods

        /// <summary>
        /// Sprawdza, czy wybrana osoba jest zaznaczona.
        /// </summary>
        /// <returns></returns>
        public bool IsChecked()
        {
            return this.checkBox.IsChecked ?? false;
        }

        /// <summary>
        /// Zwraca numer id klasy w kontrolce.
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            return this.textBlockID.Text;
        }

        #endregion

        #region Properties

        #endregion

        #region Private methods

        #endregion

        #region Events

        private void UserControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.checkBox.IsChecked = !this.checkBox.IsChecked;
        }

        #endregion

        #region Constants and Statics

        /// <summary>
        /// Wysokość kontrolki.</summary>
        public const int HEIGHT = 30;

        #endregion

        #region Fields

        #endregion
    }
}
