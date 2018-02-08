using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Model.MySql
{
	[Table("timetable.lessons")]
	public partial class lessons
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public lessons()
		{
			lessons_places = new HashSet<lessons_places>();
		}

		public int id { get; set; }

		[Required]
		[StringLength(11)]
		public string teacher { get; set; }

		public int subject { get; set; }

		[Column("class")]
		public int _class { get; set; }

		public virtual classes classes { get; set; }

		public virtual teachers teachers { get; set; }

		public virtual subjects subjects { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<lessons_places> lessons_places { get; set; }
	}
}
