using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TollFreeCalculator.Models
{
	public class TollTimes
	{
		public Dictionary<string, string> MyMappings { get; set; }
		[Required(ErrorMessage = "Please Select the valid date")]
		  [DataType(DataType.Date)]
		[DisplayFormat(DataFormatString = "{0:dd/M/yy}", ApplyFormatInEditMode = true)]
		public DateTime? StartDate { get; set; }

		[Required(ErrorMessage = "Please Select the toll count")]
		public TollCount? TollCount { get; set; }
        /// <summary>
		/// Used to Maintain the Total Toll Amount
		/// </summary>
		public int SumAmount { get; set; }
		  /// <summary>
		  /// Contains List of Vehicles
		  /// </summary>
		  [Required(ErrorMessage = "Please select the vehcile type")]
		public List<string> VehicleList { get; set; }

	 }
	 /// <summary>
	 /// TollFree Vehicle List
	 /// </summary>
	public enum TollFreeVehicles
	{
		Motorbike = 0,
		Tractor = 1,
		Emergency = 2,
		Diplomat = 3,
		Foreign = 4,
		Military = 5,
	}
	 /// <summary>
	 /// TollCount
	 /// </summary>
	public enum TollCount
	{
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4,
		Five = 5,
		Six = 6,
		Seven = 7,
		Eight = 8,
		Nine = 9,
		Ten = 10,
	 }

	
}
