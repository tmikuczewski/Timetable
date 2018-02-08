using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Model.MySql
{
	[Table("timetable.days")]
	public partial class days
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public days()
		{
			lessons_places = new HashSet<lessons_places>();
		}

		public int id { get; set; }

		[Required]
		[StringLength(255)]
		public string name { get; set; }

		public int number { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<lessons_places> lessons_places { get; set; }
	}
}
