using MySql.Data.Entity;

namespace Timetable.DAL.Model
{
	using System;
	using System.Data.Entity;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Linq;

	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public partial class TimetableModel : DbContext
	{
		public TimetableModel()
			: base("name=TimetableModelMySql")
		{
		}

		public virtual DbSet<classes> classes { get; set; }
		public virtual DbSet<classrooms> classrooms { get; set; }
		public virtual DbSet<days> days { get; set; }
		public virtual DbSet<hours> hours { get; set; }
		public virtual DbSet<lessons> lessons { get; set; }
		public virtual DbSet<lessons_places> lessons_places { get; set; }
		public virtual DbSet<students> students { get; set; }
		public virtual DbSet<subjects> subjects { get; set; }
		public virtual DbSet<teachers> teachers { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<classes>()
				.Property(e => e.code_name)
				.IsUnicode(false);

			modelBuilder.Entity<classes>()
				.Property(e => e.tutor)
				.IsUnicode(false);

			modelBuilder.Entity<classes>()
				.HasMany(e => e.lessons)
				.WithRequired(e => e.classes)
				.HasForeignKey(e => e._class)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<classes>()
				.HasMany(e => e.students)
				.WithOptional(e => e.classes)
				.HasForeignKey(e => e._class);

			modelBuilder.Entity<classrooms>()
				.Property(e => e.name)
				.IsUnicode(false);

			modelBuilder.Entity<classrooms>()
				.Property(e => e.administrator)
				.IsUnicode(false);

			modelBuilder.Entity<classrooms>()
				.HasMany(e => e.lessons_places)
				.WithRequired(e => e.classrooms)
				.HasForeignKey(e => e.classroom)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<days>()
				.Property(e => e.name)
				.IsUnicode(false);

			modelBuilder.Entity<days>()
				.HasMany(e => e.lessons_places)
				.WithRequired(e => e.days)
				.HasForeignKey(e => e.day)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<hours>()
				.HasMany(e => e.lessons_places)
				.WithRequired(e => e.hours)
				.HasForeignKey(e => e.hour)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<lessons>()
				.Property(e => e.teacher)
				.IsUnicode(false);

			modelBuilder.Entity<lessons>()
				.HasMany(e => e.lessons_places)
				.WithRequired(e => e.lessons)
				.HasForeignKey(e => e.lesson)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<students>()
				.Property(e => e.pesel)
				.IsUnicode(false);

			modelBuilder.Entity<students>()
				.Property(e => e.first_name)
				.IsUnicode(false);

			modelBuilder.Entity<students>()
				.Property(e => e.last_name)
				.IsUnicode(false);

			modelBuilder.Entity<subjects>()
				.Property(e => e.name)
				.IsUnicode(false);

			modelBuilder.Entity<subjects>()
				.HasMany(e => e.lessons)
				.WithRequired(e => e.subjects)
				.HasForeignKey(e => e.subject)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<teachers>()
				.Property(e => e.pesel)
				.IsUnicode(false);

			modelBuilder.Entity<teachers>()
				.Property(e => e.first_name)
				.IsUnicode(false);

			modelBuilder.Entity<teachers>()
				.Property(e => e.last_name)
				.IsUnicode(false);

			modelBuilder.Entity<teachers>()
				.HasMany(e => e.classes)
				.WithOptional(e => e.teachers)
				.HasForeignKey(e => e.tutor);

			modelBuilder.Entity<teachers>()
				.HasMany(e => e.classrooms)
				.WithOptional(e => e.teachers)
				.HasForeignKey(e => e.administrator);

			modelBuilder.Entity<teachers>()
				.HasMany(e => e.lessons)
				.WithRequired(e => e.teachers)
				.HasForeignKey(e => e.teacher)
				.WillCascadeOnDelete(false);
		}
	}
}
