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
		public ExpanderControl(string text, ExpanderControlType ect, MainWindow window)
		{
			InitializeComponent();

			this.button.Content = text;
			this.callingWindow = window;

			switch (ect)
			{
				case ExpanderControlType.Add:
					// this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.plus);
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.add);
					this.button.Click += AddButton_Click;
					break;
				case ExpanderControlType.Change:
					// this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.pen);
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.manage);
					this.button.Click += ChangeButton_Click;
					break;
				case ExpanderControlType.Remove:
					// this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.recycleBin);
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.delete);
					this.button.Click += RemoveButton_Click;
					break;
				case ExpanderControlType.XLSX:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.excel);
					this.button.Click += RemoveButton_Click;
					break;
				case ExpanderControlType.PDF:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.pdf);
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
			if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Students
				|| callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Teachers)
			{
				ManageWindow manageWindow = new ManageWindow(callingWindow, ExpanderControlType.Add);
				manageWindow.Show();
			}
			if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Classes)
			{
				ManageClassWindow manageClassWindow = new ManageClassWindow(callingWindow, ExpanderControlType.Add);
				manageClassWindow.Show();
			}
			if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Subjects)
			{
				ManageSubjectWindow manageSubjectWindow = new ManageSubjectWindow(callingWindow, ExpanderControlType.Add);
				manageSubjectWindow.Show();
			}
		}

		private void ChangeButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Students
			    || callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Teachers)
			{
				ManageWindow manageWindow = new ManageWindow(callingWindow, ExpanderControlType.Change);
				manageWindow.Show();
			}
			if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Classes)
			{
				ManageClassWindow manageClassWindow = new ManageClassWindow(callingWindow, ExpanderControlType.Change);
				manageClassWindow.Show();
			}
			if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Subjects)
			{
				ManageSubjectWindow manageSubjectWindow = new ManageSubjectWindow(callingWindow, ExpanderControlType.Change);
				manageSubjectWindow.Show();
			}
		}

		private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Students)
				{
					foreach (string pesel in callingWindow.GetPeselNumbersOfMarkedPeople())
					{
						Utilities.Database.DeleteStudent(pesel);
					}
				}
				if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Teachers)
				{
					foreach (string pesel in callingWindow.GetPeselNumbersOfMarkedPeople())
					{
						Utilities.Database.DeleteTeacher(pesel);
					}
				}
				if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Classes)
				{
					foreach (string id in callingWindow.GetIdNumbersOfMarkedClasses())
					{
						int idNumber = int.Parse(id);
						Utilities.Database.DeleteClass(idNumber);
					}
				}
				if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Subjects)
				{
					foreach (string id in callingWindow.GetIdNumbersOfMarkedSubjects())
					{
						int idNumber = int.Parse(id);
						Utilities.Database.DeleteSubject(idNumber);
					}
				}
			}
			catch (Utilities.EntityDoesNotExistException)
			{
				MessageBox.Show("Entity with given ID number does not existed.", "Error");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString(), "Error");
			}

			this.callingWindow.RefreshCurrentView();
		}

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		#endregion
	}
}
