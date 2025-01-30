using Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Models.Infrastructure;
public class DatabaseContext : DbContext
{
	private readonly IConfiguration _configuration;

	public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration)
		: base(options)
	{
		_configuration = configuration;
	}

	public virtual DbSet<Center> Centers { get; set; }
	public virtual DbSet<ModuleData> ModuleDatas { get; set; }
	public virtual DbSet<Module> Modules { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			var connectionString = _configuration
				.GetConnectionString(
				"Server=87.107.54.138,1433;User ID=sa;Password=Dpi@FinalProject;Database=DPIDB;MultipleActiveResultSets=true;TrustServerCertificate=True;"
				);
			optionsBuilder.UseSqlServer(connectionString);
		}
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
