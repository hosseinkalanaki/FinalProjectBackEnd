public class SensorViewModel
{
	#region public string Type { get; set; }
	/// <summary>
	/// Type
	/// </summary>
	public string Type { get; set; }
	#endregion /public string Type { get; set; }

	#region public int ModuleNumber { get; set; }
	/// <summary>
	/// ModuleNumber
	/// </summary>
	public int ModuleNumber { get; set; }
	#endregion /public int ModuleNumber { get; set; }

	#region public string Name { get; set; }
	/// <summary>
	/// Name
	/// </summary>
	public string Name { get; set; }
	#endregion /public string Name { get; set; }

	#region public bool HasAlarm { get; set; }
	/// <summary>
	/// HasAlarm
	/// </summary>
	public bool HasAlarm { get; set; }
	#endregion /public bool HasAlarm { get; set; }

	#region public double Value { get; set; }
	/// <summary>
	/// Value
	/// </summary>
	public double Value { get; set; }
	#endregion /public double Value { get; set; }
}