using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timetable.DAL.Models.MySql
{
	[Table("timetable.students")]
	public partial class StudentsRow
	{
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

		[Column("class")]
		public int? ClassId { get; set; }

		public virtual ClassesRow Class { get; set; }
	}
}
