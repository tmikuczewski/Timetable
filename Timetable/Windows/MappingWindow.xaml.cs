using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	/// Interaction logic for MappingWindow.xaml</summary>
	public partial class MappingWindow : System.Windows.Window
	{
		#region Constructors
		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>ManagePersonWindow</c>.
		/// </summary>
		public MappingWindow(MainWindow window, ExpanderControlType type)
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

		void ItemsSourceSort(IEnumerable itemsSource, string sortBy, ListSortDirection direction)
		{
			ICollectionView dataView = CollectionViewSource.GetDefaultView(itemsSource);
			dataView.SortDescriptions.Clear();
			SortDescription sd = new SortDescription(sortBy, direction);
			dataView.SortDescriptions.Add(sd);
			dataView.Refresh();
		}

		#endregion

		#region Events
		private void managementWindow_Loaded(object sender, RoutedEventArgs e)
		{
			timetableDataSet = new TimetableDataSet();

			classesTableAdapter = new ClassesTableAdapter();
			subjectsTableAdapter = new SubjectsTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();
			lessonsTableAdapter = new LessonsTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			subjectsTableAdapter.Fill(timetableDataSet.Subjects);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
			lessonsTableAdapter.Fill(timetableDataSet.Lessons);

			comboBoxTeachers.ItemsSource = timetableDataSet.Teachers.DefaultView;
			comboBoxTeachers.SelectedValuePath = "Pesel";
			comboBoxClasses.ItemsSource = timetableDataSet.Classes.DefaultView;
			ItemsSourceSort(comboBoxClasses.ItemsSource, "Id", ListSortDirection.Ascending);
			comboBoxClasses.SelectedValuePath = "Id";
			comboBoxSubjects.ItemsSource = timetableDataSet.Subjects.DefaultView;
			comboBoxSubjects.SelectedValuePath = "Id";

			if (this.controlType == ExpanderControlType.Add)
			{
				currentLessonRow = timetableDataSet.Lessons.NewLessonsRow();
			}

			if (this.controlType == ExpanderControlType.Change)
			{
				this.currentLessonID = Int32.Parse(this.callingWindow.GetIdNumbersOfMarkedLessons().FirstOrDefault());

				currentLessonRow = timetableDataSet.Lessons.FindById(this.currentLessonID);

				if (currentLessonRow != null)
				{
					this.comboBoxClasses.SelectedIndex = currentLessonRow.ClassId - 1;
					this.comboBoxSubjects.SelectedIndex = currentLessonRow.SubjectId - 1;
					this.comboBoxTeachers.SelectedValue = currentLessonRow.TeacherPesel;
				}
				else
				{
					MessageBox.Show("ID does not exist.", "Error");
					this.Close();
				}
			}
		}

		private void buttonOkMap_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
				if (this.comboBoxClasses.SelectedValue == null ||
					this.comboBoxSubjects.SelectedValue == null ||
					this.comboBoxTeachers.SelectedValue == null)
					MessageBox.Show("All fields are required.", "Error");
				else
				{
					currentLessonRow.TeacherPesel = this.comboBoxTeachers.SelectedValue.ToString();
					currentLessonRow.SubjectId = int.Parse(this.comboBoxSubjects.SelectedValue.ToString());
					currentLessonRow.ClassId = int.Parse(this.comboBoxClasses.SelectedValue.ToString());

					if (this.controlType == ExpanderControlType.Add)
					{
						timetableDataSet.Lessons.Rows.Add(currentLessonRow);
					}
					lessonsTableAdapter.Update(timetableDataSet.Lessons);

					this.callingWindow.RefreshMapping();
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

		private void buttonCancelMap_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			this.Close();
		}
		#endregion

		#region Constants and Statics

		#endregion

		#region Fields
		private readonly MainWindow callingWindow;

		private int currentLessonID;

		private readonly ExpanderControlType controlType;

		private TimetableDataSet timetableDataSet;

		private ClassesTableAdapter classesTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;
		private SubjectsTableAdapter subjectsTableAdapter;
		private LessonsTableAdapter lessonsTableAdapter;

		private TimetableDataSet.LessonsRow currentLessonRow;
		#endregion
	}
}