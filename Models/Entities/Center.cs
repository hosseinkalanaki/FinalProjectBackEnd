using Models.Infrastructure;

namespace Models.Entities;

public partial class Center : BaseEntity
{
	#region Contractor
	public Center(
		Guid ownerId,
		string name,
		string IPAddress,
		string port
		) : base(ownerId)
    {
		Name = name;
		this.IPAddress = IPAddress;
		Port=port;
	}
	#endregion /Contractor

	#region Properties

	#region public string Name { get; set; }
	/// <summary>
	/// Name
	/// </summary>
	public string Name { get; set; }
	#endregion /public string Name { get; set; }

	#region public string IPAddress { get; set; }
	/// <summary>
	/// IPAddress
	/// </summary>
	public string IPAddress { get; set; }
	#endregion /public string IPAddress { get; set; }

	#region public string Port { get; set; }
	/// <summary>
	/// Port
	/// </summary>
	public string Port { get; set; }
	#endregion /public string Port { get; set; }

	#endregion /Properties

	#region Relations

	#region public virtual ICollection<Module> Modules { get; set; }
	/// <summary>
	/// Modules
	/// </summary>
	public virtual ICollection<Module> Modules { get; set; }
	#endregion /public virtual ICollection<Module> Modules { get; set; }

	#endregion /Relations
}