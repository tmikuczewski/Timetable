using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.DAL.DataSets.MySql;
using Timetable.DAL.DataSets.MySql.TimetableDataSetTableAdapters;
using Timetable.Utilities;

namespace Timetable.Windows.Management
{
	/// <summary>
	///     Interaction logic for ManageDayWindow.xaml
	/// </summary>
	public partial class ManageDayWindow : Window
	{
		#region Constants and Statics

		#endregion


		#region Fields

		private TimetableDataSet _timetableDataSet;
		private DaysTableAdapter _daysTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;

		private int _currentDayId;
		private TimetableDataSet.DaysRow _currentDayRow;

		#endregion


		#region Properties

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>ManageDayWindow</c>.
		/// </summary>
		public ManageDayWindow(MainWindow mainWindow, ActionType actionType)
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
			_daysTableAdapter = new DaysTableAdapter();

			_daysTableAdapter.Fill(_timetableDataSet.Days);
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						_currentDayRow = _timetableDataSet.Days.NewDaysRow();
						break;
					case ActionType.Change:
						_currentDayRow = PrepareDay();
						break;
				}
			}
			catch (EntityDoesNotExistException)
			{
				ShowErrorMessageBox("Day with given ID number does not exist.");
				Close();
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
				Close();
			}
		}

		private TimetableDataSet.DaysRow PrepareDay()
		{
			if (!int.TryParse(_callingWindow.GetIdNumbersOfMarkedDays().FirstOrDefault(), out _currentDayId))
			{
				throw new EntityDoesNotExistException();
			}

			var dayRow = _timetableDataSet.Days.FindById(_currentDayId);

			if (dayRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return dayRow;
		}

		private void FillControls()
		{
			switch (_actionType)
			{
				case ActionType.Change:
					if (_currentDayRow == null)
						return;

					textBoxId.Text = _currentDayRow.Id.ToString();
					textBoxNumber.Text = _currentDayRow.Number.ToString();
					textBoxName.Text = _currentDayRow.Name;
					break;
			}

			buttonOk.IsEnabled = true;
			buttonCancel.IsEnabled = true;
		}

		private void SaveEntity()
		{
			var numberString = textBoxNumber.Text.Trim();
			var name = textBoxName.Text.Trim();

			try
			{
				SaveDay(numberString, name);
			}
			catch (FieldsNotFilledException)
			{
				ShowWarningMessageBox("All fields are required.");
			}
			catch (FormatException)
			{
				ShowWarningMessageBox("Number is invalid.");
			}
			catch (Exception ex)
			{
				ShowErrorMessageBox(ex.ToString());
			}
		}

		private void SaveDay(string numberString, string name)
		{
			if (string.IsNullOrEmpty(numberString)
				|| string.IsNullOrEmpty(name))
			{
				throw new FieldsNotFilledException();
			}

			int number = int.Parse(numberString);
			_currentDayRow.Number = number;
			_currentDayRow.Name = name;

			if (_actionType == ActionType.Add)
			{
				_timetableDataSet.Days.Rows.Add(_currentDayRow);
			}

			_daysTableAdapter.Update(_timetableDataSet.Days);

			_callingWindow.RefreshViews(EntityType.Day);

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
