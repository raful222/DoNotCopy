using DoNotCopy.Api.Services;
using DoNotCopy.Core.Data;
using DoNotCopy.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace DoNotCopy.Api.Controllers
{
    [ApiController]
    [Route("api/Questions")]
    public class QuestionsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly ClearFolderService _clearFolderService;
        private readonly GeneralSettings _generalSettings;
        private readonly IPathProvider _pathProvider;

        public QuestionsController(IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context,
            ClearFolderService clearFolderService,
            IPathProvider pathProvider,
                         IOptions<GeneralSettings> generalSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _clearFolderService = clearFolderService;
            _generalSettings = generalSettings.Value;
            _pathProvider = pathProvider;
        }


        // הורדה של תרגיל 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Send(int userId, int exerciseId)
        {
            // test
            //userId = 332412193;
            return await MakeTemplteExercises(userId, exerciseId);


        }
        //311374193  40-100  39
        public int NumberById(int min, int max, int idNumber)
        {
            var number = 0;
            while (max != 1)
            {
                number = number * 10 + idNumber % 10;
                max /= 10;
                idNumber /= 10;
            }
            if (number < min)
            {
                return min;

            }
            return number;
        }

        public int CatIdNumber(int max, int idNumber)
        {
            if (idNumber > 9)
            {
                while (max > 9)
                {
                    idNumber /= 10;
                    max /= 10;
                }
            }
            return idNumber;
        }


        public async Task<IActionResult> CheckSolution(int userId, int exerciseId, string solution)
        {
            try
            {
                var exercises = _context.Exercises
                    .Include(x => x.Course)
                    .Include(x => x.ImageFiles)
                    .ThenInclude(x => x.File)
                    .Where(x => x.Id == exerciseId)
                    .FirstOrDefault();

                var solutionTemplte = exercises.ImageFiles.Where(x => x.SolutionTemplte).FirstOrDefault().File.Name;

                await MakeTemplteExercises(userId, exerciseId);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        //[Route("ss")]
        //public async Task<string> MakeSolReveres()
        //{
        //    string extractPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    string[] files = Directory.GetFiles(extractPath, "script.tpy", SearchOption.AllDirectories);

        //    // public async Task<string> MakeSolReveres(string FileNamepath)
        //    //{
        //    string text = System.IO.File.ReadAllText(files[0]);

        //    var dic = new Dictionary<char, string>();

        //    for (int i = 0; i < text.Length; i++)
        //    {
        //        if (text[i] == '=')
        //        {
        //            var key = text[i - 1];
        //            int number = 0;
        //            while (text[i + 1] != ' ')
        //            {
        //                number = (number * 10) + (text[i + 1] - '0');
        //                i++;
        //            }
        //            dic.Add(key, number.ToString());
        //        }
        //    }
        //    extractPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    files = Directory.GetFiles(extractPath, "sol.tpy", SearchOption.AllDirectories);

        //    text = System.IO.File.ReadAllText(files[0]);

        //    var sol = 0;

        //    using (StreamReader reader = new StreamReader(files[0]))
        //    {
        //        string line;
        //        while ((line = reader.ReadLine()) != null)
        //        {
        //            for (int i = 0; i < line.Length; i++)
        //            {
        //                if (dic.ContainsKey(line[i]))
        //                {
        //                    string value = dic[line[i]];

        //                    char[] chars = value.ToCharArray();

        //                    for (int j = 0; j < chars.Length; j++)
        //                    {
        //                         var  = new string(chars);

        //                    }

        //                }
        //            }

        //        }
        //    }
        //    //    if (dic.ContainsKey(text[i]))
        //    //    {
        //    //        string value = dic[text[i]];
        //    //        int k = 0;
        //    //        while(value[k] == " ")
        //    //        {

        //    //        }
        //    //        text[i] == value;
        //    //    }

        //    return "10";
        //}


        public async Task<FileContentResult> MakeTemplteExercises(int userId, int exerciseId)
        {
            try
            {

                var tempuserId = userId;
                var templte = _context.Exercises.Include(x => x.Course).Include(x => x.ImageFiles).ThenInclude(x => x.File).Where(x => x.Id == exerciseId).FirstOrDefault();
                //end test
                //string ZipedFileFolder = @"C:\Users\Asus\OneDrive\Desktop";

                var file = _pathProvider.MapPath(_generalSettings.TempltePath, templte.CourseId.ToString(), templte.Id.ToString(), templte.ImageFiles.Where(x => !x.SolutionTemplte).FirstOrDefault().File.Name);

                string script_tpy = "script.tpy";
                if (System.IO.Directory.Exists(file))
                {
                    _clearFolderService.ClearFolder(file);
                }
                string extractPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string[] files = Directory.GetFiles(extractPath, script_tpy, SearchOption.AllDirectories);

                foreach (var directory in files)
                {
                    string directoryPaths = Path.GetDirectoryName(directory);
                    //if (System.IO.Directory.Exists(directoryPaths))
                    //{
                    //    _clearFolderService.ClearFolder(directoryPaths);
                    //}
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(file, extractPath);

                //var script = Path.Combine(file, script_tpy);

                string text = System.IO.File.ReadAllText(files[0]);
                var output = "";
                int min = 0;
                int max = 0;
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i] == '$')
                    {
                        int k = 3;
                        if (text[i + 1] == '$' && text[i + 2] == '[')
                        {
                            while (text[i + k] != '-')
                            {
                                min = min * 10;
                                min = min + (text[i + k] - '0');
                                k++;
                            }
                            k++;
                            while (text[i + k] != ']')
                            {
                                max = max * 10;
                                max = max + (text[i + k] - '0');
                                k++;
                            }
                        }
                        var number = NumberById(min, max, tempuserId);
                        if (tempuserId < 10)
                            tempuserId = userId;
                        tempuserId = CatIdNumber(max, tempuserId);
                        max = 0;
                        min = 0;
                        output += number;
                        i = i + k;
                    }
                    else
                    {
                        output += text[i];
                    }
                }
                System.IO.File.WriteAllText(files[0], output);

                var lasw = Path.Combine(extractPath, userId.ToString() + "_" + templte.Course.Name + "_" + templte.Subject + ".zip");
                var fullPath = Path.GetFullPath(lasw);

                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(lasw);
                }
                //if (System.IO.Directory.Exists(lasw))
                //{
                //    _clearFolderService.ClearFolder(lasw);
                //}
                string directoryPath = Path.GetDirectoryName(files[0]);

                int retryCount = 0;
                while (retryCount < 10)
                {
                    try
                    {
                        {
                            System.IO.Compression.ZipFile.CreateFromDirectory(directoryPath, fullPath);

                            // Do something with the file
                            break;
                        }
                    }
                    catch (IOException ex)
                    {
                        // File is still being used, wait and try again
                        Thread.Sleep(1000); // Wait for 1 second
                        retryCount++;
                    }
                }

                var bytes = await System.IO.File.ReadAllBytesAsync(lasw);
                return File(bytes, "text /plain", Path.GetFileName(lasw));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public void checkGpt()
        //{

        //    // ChatGPT Official API
        //    var bot = new ChatGpt("<API_KEY>");

        //    var response = await bot.Ask("What is the weather like today?");
        //}
         

        public async Task<double> MakeResult(int number1 , int number2, string sign)
        {
            double result;

            switch (sign)
            {
                case "+":
                    result = number1 + number2;
                    break;
                case "-":
                    result = number1 - number2;
                    break;
                case "*":
                    result = number1 * number2;
                    break;
                case "/":
                    result = number1 / (double)number2;
                    break;
                case "mod":
                    result = number1 % number2;
                    break;
                case "^":
                    result = Math.Pow(number1,number2);
                    break;
                default:
                    throw new ArgumentException("Invalid sign provided.");
            }

            return result;
        }
    }
}

