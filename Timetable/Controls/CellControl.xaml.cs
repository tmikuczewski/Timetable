using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Timetable.Utilities;
using Timetable.Windows;
using SystemColors = System.Drawing.SystemColors;

namespace Timetable.Controls
{
	/// <summary>
	///     Interaction logic for CellControl.xaml
	/// </summary>
	public partial class CellControl : UserControl
	{
		#region Constants and Statics

		private static readonly int PIXELS_PER_LETTER = 7;

		#endregion


		#region Fields

		private readonly string
			OriginalFirstRow,
			OriginalSecondRow,
			OriginalThirdRow;

		private readonly MainWindow _callingWindow;
		private ExpanderControlType _controlType;
		private ComboBoxContentType _contentType;

		private int? _classId;
		private string _teacherPesel;
		private int _dayId;
		private int _hourId;

		#endregion


		#region Properties

		/// <summary>
		///     Pierwszy wiersz kontrolki.
		/// </summary>
		public string FirstRow
		{
			get { return textBlockFirstRow.Text; }
			set { textBlockFirstRow.Text = value; }
		}

		/// <summary>
		///     Drugi wiersz kontrolki.
		/// </summary>
		public string SecondRow
		{
			get { return textBlockSecondRow.Text; }
			set { textBlockSecondRow.Text = value; }
		}

		/// <summary>
		///     Trzeci wiersz kontroli.
		/// </summary>
		public string ThirdRow
		{
			get { return textBlockThirdRow.Text; }
			set { textBlockThirdRow.Text = value; }
		}

		#endregion


		#region Constructors

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>CellControl</c>.
		/// </summary>
		/// <param name="mainWindow">Uchwyt do wywołującego okna.</param>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(MainWindow mainWindow, bool diffColor = false)
		{
			InitializeComponent();

			_callingWindow = mainWindow;

			if (diffColor)
				Background = new SolidColorBrush(SystemColors.InactiveBorder.ToMediaColor());

			FirstRow = OriginalFirstRow = string.Empty;
			SecondRow = OriginalSecondRow = string.Empty;
			ThirdRow = OriginalThirdRow = string.Empty;
		}

		/// <summary>
		///     Konstruktor tworzący obiekt typu <c>CellControl</c> na bazie przesłanych za pomocą parametru danych.
		/// </summary>
		/// <param name="firstRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="secondRow">Tekst wypełniający drugi rząd.</param>
		/// <param name="thirdRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="mainWindow">Uchwyt do wywołującego okna.</param>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(string firstRow, string secondRow, string thirdRow, MainWindow mainWindow, bool diffColor = false)
			: this(mainWindow, diffColor)
		{
			FirstRow = OriginalFirstRow = firstRow;
			SecondRow = OriginalSecondRow = secondRow;
			ThirdRow = OriginalThirdRow = thirdRow;
		}

		#endregion


		#region Events

		private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.NewSize.Width < OriginalFirstRow.Length * PIXELS_PER_LETTER)
			{
				var a = (int) Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				textBlockFirstRow.Text = $"{OriginalFirstRow.Substring(0, a - 4)} ...";
			}
			else
			{
				textBlockFirstRow.Text = OriginalFirstRow;
			}

			if (e.NewSize.Width < OriginalSecondRow.Length * PIXELS_PER_LETTER)
			{
				var a = (int) Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				textBlockSecondRow.Text = $"{OriginalSecondRow.Substring(0, a - 4)} ...";
			}
			else
			{
				textBlockSecondRow.Text = OriginalSecondRow;
			}

			if (e.NewSize.Width < OriginalThirdRow.Length * PIXELS_PER_LETTER)
			{
				var a = (int) Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				textBlockThirdRow.Text = $"{OriginalThirdRow.Substring(0, a - 4)} ...";
			}
			else
			{
				textBlockThirdRow.Text = OriginalThirdRow;
			}
		}

		private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			if (_contentType == ComboBoxContentType.Teachers || _contentType == ComboBoxContentType.Classes)
			{
				var planningWindow = new PlanningWindow(_callingWindow, _controlType, _contentType,
					_classId, _teacherPesel, _dayId, _hourId);
				planningWindow.Owner = _callingWindow;
				planningWindow.Show();
			}
		}

		#endregion
		#region Overridden methods

		#endregion

		#region Public methods

		/// <summary>
		///     Metoda zapisująca dodatkowe informacje o umiejscowieniu na planie lekcji.
		/// </summary>
		/// <param name="controlType">Rodzaj wybranej akcji.</param>
		/// <param name="classId">Identyfikator klasy.</param>
		/// <param name="dayId">Identyfikator dnia.</param>
		/// <param name="hourId">Identyfikator godziny.</param>
		public void SetLessonData(ExpanderControlType controlType, int? classId, int dayId, int hourId)
		{
			_controlType = controlType;
			_contentType = ComboBoxContentType.Classes;
			_classId = classId;
			_dayId = dayId;
			_hourId = hourId;
		}

		/// <summary>
		///     Metoda zapisująca dodatkowe informacje o umiejscowieniu na planie lekcji.
		/// </summary>
		/// <param name="controlType">Rodzaj wybranej akcji.</param>
		/// <param name="teacherPesel">Pesel nauczyciela.</param>
		/// <param name="dayId">Identyfikator dnia.</param>
		/// <param name="hourId">Identyfikator godziny.</param>
		public void SetLessonData(ExpanderControlType controlType, string teacherPesel, int dayId, int hourId)
		{
			_controlType = controlType;
			_contentType = ComboBoxContentType.Teachers;
			_teacherPesel = teacherPesel;
			_dayId = dayId;
			_hourId = hourId;
		}

		#endregion


		#region Private methods

		#endregion
	}
}
