using System;
using System.Linq;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for ManageClassWindow.xaml
	/// </summary>
	public partial class ManageClassWindow : System.Windows.Window
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
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
			timetableDataSet = new TimetableDataSet();

			classesTableAdapter = new ClassesTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);

			comboBoxTutor.ItemsSource = timetableDataSet.Teachers.DefaultView;
			comboBoxTutor.DisplayMemberPath = "LastName";
			comboBoxTutor.SelectedValuePath = "Pesel";

			if (this.controlType == ExpanderControlType.Add)
			{
				currentClassRow = timetableDataSet.Classes.NewClassesRow();
			}

			if (this.controlType == ExpanderControlType.Change)
			{
				string currentClass = this.callingWindow.GetIdNumbersOfMarkedClasses().FirstOrDefault();

				int.TryParse(currentClass, out this.currentClassId);

				currentClassRow = timetableDataSet.Classes.FindById(this.currentClassId);

				if (currentClassRow != null)
				{
					this.textBoxId.Text = currentClassRow.Id.ToString();
					this.textBoxYear.Text = currentClassRow.Year.ToString();
					this.textBoxCodeName.Text = currentClassRow.CodeName;
					this.comboBoxTutor.SelectedValue = currentClassRow.TutorPesel;
				}
				else
				{
					MessageBox.Show("Class with given ID number does not existed.", "Error");
					this.Close();
				}
			}
		}

		private void buttonOk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			string year = this.textBoxYear.Text.Trim();
			string codeName = this.textBoxCodeName.Text.Trim();

			try
			{
				if (this.comboBoxTutor.SelectedValue == null
					|| string.IsNullOrEmpty(year)
					|| string.IsNullOrEmpty(codeName))
				{
					MessageBox.Show("All fields are required.", "Error");
				}
				else
				{
					int yearNumber = int.Parse(year);

					currentClassRow.Year = yearNumber;
					currentClassRow.CodeName = codeName;

					if (this.comboBoxTutor.SelectedValue != null)
					{
						currentClassRow.TutorPesel = this.comboBoxTutor.SelectedValue.ToString();
					}

					if (this.controlType == ExpanderControlType.Add)
					{
						timetableDataSet.Classes.Rows.Add(currentClassRow);
					}

					classesTableAdapter.Update(timetableDataSet.Classes);

					this.callingWindow.RefreshCurrentView();
					this.Close();
				}
			}
			catch (FormatException)
			{
				MessageBox.Show("Year is invalid.", "Error");
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

		private TimetableDataSet timetableDataSet;

		private ClassesTableAdapter classesTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;

		private TimetableDataSet.ClassesRow currentClassRow;

		#endregion
	}
}
