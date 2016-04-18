using System;
using System.Linq;
using System.Windows;
using Timetable.Code;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for ManageSubjectWindow.xaml</summary>
	public partial class ManageSubjectWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
		/// </summary>
		public ManageSubjectWindow(MainWindow window, ExpanderControlType type)
		{
			this.InitializeComponent();
			this.callingWindow = window;
			this.controlType = type;
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

		private void managementWindow_Loaded(object sender, RoutedEventArgs e)
		{
			if (this.controlType == ExpanderControlType.Change)
			{
				try
				{
					string id = this.callingWindow.GetIdNumbersOfMarkedSubjects().FirstOrDefault();
					this.currentSubjectId = int.Parse(id);

					Models.Subject subject = Utilities.Database.GetSubjectById(this.currentSubjectId);
					this.textBoxId.Text = subject.Id.ToString();
					this.textBoxName.Text = subject.Name;
				}
				catch (FormatException)
				{
					MessageBox.Show("Subject with given ID number does not existed.", "Error");
					this.Close();
				}
				catch (Utilities.EntityDoesNotExistException)
				{
					MessageBox.Show("Subject with given ID number does not existed.", "Error");
					this.Close();
				}
			}
		}

		private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			string id = this.textBoxId.Text;
			string name = this.textBoxName.Text.Trim();

			try
			{
				if (string.IsNullOrEmpty(name))
				{
					MessageBox.Show("All fields are required.", "Error");
				}
				else
				{
					if (controlType == ExpanderControlType.Add)
					{
						Utilities.Database.AddSubject(name);

					}

					if (controlType == ExpanderControlType.Change)
					{
						int idNumber = int.Parse(id);

						Utilities.Database.EditSubject(idNumber, name);
					}

					this.callingWindow.RefreshCurrentView();
					this.Close();
				}
			}
			catch (FormatException)
			{
				MessageBox.Show("Number is invalid.", "Error");
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

		private readonly ExpanderControlType controlType;

		private int currentSubjectId;

		#endregion
	}
}
