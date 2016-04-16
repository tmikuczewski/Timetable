using System;
using System.Windows;
using Timetable.Code;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for ManageWindow.xaml</summary>
	public partial class ManageWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>Konstruktor tworzący obiekt typu <c>ManageWindow</c>.
		/// </summary>
		public ManageWindow(MainWindow window)
		{
			this.InitializeComponent();
			this.callingWindow = window;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		#endregion

		#region Private methods

		#endregion

		#region Events

		private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			string peselString = this.maskedTextBoxPesel.Text;
			string firstName = this.textBoxFirstName.Text.Trim();
			string lastName = this.textBoxLastName.Text.Trim();

			try
			{
				Pesel pesel = new Pesel(peselString);

				if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
				{
					MessageBox.Show("All fields are required.", "Error");
				}
				else
				{
					Utilities.Database.AddStudent(pesel.ToString(), firstName, lastName);
					this.callingWindow.RefreshStudents();
					this.Close();
				}
			}
			catch (Utilities.InvalidPeselException)
			{
				MessageBox.Show("PESEL number is invalid.", "Error");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error");
			}
		}

		private void buttonCancel_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		#endregion
	}
}
