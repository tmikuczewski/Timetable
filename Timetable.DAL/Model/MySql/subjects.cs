using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Model.MySql
{
	[Table("timetable.subjects")]
	public partial class subjects
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public subjects()
		{
			lessons = new HashSet<lessons>();
		}

		public int id { get; set; }

		[Required]
		[StringLength(255)]
		public string name { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<lessons> lessons { get; set; }
	}
}
