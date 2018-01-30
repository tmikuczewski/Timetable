using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Timetable.Utilities;
using Timetable.Windows;
using SystemColors = System.Drawing.SystemColors;

namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for CellControl.xaml
	/// </summary>
	public partial class CellControl : UserControl
	{
		#region Constructors

		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>CellControl</c>.</summary>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(MainWindow window, bool diffColor = false)
		{
			InitializeComponent();
			this.callingWindow = window;

			if (diffColor)
			{
				this.Background = new SolidColorBrush(SystemColors.InactiveBorder.ToMediaColor());
			}

			this.FirstRow = this.OriginalFirstRow = string.Empty;
			this.SecondRow = this.OriginalSecondRow = string.Empty;
			this.ThirdRow = this.OriginalThirdRow = string.Empty;
		}
		/// <summary>
		/// Konstruktor tworzący obiekt typu <c>CellControl</c> na bazie przesłanych za pomocą parametru danych.</summary>
		/// <param name="firstRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="secondRow">Tekst wypełniający drugi rząd.</param>
		/// <param name="thirdRow">Tekst wypełniający pierwszy rząd.</param>
		/// <param name="diffColor">Parametr sterujący zmianą koloru kontrolki.</param>
		public CellControl(string firstRow, string secondRow, string thirdRow, MainWindow window, bool diffColor = false)
			: this(window, diffColor)
		{
			this.FirstRow = this.OriginalFirstRow = firstRow;
			this.SecondRow = this.OriginalSecondRow = secondRow;
			this.ThirdRow = this.OriginalThirdRow = thirdRow;
		}

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		public void SetLessonData(ExpanderControlType controlType, ComboBoxContent contentType, string teacherPesel, int? classId, int dayId, int hourId)
		{
			this.controlType = controlType;
			this.contentType = contentType;
			this.teacherPesel = teacherPesel;
			this.classId = classId;
			this.dayId = dayId;
			this.hourId = hourId;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Pierwszy wiersz kontrolki.</summary>
		public string FirstRow { get { return this.textBlockFirstRow.Text; } set { this.textBlockFirstRow.Text = value; } }
		/// <summary>
		/// Drugi wiersz kontrolki.</summary>
		public string SecondRow { get { return this.textBlockSecondRow.Text; } set { this.textBlockSecondRow.Text = value; } }
		/// <summary>
		/// Trzeci wiersz kontroli.</summary>
		public string ThirdRow { get { return this.textBlockThirdRow.Text; } set { this.textBlockThirdRow.Text = value; } }

		#endregion

		#region Private methods

		#endregion

		#region Events

		private void UserControl_SizeChanged(object sender, System.Windows.SizeChangedEventArgs e)
		{

			if (e.NewSize.Width < (this.OriginalFirstRow.Length * PIXELS_PER_LETTER))
			{
				var a = (int)Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				this.textBlockFirstRow.Text = $"{this.OriginalFirstRow.Substring(0, a - 4)} ...";
			}
			else
			{
				this.textBlockFirstRow.Text = this.OriginalFirstRow;
			}

			if (e.NewSize.Width < (this.OriginalSecondRow.Length * PIXELS_PER_LETTER))
			{
				var a = (int)Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				this.textBlockSecondRow.Text = $"{this.OriginalSecondRow.Substring(0, a - 4)} ...";
			}
			else
			{
				this.textBlockSecondRow.Text = this.OriginalSecondRow;
			}

			if (e.NewSize.Width < (this.OriginalThirdRow.Length * PIXELS_PER_LETTER))
			{
				var a = (int)Math.Floor(e.NewSize.Width / PIXELS_PER_LETTER);
				this.textBlockThirdRow.Text = $"{this.OriginalThirdRow.Substring(0, a - 4)} ...";
			}
			else
			{
				this.textBlockThirdRow.Text = this.OriginalThirdRow;
			}
		}

		private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			if (contentType == ComboBoxContent.Teachers || contentType == ComboBoxContent.Classes)
			{
				PlanningWindow planningWindow = new PlanningWindow(callingWindow, controlType,
					contentType, teacherPesel, classId, dayId, hourId);
				planningWindow.Owner = callingWindow;
				planningWindow.Show();
			}
		}

		#endregion

		#region Constants and Statics

		private static readonly int PIXELS_PER_LETTER = 7;

		#endregion

		#region Fields

		private readonly MainWindow callingWindow;

		private string
			OriginalFirstRow,
			OriginalSecondRow,
			OriginalThirdRow;

		private ExpanderControlType controlType;

		private ComboBoxContent contentType;

		private string teacherPesel;

		private int? classId;

		private int
			dayId,
			hourId;

		#endregion
	}
}
