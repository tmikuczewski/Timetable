using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Management
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
		private IList<TimetableDataSet.TeachersRow> _teachersItemsSource;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
		/// </summary>
		public ManageClassWindow(MainWindow mainWindow, ExpanderControlType controlType)
		{
			InitializeComponent();

			_callingWindow = mainWindow;
			_controlType = controlType;
		}

		#endregion

		#region Events

		private async void managementWindow_Loaded(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(() =>
				{
					InitDatabaseObjects();

					FillComboBoxes();

					PrepareEntity();

					FillControls();
				});
			});
		}

		private async void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() => { Dispatcher.Invoke(SaveEntity); });
		}

		private async void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() => { Dispatcher.Invoke(Close); });
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
			_teachersItemsSource = timetableDataSet.Teachers
				.OrderBy(t => t.LastName)
				.ThenBy(t => t.FirstName)
				.ToList();

			var emptyTeacherRow = timetableDataSet.Teachers.NewTeachersRow();
			emptyTeacherRow["Pesel"] = DBNull.Value;
			_teachersItemsSource.Insert(0, emptyTeacherRow);

			comboBoxTutor.ItemsSource = _teachersItemsSource;
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
				ShowErrorMessageBox("Class with given ID number does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
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
					if (_currentClassRow == null)
						return;

					textBoxId.Text = _currentClassRow.Id.ToString();
					textBoxYear.Text = _currentClassRow.Year.ToString();
					textBoxCodeName.Text = _currentClassRow.CodeName;
					comboBoxTutor.SelectedValue = _currentClassRow.TutorPesel;
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			var yearString = textBoxYear.Text.Trim();
			var codeName = textBoxCodeName.Text.Trim();

			try
			{
				SaveClass(yearString, codeName);
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("Year is required.");
			}
			catch (FormatException)
			{
				ShowWarningMessageBox("Year is invalid.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveClass(string yearString, string codeName)
		{
			if (string.IsNullOrEmpty(yearString))
			{
				throw new FieldsNotFilledException();
			}

			int year = int.Parse(yearString);
			_currentClassRow.Year = year;
			_currentClassRow.CodeName = codeName;
			_currentClassRow["TutorPesel"] = comboBoxTutor.SelectedValue ?? DBNull.Value;

			if (_controlType == ExpanderControlType.Add)
			{
				timetableDataSet.Classes.Rows.Add(_currentClassRow);
			}

			SetOdbcUpdateClassCommand(_currentClassId, year, codeName);

			classesTableAdapter.Update(timetableDataSet.Classes);

			_callingWindow.RefreshViews(ComboBoxContentType.Classes);

			Close();
		}

		private void SetOdbcUpdateClassCommand(int id, int year, string codeName)
		{
			OdbcConnection conn = new OdbcConnection(System.Configuration.ConfigurationManager
				.ConnectionStrings["Timetable.Properties.Settings.ConnectionString"].ConnectionString);

			OdbcCommand cmd = conn.CreateCommand();

			cmd.CommandText = "UPDATE classes " +
							  "SET year = ?, code_name = ?, tutor = ? " +
							  "WHERE id = ?";

			cmd.Parameters.Add("year", OdbcType.Int).Value = year;
			cmd.Parameters.Add("code_name", OdbcType.Text).Value = codeName;
			cmd.Parameters.Add("tutor", OdbcType.VarChar).Value = comboBoxTutor.SelectedValue ?? DBNull.Value;
			cmd.Parameters.Add("id", OdbcType.Int).Value = id;

			classesTableAdapter.Adapter.UpdateCommand = cmd;
		}

		private MessageBoxResult ShowErrorMessageBox(string message)
		{
			return MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
		}

		private MessageBoxResult ShowWarningMessageBox(string message)
		{
			return MessageBox.Show(this, message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		#endregion
	}
}
