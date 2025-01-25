using Models.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities;

public partial class Module : BaseEntity
{
	#region Contractor
	public Module(
		Guid ownerId,
		Guid centerId,
		int minimum,
		int maximum,
		int moduleNumber,
		bool isTemp
		) : base(ownerId)
    {
		CenterId = centerId;
		Minimum = minimum;
		Maximum = maximum;
		CenterId = centerId;
		ModuleNumber = moduleNumber;
		IsTemp = isTemp;

		ModuleDatas = new List<ModuleData>();
	}
	#endregion /Contractor

	#region Properties

	#region public int Minimum { get; set; }
	/// <summary>
	/// Minimum
	/// </summary>
	public int Minimum { get; set; }
	#endregion /public int Minimum { get; set; }

	#region public int Maximum { get; set; }
	/// <summary>
	/// Maximum
	/// </summary>
	public int Maximum { get; set; }
	#endregion /public int Maximum { get; set; }

	#region public int ModuleNumber { get; set; }
	/// <summary>
	/// ModuleNumber
	/// </summary>
	public int ModuleNumber { get; set; }
	#endregion /public int ModuleNumber { get; set; }

	#region public bool IsTemp { get; set; }
	/// <summary>
	/// IsTemp
	/// </summary>
	public bool IsTemp { get; set; }
	#endregion /public bool IsTemp { get; set; }

	#endregion /Properties

	#region Relations

	#region [ForeignKey("CenterId")]
	[ForeignKey("CenterId")]
	public Guid CenterId { get; set; }
	#endregion /[ForeignKey("CenterId")]

#nullable disable

	#region public virtual Center Center { get; set; }
	[JsonIgnore]
	public virtual Center Center { get; set; }
	#endregion /public virtual Center Center { get; set; }

#nullable enable

	#region public virtual ICollection<ModuleData> ModuleDatas { get; set; }
	/// <summary>
	/// Modules
	/// </summary>
	public virtual ICollection<ModuleData> ModuleDatas { get; set; }
	#endregion /public virtual ICollection<ModuleData> ModuleDatas { get; set; }

	#endregion /Relations
}