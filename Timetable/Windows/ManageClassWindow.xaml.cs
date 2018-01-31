using System;
using System.Linq;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows
{
	/// <summary>
	///     Interaction logic for ManageClassWindow.xaml
	/// </summary>
	public partial class ManageClassWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet timetableDataSet;
		private ClassesTableAdapter classesTableAdapter;
		private TeachersTableAdapter teachersTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ExpanderControlType _controlType;

		private int _currentClassId;
		private TimetableDataSet.ClassesRow _currentClassRow;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
		/// </summary>
		public ManageClassWindow(MainWindow mainWindow, ExpanderControlType controlType)
		{
			InitDatabaseObjects();

			InitializeComponent();

			_callingWindow = mainWindow;
			_controlType = controlType;
		}

		#endregion

		#region Events

		private void managementWindow_Loaded(object sender, RoutedEventArgs e)
		{
			FillComboBoxes();

			PrepareEntity();

			FillControls();
		}

		private void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			SaveEntity();
		}

		private void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		private void InitDatabaseObjects()
		{
			timetableDataSet = new TimetableDataSet();
			classesTableAdapter = new ClassesTableAdapter();
			teachersTableAdapter = new TeachersTableAdapter();

			classesTableAdapter.Fill(timetableDataSet.Classes);
			teachersTableAdapter.Fill(timetableDataSet.Teachers);
		}

		private void FillComboBoxes()
		{
			comboBoxTutor.ItemsSource = timetableDataSet.Teachers.OrderBy(t => new Pesel(t.Pesel).BirthDate);
			comboBoxTutor.SelectedValuePath = "Pesel";
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_controlType)
				{
					case ExpanderControlType.Add:
						_currentClassRow = timetableDataSet.Classes.NewClassesRow();
						break;
					case ExpanderControlType.Change:
						_currentClassRow = PrepareClass();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				MessageBox.Show(this, "Class with given ID number does not exist.", "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
				Close();
			}
		}

		private TimetableDataSet.ClassesRow PrepareClass()
		{
			if (!int.TryParse(_callingWindow.GetIdNumbersOfMarkedClasses().FirstOrDefault(), out _currentClassId))
			{
				throw new EntityDoesNotExistException();
			}

			var classRow = timetableDataSet.Classes.FindById(_currentClassId);

			if (classRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return classRow;
		}

		private void FillControls()
		{
			switch (_controlType)
			{
				case ExpanderControlType.Change:
					textBoxId.Text = _currentClassRow.Id.ToString();
					textBoxYear.Text = _currentClassRow.Year.ToString();
					textBoxCodeName.Text = _currentClassRow.CodeName;
					comboBoxTutor.SelectedValue = _currentClassRow.TutorPesel;
					break;
			}
		}

		private void SaveEntity()
		{
			var year = textBoxYear.Text.Trim();
			var codeName = textBoxCodeName.Text.Trim();

			try
			{
				SaveClass(year, codeName);
			}
			catch (FieldsNotFilledException)
			{
				MessageBox.Show(this, "All fields are required.", "Warning",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			catch (FormatException)
			{
				MessageBox.Show(this, "Year is invalid.", "Warning",
					MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex.ToString(), "Error",
					MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		private void SaveClass(string year, string codeName)
		{
			if (comboBoxTutor.SelectedValue == null
				|| string.IsNullOrEmpty(year)
				|| string.IsNullOrEmpty(codeName))
			{
				throw new FieldsNotFilledException();
			}

			_currentClassRow.Year = int.Parse(year);
			_currentClassRow.CodeName = codeName;

			if (comboBoxTutor.SelectedValue != null)
			{
				_currentClassRow.TutorPesel = comboBoxTutor.SelectedValue.ToString();
			}
			else
			{
				throw new FieldsNotFilledException();
			}

			if (_controlType == ExpanderControlType.Add)
			{
				timetableDataSet.Classes.Rows.Add(_currentClassRow);
			}

			classesTableAdapter.Update(timetableDataSet.Classes);

			_callingWindow.RefreshCurrentView(ComboBoxContentType.Classes);

			Close();
		}

		#endregion
	}
}
