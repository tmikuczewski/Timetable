using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetMySqlTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Management
{
	/// <summary>
	///     Interaction logic for ManageClassroomWindow.xaml
	/// </summary>
	public partial class ManageClassroomWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSetMySql _timetableDataSet;
		private ClassroomsTableAdapter _classroomsTableAdapter;
		private TeachersTableAdapter _teachersTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;

		private int _currentClassroomId;
		private TimetableDataSetMySql.ClassroomsRow _currentClassroomRow;
		private IList<TimetableDataSetMySql.TeachersRow> _teachersItemsSource;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManageClassroomWindow</c>.
		/// </summary>
		public ManageClassroomWindow(MainWindow mainWindow, ActionType actionType)
		{
			InitializeComponent();

			_callingWindow = mainWindow;
			_actionType = actionType;
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
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(SaveEntity);
			});
		}

		private async void buttonCancel_Click(object sender, RoutedEventArgs e)
		{
			await Task.Factory.StartNew(() =>
			{
				Dispatcher.Invoke(Close);
			});
		}

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		#endregion


		#region Private methods

		private void InitDatabaseObjects()
		{
			_timetableDataSet = new TimetableDataSetMySql();
			_classroomsTableAdapter = new ClassroomsTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();

			_classroomsTableAdapter.Fill(_timetableDataSet.Classrooms);
			_teachersTableAdapter.Fill(_timetableDataSet.Teachers);
		}

		private void FillComboBoxes()
		{
			_teachersItemsSource = _timetableDataSet.Teachers
				.OrderBy(t => t.LastName)
				.ThenBy(t => t.FirstName)
				.ToList();

			var emptyTeacherRow = _timetableDataSet.Teachers.NewTeachersRow();
			emptyTeacherRow["Pesel"] = DBNull.Value;
			_teachersItemsSource.Insert(0, emptyTeacherRow);

			comboBoxAdministrator.ItemsSource = _teachersItemsSource;
			comboBoxAdministrator.SelectedValuePath = "Pesel";
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						_currentClassroomRow = _timetableDataSet.Classrooms.NewClassroomsRow();
						break;
					case ActionType.Change:
						_currentClassroomRow = PrepareClassroom();
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

		private TimetableDataSetMySql.ClassroomsRow PrepareClassroom()
		{
			if (!int.TryParse(_callingWindow.GetIdNumbersOfMarkedClassrooms().FirstOrDefault(), out _currentClassroomId))
			{
				throw new EntityDoesNotExistException();
			}

			var classroomRow = _timetableDataSet.Classrooms.FindById(_currentClassroomId);

			if (classroomRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return classroomRow;
		}

		private void FillControls()
		{
			switch (_actionType)
			{
				case ActionType.Change:
					if (_currentClassroomRow == null)
						return;

					textBoxId.Text = _currentClassroomRow.Id.ToString();
					textBoxName.Text = _currentClassroomRow.Name;
					comboBoxAdministrator.SelectedValue = _currentClassroomRow.AdministratorPesel;
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			var name = textBoxName.Text.Trim();

			try
			{
				SaveClassroom(name);
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("Name is required.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveClassroom(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new FieldsNotFilledException();
			}

			_currentClassroomRow.Name = name;
			_currentClassroomRow["AdministratorPesel"] = comboBoxAdministrator.SelectedValue ?? DBNull.Value;

			if (_actionType == ActionType.Add)
			{
				_timetableDataSet.Classrooms.Rows.Add(_currentClassroomRow);
			}

			//SetOdbcUpdateClassroomCommand(_currentClassroomId, name);

			_classroomsTableAdapter.Update(_timetableDataSet.Classrooms);

			_callingWindow.RefreshViews(EntityType.Classroom);

			Close();
		}

		private void SetOdbcUpdateClassroomCommand(int id, string name)
		{
			//OdbcConnection conn = new OdbcConnection(System.Configuration.ConfigurationManager
			//	.ConnectionStrings["Timetable.Properties.Settings.ConnectionString"].ConnectionString);

			//OdbcCommand cmd = conn.CreateCommand();

			//cmd.CommandText = "UPDATE classrooms " +
			//				  "SET name = ?, administrator = ? " +
			//                  "WHERE id = ?";

			//cmd.Parameters.Add("name", OdbcType.Text).Value = name;
			//cmd.Parameters.Add("administrator", OdbcType.VarChar).Value = comboBoxAdministrator.SelectedValue ?? DBNull.Value;
			//cmd.Parameters.Add("id", OdbcType.Int).Value = id;

			//_classroomsTableAdapter.Adapter.UpdateCommand = cmd;
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
