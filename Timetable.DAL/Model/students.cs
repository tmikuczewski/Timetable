namespace Timetable.DAL.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("timetable.students")]
    public partial class students
    {
        [Key]
        [StringLength(11)]
        public string pesel { get; set; }

        [Required]
        [StringLength(255)]
        public string first_name { get; set; }

        [Required]
        [StringLength(255)]
        public string last_name { get; set; }

        [Column("class")]
        public int? _class { get; set; }

        public virtual classes classes { get; set; }
    }
}
