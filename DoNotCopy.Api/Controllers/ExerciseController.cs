using DoNotCopy.Api.Dto;
using DoNotCopy.Core.Data;
using DoNotCopy.Core.Entities;
using DoNotCopy.Core.Infrastructure;
using DoNotCopy.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Controllers
{

    [ApiController]
    [Route("api/Exercise")]
    public class ExerciseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private IImageUploader _uploader;
        private readonly FileService _fileService;
        private readonly IPathProvider _pathProvider;
        private readonly GeneralSettings _generalSettings;

        public ExerciseController(IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context,
            IImageUploader uploader,
            FileService fileService,
                        IPathProvider pathProvider,
                         IOptions<GeneralSettings> generalSettings
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _uploader = uploader;
            _fileService = fileService;
            _pathProvider = pathProvider;
            _generalSettings = generalSettings.Value;
        }

        // הוספה של תרגיל
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(ExerciseModel exerciseModel)
        {
            try
            {
                var exercise = new Exercise()
                {
                    Number = exerciseModel.Number,
                    CourseId = exerciseModel.CourseId,
                    Subject = exerciseModel.Name,
                    ExeciseTime = exerciseModel.ExeciseTime,
                };
                using (var _contextTransaction = _context.Database.BeginTransaction())
                {
                    var fileCast = await _fileService.CastFilePathToIFormFile(exerciseModel.FilePath);



                    var file = await _fileService.CreateFileAsync(fileCast, FileType.Zip , _generalSettings.TempltePath);

                    await _context.SaveChangesAsync();

                    var itemImage = new ExerciseFile
                    {
                        Id = file.Id,
                        SolutionTemplte = false,
                        Priority = 1,
                        ExerciseId = exercise.CourseId,
                        Alt = "שאלה"

                    };

                    exercise.ImageFiles.Add(itemImage);
                    _context.Add(exercise);

                    await _context.SaveChangesAsync();

                    _contextTransaction.Commit();

                }
                var fileName = exercise.ImageFiles.Where(x => x.Alt == "שאלה").FirstOrDefault().File.Name;
                var sourceFile = _pathProvider.MapPath(_generalSettings.TempltePath, fileName);
                var courseFolder = _pathProvider.MapPath(_generalSettings.TempltePath, exerciseModel.CourseId.ToString());
                if (!Directory.Exists(courseFolder))
                {
                    Directory.CreateDirectory(courseFolder);
                }
                var exerciseFolder = _pathProvider.MapPath(_generalSettings.TempltePath, exerciseModel.CourseId.ToString(), exercise.Id.ToString());
                if (!Directory.Exists(exerciseFolder))
                {
                    Directory.CreateDirectory(exerciseFolder);
                }
                var destFile = _pathProvider.MapPath(_generalSettings.TempltePath, exerciseModel.CourseId.ToString(), exercise.Id.ToString(), fileName);

                System.IO.File.Move(sourceFile, destFile);

                // Confirm the move.
                if (System.IO.File.Exists(destFile))
                {
                    Console.WriteLine("File moved successfully.");
                }
                else
                {
                    Console.WriteLine("File move failed.");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }


        // הוספה של טלפמלט פתרון
        [HttpPost("AddResultTemplate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddResultTemplate(ResultTempalteModel model)
        {
            var exercise = _context.Exercises.Where(x => x.Id == model.ExerciseId).FirstOrDefault();
            if (exercise != null)
            {
                try
                {
                    using (var _contextTransaction = _context.Database.BeginTransaction())
                    {
                        var fileCast = await _fileService.CastFilePathToIFormFile(model.FilePath);

                        var file = await _fileService.CreateFileAsync(fileCast, FileType.Zip,_generalSettings.ResultTempltePath);

                        await _context.SaveChangesAsync();

                        var itemImage = new ExerciseFile
                        {
                            Id = file.Id,
                            SolutionTemplte = true,
                            Priority = 1,
                            ExerciseId = exercise.Id,
                            Alt = "פתרון"
                        };

                        exercise.ImageFiles.Add(itemImage);
                        _context.Add(exercise);

                        await _context.SaveChangesAsync();

                        _contextTransaction.Commit();

                    }
                    var fileName = exercise.ImageFiles.Where(x => x.Alt == "פתרון").FirstOrDefault().File.Name;
                    var sourceFile = _pathProvider.MapPath(_generalSettings.ResultTempltePath, fileName);
                    var courseFolder = _pathProvider.MapPath(_generalSettings.ResultTempltePath, exercise.CourseId.ToString());
                    if (!Directory.Exists(courseFolder))
                    {
                        Directory.CreateDirectory(courseFolder);
                    }
                    var exerciseFolder = _pathProvider.MapPath(_generalSettings.ResultTempltePath, exercise.CourseId.ToString(), exercise.Id.ToString());
                    if (!Directory.Exists(exerciseFolder))
                    {
                        Directory.CreateDirectory(exerciseFolder);
                    }
                    var destFile = _pathProvider.MapPath(_generalSettings.ResultTempltePath, exercise.CourseId.ToString(), exercise.Id.ToString(), fileName);

                    System.IO.File.Move(sourceFile, destFile);

                    // Confirm the move.
                    if (System.IO.File.Exists(destFile))
                    {
                        Console.WriteLine("File moved successfully.");
                    }
                    else
                    {
                        Console.WriteLine("File move failed.");
                    }
                    return Ok();
                }
                catch (Exception ex)
                {
                    return NotFound();
                }

            }
            return NotFound();

        }

    }
}
