using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.JSInterop;

namespace MediaStat.Controllers
{

    [DisableRequestSizeLimit]
    public class UploadController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public UploadController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("ServicesPage/single")]
        public IActionResult Single(IFormFile file)
        {
            try
            {
                UploadTweetFile(file);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost("ServicesPage/uploadProfiles")]
        public IActionResult UploadProfiles(IFormFile file)
        {
            try
            {
                UploadProfilesFile(file);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }


        public async Task UploadTweetFile(IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
                if(file.FileName.CompareTo(@"Tweets.csv") != 0)
                {
                    //await JSRuntime.InvokeAsync<bool>("alert", "Tweets.csv");
                }
                //var imagePath = @"\Upload";
                //var uploadPath = _environment.WebRootPath + imagePath;

                string _myConnectionString = "Server=.;Database=MediaStat;Trusted_Connection=True;MultipleActiveResultSets=true;";
                string strQuery = "SELECT [TweetsFullPath] FROM [MediaStat].[dbo].[GeneralConfig]";
                SqlConnection cnn = new SqlConnection(_myConnectionString);
                SqlCommand cmd = new SqlCommand(strQuery, cnn);
                cmd.CommandType = System.Data.CommandType.Text;
                cnn.Open();
                string uploadPath = (string)cmd.ExecuteScalar();
                cnn.Close();

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                else
                {
                    System.IO.DirectoryInfo di = new DirectoryInfo(uploadPath);

                    foreach (FileInfo f in di.GetFiles())
                    {
                        f.Delete();
                    }
                }
                var fullPath = Path.Combine(uploadPath, file.FileName);
                using(FileStream fileStream = new FileStream(fullPath,FileMode.Create,FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }

                if (file.FileName.CompareTo(@"Tweets.csv") != 0)
                {
                    System.IO.File.Move(fullPath, Path.Combine(uploadPath, "Tweets.csv"),true);
                }
            }
        }

        public async Task UploadProfilesFile(IFormFile file)
        {
            if(file != null && file.Length > 0)
            {
   
                string _myConnectionString = "Server=.;Database=MediaStat;Trusted_Connection=True;MultipleActiveResultSets=true;";
                string strQuery = "SELECT [ProfilesFullPath] FROM [MediaStat].[dbo].[GeneralConfig]";
                SqlConnection cnn = new SqlConnection(_myConnectionString);
                SqlCommand cmd = new SqlCommand(strQuery, cnn);
                cmd.CommandType = System.Data.CommandType.Text;
                cnn.Open();
                string uploadPath = (string)cmd.ExecuteScalar();
                cnn.Close();

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }
                var fullPath = Path.Combine(uploadPath, file.FileName);
                using(FileStream fileStream = new FileStream(fullPath,FileMode.Create,FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
        }
    }
}
