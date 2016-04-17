using System.Linq;

using Timetable.Models;

namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for PersonControl.xaml</summary>
	public partial class PersonControl : System.Windows.Controls.UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>Controls.PersonControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="person">Obiekt typu <c>Models.Base.Person</c> wypełniający danymi pola tekstowe kontrol.</param>
		public PersonControl(Models.Base.Person person)
		{
			InitializeComponent();

			this.textBlockPesel.Text = person.Pesel.StringRepresentation;
			this.textBlockFirstName.Text = person.FirstName;
			this.textBlockLastName.Text = person.LastName;

			if (person is Teacher)
			{
				//TODO: 'CodeName' klasy, której jest wychowawcą.
				this.textBlockInfo.Text = string.Empty;
			}
			else if (person is Student)
			{
				//TODO: 'ClassId' klasy, do której uczęszcza student.
				this.textBlockInfo.Text = string.Empty;
			}
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
		/// Zwraca numer PESEL osoby w kontrolce.
		/// </summary>
		/// <returns></returns>
		public string GetPesel()
		{
			return this.textBlockPesel.Text;
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
