using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Management
{
	/// <summary>
	///     Interaction logic for ManageSubjectWindow.xaml
	/// </summary>
	public partial class ManageSubjectWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet _timetableDataSet;
		private SubjectsTableAdapter _subjectsTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;

		private int _currentSubjectId;
		private TimetableDataSet.SubjectsRow _currentSubjectRow;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManageClassWindow</c>.
		/// </summary>
		public ManageSubjectWindow(MainWindow mainWindow, ActionType actionType)
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
			_timetableDataSet = new TimetableDataSet();
			_subjectsTableAdapter = new SubjectsTableAdapter();

			_subjectsTableAdapter.Fill(_timetableDataSet.Subjects);
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						_currentSubjectRow = _timetableDataSet.Subjects.NewSubjectsRow();
						break;
					case ActionType.Change:
						_currentSubjectRow = PrepareSubject();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				ShowErrorMessageBox("Subject with given ID number does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
				Close();
			}
		}

		private TimetableDataSet.SubjectsRow PrepareSubject()
		{
			if (!int.TryParse(_callingWindow.GetIdNumbersOfMarkedSubjects().FirstOrDefault(), out _currentSubjectId))
			{
				throw new EntityDoesNotExistException();
			}

			var subjectRowe = _timetableDataSet.Subjects.FindById(_currentSubjectId);

			if (subjectRowe == null)
			{
				throw new EntityDoesNotExistException();
			}

			return subjectRowe;
		}

		private void FillControls()
		{
			switch (_actionType)
			{
				case ActionType.Change:
					if (_currentSubjectRow == null)
						return;

					textBoxId.Text = _currentSubjectRow.Id.ToString();
					textBoxName.Text = _currentSubjectRow.Name;
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
				SaveSubject(name);
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("All fields are required.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveSubject(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new FieldsNotFilledException();
			}

			_currentSubjectRow.Name = name;

			if (_actionType == ActionType.Add)
			{
				_timetableDataSet.Subjects.Rows.Add(_currentSubjectRow);
			}

			_subjectsTableAdapter.Update(_timetableDataSet.Subjects);

			_callingWindow.RefreshViews(EntityType.Subject);

			Close();
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
