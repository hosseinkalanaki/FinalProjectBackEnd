using Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Models.Infrastructure;

public class DatabaseContext : DbContext
{
	public DatabaseContext()
	 : base()
	{
	}

	public virtual DbSet<Center> Centers { get; set; }
	public virtual DbSet<ModuleData> ModuleDatas { get; set; }
	public virtual DbSet<Module> Modules { get; set; }

	protected override void OnConfiguring
		(DbContextOptionsBuilder optionsBuilder)
	{
		var connectionString =
			"Server=87.107.54.138,1433;User ID=sa;Password=Dpi@FinalProject;Database=DPIDB;MultipleActiveResultSets=true;TrustServerCertificate=True;";

		optionsBuilder.UseSqlServer
			(connectionString: connectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
        base.OnModelCreating(modelBuilder);

		#region Center 1 - * Module
		modelBuilder.Entity<Center>()
			.HasMany(parent => parent.Modules)
			.WithOne(child => child.Center)
			.HasForeignKey(child => child.CenterId);
		#endregion Center 1 - * Module

		#region Module 1 - * ModuleData
		modelBuilder.Entity<Module>()
			.HasMany(parent => parent.ModuleDatas)
			.WithOne(child => child.Module)
			.HasForeignKey(child => child.ModuleId);
		#endregion Module 1 - * ModuleData
	}
}
