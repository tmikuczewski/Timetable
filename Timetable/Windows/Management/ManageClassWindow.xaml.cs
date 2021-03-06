﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Timetable.DAL.DataSets.MySql;
using Timetable.DAL.DataSets.MySql.TimetableDataSetTableAdapters;
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

		private TimetableDataSet _timetableDataSet;
		private ClassesTableAdapter _classesTableAdapter;
		private TeachersTableAdapter _teachersTableAdapter;

		private readonly MainWindow _callingWindow;
		private readonly ActionType _actionType;

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
		public ManageClassWindow(MainWindow mainWindow, ActionType actionType)
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
			_timetableDataSet = new TimetableDataSet();
			_classesTableAdapter = new ClassesTableAdapter();
			_teachersTableAdapter = new TeachersTableAdapter();

			_classesTableAdapter.Fill(_timetableDataSet.Classes);
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

			comboBoxTutor.ItemsSource = _teachersItemsSource;
			comboBoxTutor.SelectedValuePath = "Pesel";
		}

		private void PrepareEntity()
		{
			try
			{
				switch (_actionType)
				{
					case ActionType.Add:
						_currentClassRow = _timetableDataSet.Classes.NewClassesRow();
						break;
					case ActionType.Change:
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

			var classRow = _timetableDataSet.Classes.FindById(_currentClassId);

			if (classRow == null)
			{
				throw new EntityDoesNotExistException();
			}

			return classRow;
		}

		private void FillControls()
		{
			switch (_actionType)
			{
				case ActionType.Change:
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

			if (_actionType == ActionType.Add)
			{
				_timetableDataSet.Classes.Rows.Add(_currentClassRow);
			}

			_classesTableAdapter.Update(_timetableDataSet.Classes);

			_callingWindow.RefreshViews(EntityType.Class);

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
