using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TollFreeCalculator.Models
{
     /// <summary>
     /// Toll timing class used to set the time
     /// </summary>
     public class TollTimes_Old
     {
          /// <summary>
          /// config property used to read configuration from appsetting.json
          /// </summary>
         // private readonly IConfiguration _config;

          public Dictionary<string, string> Value { get; set; }

     }
}
