using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using Timetable.Utilities;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for PersonControl.xaml
	/// </summary>
	public partial class PersonControl : UserControl
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

		public Pesel Pesel
		{
			get { return new Pesel(textBlockPesel.Text); }
			set { textBlockPesel.Text = value.StringRepresentation; }
		}

		public string FirstName { get; set; }
		public string LastName { get; set; }

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.PersonControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="peselString">Numer PESEL.</param>
		/// <param name="firstName">Imię.</param>
		/// <param name="lastName">Nazwisko.</param>
		/// <param name="info">Informacje dodatkowe.</param>
		public PersonControl(string peselString, string firstName, string lastName, string info)
		{
			InitializeComponent();

			try
			{
				Pesel = new Pesel(peselString);
			}
			catch (InvalidPeselException)
			{
				Pesel = new Pesel("00000000000");
			}

			textBlockFirstName.Text = firstName;
			textBlockLastName.Text = lastName;
			textBlockInfo.Text = info;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.PersonControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="studentRow">Obiekt typu <c>TimetableDataSet.StudentsRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public PersonControl(TimetableDataSet.StudentsRow studentRow)
			: this(studentRow.Pesel,
				  studentRow.FirstName,
				  studentRow.LastName,
				  (studentRow.ClassesRow != null)
					? studentRow.ClassesRow.ToFriendlyString()
					: string.Empty)
		{
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>Controls.PersonControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="teacherRow">Obiekt typu <c>TimetableDataSet.TeachersRow</c> wypełniający danymi pola tekstowe kontrolek.</param>
		public PersonControl(TimetableDataSet.TeachersRow teacherRow)
			: this(teacherRow.Pesel,
				  teacherRow.FirstName,
				  teacherRow.LastName,
				  (teacherRow.GetClassesRows().Length > 0)
					? teacherRow.GetClassesRows().First().ToFriendlyString()
					: string.Empty)
		{
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

		#endregion


		#region Private methods

		#endregion
	}
}
