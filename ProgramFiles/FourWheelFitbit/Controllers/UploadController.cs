using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Data;
using FourWheelFitbit.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FourWheelFitbit.Controllers
{
    [Route("api/[controller]")]
    public class UploadController : Controller
    {
        [HttpPost("upload")]
        public IActionResult Post(IFormFile file)
        {
            try
            {
                string inputData;

                using (StreamReader stream = new StreamReader(file.OpenReadStream()))
                {
                    inputData = stream.ReadToEnd();
                }

                DataTable inputDataTable = new WheelchairData(inputData).wheelchairDataTable;

                return Ok(new { length = file.Length, name = file.FileName });
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}
