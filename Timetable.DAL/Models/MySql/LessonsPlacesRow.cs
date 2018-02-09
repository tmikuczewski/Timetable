using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.lessons_places")]
	public partial class LessonsPlacesRow
	{
		[Key]
		[Column("lesson", Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int LessonId { get; set; }

		[Key]
		[Column("classroom", Order = 1)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ClassroomId { get; set; }

		[Key]
		[Column("day", Order = 2)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DayId { get; set; }

		[Key]
		[Column("hour", Order = 3)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int HourId { get; set; }

		public virtual ClassroomsRow Classroom { get; set; }

		public virtual DaysRow Day { get; set; }

		public virtual HoursRow Hour { get; set; }

		public virtual LessonsRow Lesson { get; set; }
	}
}
