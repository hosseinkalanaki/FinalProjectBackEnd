public class CenterViewModel
{
	#region public Guid Id { get; set; }
	/// <summary>
	/// Id
	/// </summary>
	public Guid Id { get; set; }
	#endregion /public Guid Id { get; set; }

	#region public string CenterName { get; set; }
	/// <summary>
	/// CenterName
	/// </summary>
	public string CenterName { get; set; }
	#endregion /public string CenterName { get; set; }

	#region public string IpAddress { get; set; }
	/// <summary>
	/// IpAddress
	/// </summary>
	public string IpAddress { get; set; }
	#endregion /public string IpAddress { get; set; }

	#region public string Port { get; set; }
	/// <summary>
	/// Port
	/// </summary>
	public string Port { get; set; }
	#endregion /public string Port { get; set; }

	#region public List<SensorViewModel> Sensors { get; set; }
	/// <summary>
	/// Sensors
	/// </summary>
	public List<SensorViewModel> Sensors { get; set; }
	#endregion /public List<SensorViewModel> Sensors { get; set; }
}