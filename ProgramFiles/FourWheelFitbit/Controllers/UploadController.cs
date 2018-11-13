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
    public class UploadController : Controller
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Post(IFormFile file)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", file.FileName);
                var stream = new FileStream(path, FileMode.Create);

                await file.CopyToAsync(stream);
                stream.Close();

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.SetAttributes(path, FileAttributes.Normal);
                    System.IO.File.Delete(path);

                }
                return Ok(new { length = file.Length, name = file.FileName });
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
