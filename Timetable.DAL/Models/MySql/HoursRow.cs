using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.hours")]
	public partial class HoursRow
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public HoursRow()
		{
			LessonsPlaces = new HashSet<LessonsPlacesRow>();
		}

		[Column("id")]
		public int Id { get; set; }

		[Column("begin")]
		public TimeSpan Begin { get; set; }

		[Column("end")]
		public TimeSpan End { get; set; }

		[Column("number")]
		public int Number { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<LessonsPlacesRow> LessonsPlaces { get; set; }
	}
}
