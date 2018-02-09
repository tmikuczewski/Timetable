using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.days")]
	public partial class DaysRow
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public DaysRow()
		{
			LessonsPlaces = new HashSet<LessonsPlacesRow>();
		}

		[Column("id")]
		public int Id { get; set; }

		[Column("name")]
		[Required]
		[StringLength(255)]
		public string Name { get; set; }

		[Column("number")]
		public int Number { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LessonsPlacesRow> LessonsPlaces { get; set; }
	}
}
