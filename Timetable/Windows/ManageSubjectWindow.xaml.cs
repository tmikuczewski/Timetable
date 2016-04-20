using System;
using System.Linq;
using System.Windows;
using Timetable.Code;
using Timetable.Models.DataSet;
using Timetable.Models.DataSet.TimetableDataSetTableAdapters;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for ManageSubjectWindow.xaml
	/// </summary>
	public partial class ManageSubjectWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
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
			timetableDataSet = new TimetableDataSet();

			subjectsTableAdapter = new SubjectsTableAdapter();

			subjectsTableAdapter.Fill(timetableDataSet.Subjects);

			if (this.controlType == ExpanderControlType.Add)
			{
				currentSubjectRow = timetableDataSet.Subjects.NewSubjectsRow();
			}

			if (this.controlType == ExpanderControlType.Change)
			{
				string currentSubject = this.callingWindow.GetIdNumbersOfMarkedSubjects().FirstOrDefault();

				int.TryParse(currentSubject, out this.currentSubjectId);

				currentSubjectRow = timetableDataSet.Subjects.FindById(this.currentSubjectId);

				if (currentSubjectRow != null)
				{
					this.textBoxId.Text = currentSubjectRow.Id.ToString();
					this.textBoxName.Text = currentSubjectRow.Name;
				}
				else
				{
					MessageBox.Show("Subject with given ID number does not existed.", "Error");
					this.Close();
				}
			}
		}

		private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			string name = this.textBoxName.Text.Trim();

			try
			{
				if (string.IsNullOrEmpty(name))
				{
					MessageBox.Show("All fields are required.", "Error");
				}
				else
				{
					currentSubjectRow.Name = name;

					if (this.controlType == ExpanderControlType.Add)
					{
						timetableDataSet.Subjects.Rows.Add(currentSubjectRow);
					}

					subjectsTableAdapter.Update(timetableDataSet.Subjects);

					this.callingWindow.RefreshCurrentView();
					this.Close();
				}
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

		private TimetableDataSet timetableDataSet;

		private SubjectsTableAdapter subjectsTableAdapter;

		private TimetableDataSet.SubjectsRow currentSubjectRow;

		#endregion
	}
}
