using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.classes")]
	public partial class ClassesRow
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public ClassesRow()
		{
			Lessons = new HashSet<LessonsRow>();
			Students = new HashSet<StudentsRow>();
		}

		[Column("id")]
		public int Id { get; set; }

		[Column("year")]
		public int Year { get; set; }

		[Column("code_name")]
		[StringLength(255)]
		public string CodeName { get; set; }

		[Column("tutor")]
		[StringLength(11)]
		public string TutorPesel { get; set; }

		public virtual TeachersRow Tutor { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LessonsRow> Lessons { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<StudentsRow> Students { get; set; }
	}
}
