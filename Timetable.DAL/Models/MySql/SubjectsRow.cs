using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.subjects")]
	public partial class SubjectsRow
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public SubjectsRow()
		{
			Lessons = new HashSet<LessonsRow>();
		}

		[Column("id")]
		public int Id { get; set; }

		[Column("name")]
		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LessonsRow> Lessons { get; set; }
	}
}
