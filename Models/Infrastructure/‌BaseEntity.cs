using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models.Infrastructure;

public class BaseEntity : object
{
	#region Constructor
	/// <summary>
	/// Constractor
	/// </summary>
	public BaseEntity(Guid ownerId) : base()
	{
		Id = Guid.NewGuid();
		OwnerId = ownerId;
		DateAdded = DateTime.Now;
		LastUpdatedBy = ownerId;
		LastUpdateDate = DateAdded;
		IsDeleted = false;
	}
	#endregion /Constructor

	#region Properties

	#region public Guid Id { get; set; }
	/// <summary>
	/// Id
	/// </summary>
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public Guid Id { get; set; }
	#endregion /public Guid Id { get; set; }

	#region public Guid OwnerId { get; set; }
	/// <summary>
	/// Creator Id
	/// </summary>
	[JsonIgnore]
	public Guid OwnerId { get; set; }
	#endregion /public Guid OwnerId { get; set; }

	#region public DateTime DateAdded { get; set; }
	/// <summary>
	/// Date Added
	/// </summary>
	[JsonIgnore]
	public DateTime DateAdded { get; set; }
	#endregion /public DateTime DateAdded { get; set; }

	#region public Guid LastUpdatedBy { get; set; }
	/// <summary>
	/// Last Updater User Id
	/// </summary>
	[JsonIgnore]
	public Guid LastUpdatedBy { get; set; }
	#endregion /public Guid LastUpdatedBy { get; set; }

	#region public DateTime LastUpdateDate { get; set; }
	/// <summary>
	/// Last Update datetime
	/// </summary>
	[JsonIgnore]
	public DateTime LastUpdateDate { get; set; }
	#endregion /public DateTime LastUpdateDate { get; set; }

	#region public bool IsDeleted { get; set; }
	/// <summary>
	/// Last Update datetime
	/// </summary>
	[JsonIgnore]
	public bool IsDeleted { get; set; }
	#endregion /public bool IsDeleted { get; set; }


	#endregion /Properties
}
