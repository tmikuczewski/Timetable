using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.lessons")]
	public partial class LessonsRow
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public LessonsRow()
		{
			LessonsPlaces = new HashSet<LessonsPlacesRow>();
		}

		[Column("id")]
		public int Id { get; set; }

		[Column("teacher")]
		[Required]
		[StringLength(11)]
		public string TeacherPesel { get; set; }

		[Column("subject")]
		public int SubjectId { get; set; }

		[Column("class")]
		public int ClassId { get; set; }

		public virtual TeachersRow Teacher { get; set; }

		public virtual SubjectsRow Subject { get; set; }

		public virtual ClassesRow Class { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LessonsPlacesRow> LessonsPlaces { get; set; }
	}
}
