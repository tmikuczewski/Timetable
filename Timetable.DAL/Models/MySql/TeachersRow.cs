using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.teachers")]
	public partial class TeachersRow
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public TeachersRow()
		{
			Classes = new HashSet<ClassesRow>();
			Classrooms = new HashSet<ClassroomsRow>();
			Lessons = new HashSet<LessonsRow>();
		}

		[Key]
		[Column("pesel")]
		[StringLength(11)]
		public string Pesel { get; set; }

		[Column("first_name")]
		[Required]
		[StringLength(255)]
		public string FirstName { get; set; }

		[Column("last_name")]
		[Required]
		[StringLength(255)]
		public string LastName { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<ClassesRow> Classes { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<ClassroomsRow> Classrooms { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LessonsRow> Lessons { get; set; }
	}
}
