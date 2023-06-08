using DoNotCopy.Api.Dto;
using DoNotCopy.Api.Services;
using DoNotCopy.Api.VIewModel;
using DoNotCopy.Core.Data;
using DoNotCopy.Core.Entities;
using DoNotCopy.Core.Infrastructure;
using DoNotCopy.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Controllers
{
    [ApiController]
    [Route("api/Lecturer")]
    public class LecturerController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private readonly EncryptionService _hashService;
        private readonly FileService _fileService;
        private readonly IPathProvider _pathProvider;
        private readonly GeneralSettings _generalSettings;


        private static readonly Object thisLock = new Object();

        public LecturerController(IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context,
            EncryptionService hashService,
            IPathProvider pathProvider,
            FileService fileService,
            IOptions<GeneralSettings> generalSettings)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _hashService = hashService;
            _pathProvider = pathProvider;
            _fileService = fileService;
            _generalSettings = generalSettings.Value;
        }

        // הוספה של מרצה 
        [HttpPost("AddLucturer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddLucturer(LucturerModel lucturerModel)
        {
            try
            {
                lock (thisLock)
                {
                    var anyLucturer = _context.Lecturers.Any(x => x.Email == lucturerModel.Email &&
                x.Name == lucturerModel.Name);
                    if (!anyLucturer)
                    {
                        var passwordHash = /*_hashService.Encrypt(lucturerModel.Password);*/"1234";
                        var lucturer = new Lecturer()
                        {
                            Name = lucturerModel.Name,
                            PasswordHash = passwordHash,
                            Email = lucturerModel.Email,

                        };
                        _context.Add(lucturer);
                        _context.SaveChanges();
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // העלת פתרון 
        // תעודת זהות סטודנט 
        // מספר תרגיל
        // כתובת תרגיל שהועלה 
        public async Task<IActionResult> UploadsSol(UploadsSolViewModel model)
        {
            try
            {
                using (var _contextTransaction = _context.Database.BeginTransaction())
                {
                    var CourseId = _context.Exercises.Where(x => x.Id == model.ExerciseId).FirstOrDefault().CourseId;
                    var student = _context.Students.Where(x => x.IdentityCard == model.IdentityCard).FirstOrDefault();
                    var fileCast = await _fileService.CastFilePathToIFormFile(model.FilePath);
                    var filePath = _pathProvider.MapPath(_generalSettings.ReportPath, CourseId.ToString(), model.ExerciseId.ToString());
                    var file = await _fileService.CreateFileAsync(fileCast, FileType.Document, filePath);
                    await _context.SaveChangesAsync();

                    var itemImage = new StudentFile
                    {
                        Id = file.Id,
                        ExerciseId = model.ExerciseId,
                        StudentId = student.Id,
                        Alt = "פתרון סטודנט"
                    };

                    student.ImageFiles.Add(itemImage);

                    await _context.SaveChangesAsync();

                    _contextTransaction.Commit();
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //הצגת פתרונות לתרגיל 
        public async Task<IActionResult> GetSolUploads(ResultTempalteModel model)
        {
            try
            {
                var AllFile = _context.StudentFiles
                    .Include(x => x.Exercise)
                    .Include(x => x.Student)
                    .Include(x => x.File)
                    .Where(x => x.ExerciseId == model.ExerciseId)
                    .Select(x => new SolPerExercise()
                    {
                        IdentityCard = x.Student.IdentityCard,
                        Grade = x.Grade,
                        FilePath = _pathProvider.MapPath(_generalSettings.ReportPath,
                        x.Exercise.CourseId.ToString(), x.Exercise.Id.ToString(),
                        x.File.Name)
                    });
                return Ok(AllFile);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
