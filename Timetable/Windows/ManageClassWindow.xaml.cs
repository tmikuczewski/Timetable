using System;
using System.Linq;
using System.Windows;

using Timetable.Code;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for ManageClassWindow.xaml</summary>
	public partial class ManageClassWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
		/// </summary>
		public ManageClassWindow(MainWindow window, ExpanderControlType type)
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
					string id = this.callingWindow.GetIdNumbersOfMarkedClasses().FirstOrDefault();
					this.currentClassId = int.Parse(id);

					Models.Class oClass = Utilities.Database.GetClassById(this.currentClassId);
					this.textBoxId.Text = oClass.Id.ToString();
					this.textBoxYear.Text = oClass.Year.ToString();
					this.textBoxCodeName.Text = oClass.CodeName;
				}
				catch (FormatException)
				{
					MessageBox.Show("Class with given ID number does not existed.", "Error");
					this.Close();
				}
				catch (Utilities.EntityDoesNotExistException)
				{
					MessageBox.Show("Class with given ID number does not existed.", "Error");
					this.Close();
				}
			}
		}

		private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			string id = this.textBoxId.Text;
			string year = this.textBoxYear.Text.Trim();
			string codeName = this.textBoxCodeName.Text.Trim();

			try
			{
				if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(codeName))
				{
					MessageBox.Show("All fields are required.", "Error");
				}
				else
				{
					int yearNumber = int.Parse(year);

					if (controlType == ExpanderControlType.Add)
					{
						Utilities.Database.AddClass(yearNumber, codeName);

					}

					if (controlType == ExpanderControlType.Change)
					{
						int idNumber = int.Parse(id);

						Utilities.Database.EditClass(idNumber, yearNumber, codeName);
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

		private int currentClassId;

		#endregion
	}
}
