namespace Timetable.Utilities
{
	public static class Utilities
	{
		#region Constructors

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		public static System.Windows.Media.Imaging.BitmapImage ConvertBitmapToBitmapImage(System.Drawing.Bitmap bitmap)
		{
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
			ms.Position = 0;

			System.Windows.Media.Imaging.BitmapImage bitmapImg = new System.Windows.Media.Imaging.BitmapImage();
			bitmapImg.BeginInit();
			bitmapImg.StreamSource = ms;
			bitmapImg.EndInit();

			return bitmapImg;
		}

		#endregion

		#region Properties

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
