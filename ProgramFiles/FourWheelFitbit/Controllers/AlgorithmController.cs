using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FourWheelFitbit.Controllers
{
    [Route("api/[controller]")]
    public class AlgorithmController : Controller
    {
        // POST api/<controller>
        // This is the api for the Android App to upload the recorded wheelchair data to. Data will be a CSV string in the following format:
        // x-axis, y-axis, z-axis, timestamp;
        [HttpPost]
        public string Post(string wheelchairData)
        {
            Algorithm.Algorithm algo = new Algorithm.Algorithm();
            return algo.AnalyzeWheelchairData(wheelchairData);
        }
    }
}
