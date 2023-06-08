using DoNotCopy.Api.Dto;
using DoNotCopy.Core.Data;
using DoNotCopy.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Controllers
{

    [ApiController]
    [Route("api/Course")]
    public class CourseController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public CourseController(IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // הוספה של קורס
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(CourseModel courseModel)
        {
            try
            {
                var course = new Course()
                {
                    Name = courseModel.Name,
                    LecturerId = courseModel.LecturerId,
                };
                await _context.AddAsync(course);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // הצגת כל הקורסים של הסטודנט
        [HttpGet("GetStudentCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetStudentCourse(int studentId)
        {
            var course = _context.StudentWithCourses.Include(x => x.Course).Where(x => x.StudentId == studentId).ToList();

            return Ok(course);
        }


        //// הצגת כל הקורסים של המרצה
        //[HttpGet("GetLecturerCourse")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult GetLecturerCourse(int lecturerId)
        //{
        //    var course = _context.StudentWithCourses.Include(x => x.Course).Where(x => x.Course.LecturerId == lecturerId).ToList();
        //    return Ok(course);
        //}
        // הצגת כל הקורסים של המרצה
        [HttpGet("GetLecturerCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLecturerCourse(int lecturerId)
        {
            var course = _context.Courses.Where(x => x.LecturerId == lecturerId).Select(x => new
            {
                Id = x.Id,
                CourseName = x.Name
            }).ToList();
            return Ok(course);
        }

        // הצגת כל הקורסים של המרצה
        //[HttpGet("AddCourse")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public IActionResult AddCourse(int lecturerId, string CourseName)
        //{
        //    try
        //    {
        //        var cousere = new Course()
        //        {
        //            LecturerId = lecturerId,
        //            Name = CourseName
        //        };
        //        _context.Add(cousere);
        //        _context.SaveChanges();
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest();
        //    }
        //}


        [HttpGet("AddCourse")]
        public void AddCourse()
        {
            var cousere = new Course()
            {
                LecturerId = 1,
                Name = "math",
            };
            _context.Add(cousere);
            _context.SaveChanges();
        }

    }
}
