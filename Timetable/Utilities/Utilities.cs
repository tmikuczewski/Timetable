using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;
using Color = System.Windows.Media.Color;

namespace Timetable.Utilities
{
	/// <summary>
	///     Statyczna klasa przechowująca metody/klasy pomocniczne.
	/// </summary>
	public static class Utilities
	{
		#region Constants and Statics

		#endregion


		#region Fields

		#endregion


		#region Properties

		#endregion


		#region Constructors

		#endregion


		#region Overridden methods

		#endregion


		#region Public methods

		/// <summary>
		///     Statyczna metoda konwertująca obiekt typu <c>System.Drawing.Bitmap</c> na obiekt
		///     typu <c>System.Windows.Media.Imaging.BitmapImage</c>.
		/// </summary>
		/// <param name="bitmap">Obiekt typu <c>System.Drawing.Bitmap</c> mający ulec konwersji.</param>
		/// <returns>Obiekt przekonwertowany na typ <c>System.Windows.Media.Imaging.BitmapImage</c>.</returns>
		public static BitmapImage ToBitmapImage(this Bitmap bitmap)
		{
			var ms = new MemoryStream();
			bitmap.Save(ms, ImageFormat.Png);
			ms.Position = 0;

			var bitmapImg = new BitmapImage();
			bitmapImg.BeginInit();
			bitmapImg.StreamSource = ms;
			bitmapImg.EndInit();

			return bitmapImg;
		}

		/// <summary>
		///     Statyczna metoda konwertująca obiekt typu <c>System.Drawing.Color</c> na obiekt
		///     typu <c>System.Windows.Media.Color</c>.
		/// </summary>
		/// <param name="color">Obiekt typu <c>System.Drawing.Color</c> mający ulec konwersji.</param>
		/// <returns>Obiekt przekonwertowany na typ <c>System.Windows.Media.Color</c>.</returns>
		public static Color ToMediaColor(this System.Drawing.Color color)
		{
			return Color.FromArgb(color.A, color.R, color.G, color.B);
		}

		#endregion


		#region Private methods

		#endregion
	}
}
