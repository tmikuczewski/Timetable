using System.Windows.Controls;

using Timetable.Utilities.Enums;

namespace Timetable.Controls
{
	/// <summary>
	/// Interaction logic for ExpanderControl.xaml
	/// </summary>
	public partial class ExpanderControl : UserControl
	{
		#region Constructors

		public ExpanderControl(string text, ExpanderControlImgType ect)
		{
			InitializeComponent();

			this.button.Content = text;

			switch (ect)
			{
				case ExpanderControlImgType.ADD_BTN:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.plus);
					break;
				case ExpanderControlImgType.CHANGE_BTN:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.pen);
					break;
				case ExpanderControlImgType.REMOVE_BTN:
				default:
					this.image.Source = Utilities.Utilities.ConvertBitmapToBitmapImage(Properties.Resources.recycleBin);
					break;
			}
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

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
