namespace Timetable.Models
{
	/// <summary>
	/// Klasa reprezentująca komórkę w planie lekcji.
	/// </summary>
	public class LessonPlace
	{
		#region Constructors

		#endregion

		#region Overridden methods

		#endregion

		#region Public methods

		#endregion

		#region Properties

		/// <summary>
		/// Obiekt lekcji określający klasę, przedmiot i nauczyciela.</summary>
		public Lesson Lesson { get; set; }

		/// <summary>
		/// Sala.</summary>
		public Classroom Classroom { get; set; }

		/// <summary>
		/// Dzień tygodnia.</summary>
		public Day Day { get; set; }

		/// <summary>
		/// Blog godzinowy.</summary>
		public Hour Hour { get; set; }

		#endregion

		#region Private methods

		#endregion

		#region Constants and Statics

		#endregion

		#region Fields

		#endregion
	}
}
