using System;
using System.Windows;
using Timetable.Code;
using Timetable.Windows;

namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for ExpanderControl.xaml
	/// </summary>
	public partial class ExpanderControl : System.Windows.Controls.UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>Controls.ExpanderControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="text">Tekst przycisku <c>button</c>.</param>
		/// <param name="ect"></param>
		/// <param name="window"></param>
		public ExpanderControl(string text, Code.ExpanderControlType ect, MainWindow window)
		{
			InitializeComponent();

			this.button.Content = text;
			this.callingWindow = window;

			switch (ect)
			{
				case Code.ExpanderControlType.Add:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.plus);
					this.button.Click += AddButton_Click;
					break;
				case Code.ExpanderControlType.Change:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.pen);
					this.button.Click += ChangeButton_Click;
					break;
				case Code.ExpanderControlType.Remove:
				default:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.recycleBin);
					this.button.Click += RemoveButton_Click;
					break;
			}
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

		private void AddButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			ManageWindow manageWindow = new ManageWindow(callingWindow, ExpanderControlType.Add);
			manageWindow.Show();
		}

		private void ChangeButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			ManageWindow manageWindow = new ManageWindow(callingWindow, ExpanderControlType.Change);
			manageWindow.Show();
		}

		private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			foreach (string pesel in callingWindow.GetPeselNumbersOfMarkedPeople())
			{
				try
				{
					if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Students)
					{
						Utilities.Database.DeleteStudent(pesel);
						this.callingWindow.RefreshCurrentView();
					}
					if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Teachers)
					{
						Utilities.Database.DeleteTeacher(pesel);
						this.callingWindow.RefreshCurrentView();
					}
				}
				catch (Utilities.EntityDoesNotExistException)
				{
					MessageBox.Show("Person with given PESEL number does not existed.", "Error");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString(), "Error");
				}
			}
		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		#endregion
	}
}
