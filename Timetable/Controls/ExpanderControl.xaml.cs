using System;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;
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
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.add);
					this.button.Click += AddButton_Click;
					break;
				case ExpanderControlType.Change:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.manage);
					this.button.Click += ChangeButton_Click;
					break;
				case ExpanderControlType.Remove:
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

			timetableDataSet = new TimetableDataSet();

			classesTableAdapter = new ClassesTableAdapter();
			studentsTableAdapter = new StudentsTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			studentsTableAdapter.Fill(timetableDataSet.Students);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
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
				ManagePersonWindow manageWindow = new ManagePersonWindow(callingWindow, ExpanderControlType.Add);
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
				ManagePersonWindow manageWindow = new ManagePersonWindow(callingWindow, ExpanderControlType.Change);
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
					studentsTableAdapter.Fill(timetableDataSet.Students);

					foreach (string pesel in callingWindow.GetPeselNumbersOfMarkedPeople())
					{
						timetableDataSet.Students.FindByPesel(pesel).Delete();
						studentsTableAdapter.Update(timetableDataSet.Students);
					}
				}
				if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Teachers)
				{
					teachersTableAdapter.Fill(timetableDataSet.Teachers);

					foreach (string pesel in callingWindow.GetPeselNumbersOfMarkedPeople())
					{
						timetableDataSet.Teachers.FindByPesel(pesel).Delete();
						teachersTableAdapter.Update(timetableDataSet.Teachers);

					}
				}
				if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Classes)
				{
					classesTableAdapter.Fill(timetableDataSet.Classes);

					foreach (string id in callingWindow.GetIdNumbersOfMarkedClasses())
					{
						int idNumber;
						int.TryParse(id, out idNumber);
						timetableDataSet.Classes.FindById(idNumber).Delete();
						classesTableAdapter.Update(timetableDataSet.Classes);
					}
				}
				if (callingWindow.GetCurrentCoboBoxContent() == ComboBoxContent.Subjects)
				{
					subjectsTableAdapter.Fill(timetableDataSet.Subjects);

					foreach (string id in callingWindow.GetIdNumbersOfMarkedSubjects())
					{
						int idNumber;
						int.TryParse(id, out idNumber);
						timetableDataSet.Subjects.FindById(idNumber).Delete();
						subjectsTableAdapter.Update(timetableDataSet.Subjects);
					}
				}
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

		private TimetableDataSet timetableDataSet;

		private ClassesTableAdapter classesTableAdapter;
		private StudentsTableAdapter studentsTableAdapter;
		private SubjectsTableAdapter subjectsTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;

		#endregion
	}
}
