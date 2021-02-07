using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TollFreeCalculator.Models;

namespace TollFreeCalculator.Controllers
{
	 public class TollFreeCalculatorController : Controller
	 {
		  #region Varible Declartion 
		  private readonly IOptions<TollTimes> tollConfig;
		  Dictionary<int, int> tollFreeDate = new Dictionary<int, int>();
		  private int tollTollFee = 0;
          #endregion

          #region Constructor
          public TollFreeCalculatorController(ILogger<TollFreeCalculatorController> logger,
			IOptions<TollTimes> _tollconfig)
		  {
			   tollConfig = _tollconfig;
		  }
          #endregion

          #region Action methods
          /// <summary>
          /// 
          /// </summary>
          /// <returns></returns>
          public IActionResult Home()
		  {
			   
			   TollTimes tollTimes = new TollTimes();
			   tollTimes.VehicleList = tollConfig.Value.VehicleList;
			   tollTimes.MyMappings = tollConfig.Value.MyMappings;
			   ViewBag.IsShow = false;
			   return View(tollTimes);
		  }

		  [HttpPost]
		  public ActionResult GetTollFee(TollTimes contact, IFormCollection form)
		  {

			   var dateTravelled =Request.Form["StartDate"];
			   var vehcileType = Request.Form["VehicleList"].ToString();
			   var tollCount = Request.Form["TollCount"].ToString();
			   var tollCrossedTimes = Request.Form["MyMappings"].ToString();
			   contact.MyMappings = tollConfig.Value.MyMappings;
			   contact.VehicleList = tollConfig.Value.VehicleList;
			   ViewBag.IsShow = false;
			   if (Request.Form["Calculate"].ToString() != string.Empty)
			   {
					if (ModelState.IsValid)
					{
						 ViewBag.IsShow = true;
						 if (IsTollFreeVehicle(vehcileType))
						 {
							  contact.SumAmount = 0;
							  return View("Home", contact);
						 }
						 if (IsTollFreeDate(Convert.ToDateTime(dateTravelled)))
						 {
							  contact.SumAmount = 0;
							  return View("Home", contact);
						 }
						 contact.SumAmount = GetTotalTollFee(tollCrossedTimes);
					}
			   }
			   return View("Home", contact);
		  }

          #endregion

          #region Private Methods
          /// <summary>
          /// Method Used to get the Total Toll Fee 
          /// </summary>
          /// <returns>Total Toll Fee</returns>
          private int GetTotalTollFee(string crossedTimes)
		  {
			   string[] times = crossedTimes.Split(",");
			   string prevTollCrossed = times[0];
			   foreach (string time in times)
			   {
					int currentFee = GetTollFee(time);
					int prevTollFee = GetTollFee(prevTollCrossed);
					// MultiPassage Rule 
					if (prevTollCrossed == time || time== "06 30 - 06 59")
					{
						 if (tollTollFee > 0) tollTollFee -= prevTollFee;
						 if (currentFee >= prevTollFee) prevTollFee = currentFee;
						 tollTollFee += prevTollFee;
					}
					else
					{
						 tollTollFee += currentFee;
					}
					prevTollCrossed = time;
			   }
			   if (tollTollFee > 60) tollTollFee = 60;
			   return tollTollFee;
		  }

		  /// <summary>
		  /// Method used to get the Toll fee for the particular Toll
		  /// </summary>
		  /// <returns>Toll Fee</returns>
		  private int GetTollFee(string time)
		  {
			   string outValue;
			   int fee = 0;
			   if (tollConfig.Value.MyMappings.TryGetValue(time, out outValue))
			   {
					outValue = outValue.Replace("kr", "");
					fee = Convert.ToInt32(outValue);
			   }
			   return fee;
		  }
		  /// <summary>
		  /// Method Used to Check the date is toll free date or not
		  /// </summary>
		  /// <returns>True or False</returns>
		  private Boolean IsTollFreeDate(DateTime date)
		  {
			   int year = date.Year;
			   int month = date.Month;
			   int day = date.Day;
			   if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
					return true;
			   if (year == 2013)
			   {
					if (tollFreeDate.Contains(new KeyValuePair<int, int>(month, day)))
						 return true;

			   }
			   return false;

		  }
		  /// <summary>
		  /// Method used to check the vehcile is TollFree or Not
		  /// </summary>
		  /// <returns>True or False</returns>
		  private bool IsTollFreeVehicle(string vehicle)
		  {
			   if (Enum.IsDefined(typeof(TollFreeVehicles), vehicle))
					return true;
			   else return false;
		  }

		  /// <summary>
		  /// Method used maintain the tollfree date
		  /// </summary>
		  private void TollFreeDate()
		  {
			   tollFreeDate.Add(1, 1);
			   tollFreeDate.Add(3, 28);
			   tollFreeDate.Add(3, 29);
			   tollFreeDate.Add(4, 1);
			   tollFreeDate.Add(4, 30);
			   tollFreeDate.Add(5, 1);
			   tollFreeDate.Add(5, 8);
			   tollFreeDate.Add(5, 9);
			   tollFreeDate.Add(6, 5);
			   tollFreeDate.Add(6, 6);
			   tollFreeDate.Add(6, 21);
			   tollFreeDate.Add(7, 1);
			   tollFreeDate.Add(7, 11);
			   tollFreeDate.Add(11, 1);
			   tollFreeDate.Add(12, 24);
			   tollFreeDate.Add(12, 25);
			   tollFreeDate.Add(12, 26);
			   tollFreeDate.Add(12, 31);

		  }
		
	 }
     #endregion
}
