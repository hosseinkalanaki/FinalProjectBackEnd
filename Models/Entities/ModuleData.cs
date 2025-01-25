using Models.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Entities;

public partial class ModuleData : BaseEntity
{
	#region Contractor
	public ModuleData(
		Guid ownerId,
		Guid moduleId,
		int value
		) : base(ownerId)
	{
		Value = value;
		ModuleId = moduleId;
	}
	#endregion /Contractor

	#region Properties

	#region public int Value { get; set; }
	/// <summary>
	/// Value
	/// </summary>
	public int Value { get; set; }
	#endregion /public int Value { get; set; }

	#endregion /Properties

	#region Relations

	#region [ForeignKey("ModuleId")]
	[ForeignKey("ModuleId")]
	public Guid ModuleId { get; set; }
	#endregion /[ForeignKey("ModuleId")]

#nullable disable

	#region public virtual Module Module { get; set; }
	[JsonIgnore]
	public virtual Module Module { get; set; }
	#endregion /public virtual Module Module { get; set; }

#nullable enable

	#endregion /Relations
}