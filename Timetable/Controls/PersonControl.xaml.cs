using System.Linq;
using System.Windows.Controls;

using Timetable.Utilities;

namespace Timetable.Controls
{
    /// <summary>
    /// Interaction logic for PersonControl.xaml</summary>
    public partial class PersonControl : UserControl
    {
        #region Constructors

        /// <summary>
        /// Konstruktor tworzący obiekt typu <c>Controls.PersonControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
        /// <param name="studentRow">Obiekt typu <c>TimetableDataSet.StudentsRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
        public PersonControl(TimetableDataSet.StudentsRow studentRow)
        {
            InitializeComponent();

            this.Pesel = new Pesel(studentRow.Pesel);
            this.textBlockFirstName.Text = studentRow.FirstName;
            this.textBlockLastName.Text = studentRow.LastName;
            this.textBlockInfo.Text = (studentRow.ClassesRow != null)
                ? studentRow.ClassesRow.Year + " " + studentRow.ClassesRow.CodeName
                : string.Empty;
        }

        /// <summary>
        /// Konstruktor tworzący obiekt typu <c>Controls.PersonControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
        /// <param name="teacherRow">Obiekt typu <c>TimetableDataSet.TeachersRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
        public PersonControl(TimetableDataSet.TeachersRow teacherRow)
        {
            InitializeComponent();

            this.Pesel = new Pesel(teacherRow.Pesel);
            this.textBlockFirstName.Text = teacherRow.FirstName;
            this.textBlockLastName.Text = teacherRow.LastName;
            this.textBlockInfo.Text = (teacherRow.GetClassesRows().Length > 0)
                ? teacherRow.GetClassesRows().First().Year + " " + teacherRow.GetClassesRows().First().CodeName
                : string.Empty;
        }

        #endregion

        #region Overridden methods

        #endregion

        #region Public methods

        /// <summary>
        /// Sprawdza, czy wybrana osoba jest zaznaczona.
        /// </summary>
        /// <returns></returns>
        public bool IsChecked() => (this.checkBox.IsChecked ?? false);

        #endregion

        #region Properties


        public Pesel Pesel
        {
            get { return new Pesel(this.textBlockPesel.Text); }
            set { this.textBlockPesel.Text = value.StringRepresentation; }
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }

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
