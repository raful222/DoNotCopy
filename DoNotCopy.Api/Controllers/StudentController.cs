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
    [Route("api/Student")]
    public class StudentController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public StudentController(IHttpContextAccessor httpContextAccessor,
            ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        // הוספה של סטודנט
        [HttpPost("Add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Add(StudentModel studentModel)
        {
            try
            {
                var anyIdetityCard = _context.Students.Where(x => x.IdentityCard == studentModel.IdentityCard).FirstOrDefault();
                if (anyIdetityCard == null)
                {

                    var student = new Student()
                    {
                        Name = studentModel.Name,
                        IdentityCard = studentModel.IdentityCard,
                        Email = studentModel.Email,

                    };
                    var studentWithCourse = new StudentWithCourse()
                    {
                        Student = anyIdetityCard,
                        StudentId = anyIdetityCard.Id,
                        CourseId = studentModel.CourseId,
                    };
                    await _context.AddAsync(studentWithCourse);
                    await _context.SaveChangesAsync();
                }
                else {
                    var studentWithCourse = new StudentWithCourse()
                    {
                        Student = anyIdetityCard,
                        StudentId = anyIdetityCard.Id,
                        CourseId = studentModel.CourseId,
                    };
                    await _context.AddAsync(studentWithCourse);
                    await _context.SaveChangesAsync();
                }
               
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task AddStudent()
        {
            var student = new Student()
            {
                Name = "טל",
                IdentityCard = "312312312",
                Email = "a@gmail.com",

            };
            await _context.AddAsync(student);
            await _context.SaveChangesAsync();
        }

        //   הצגה של קורסים לסטודנט
        [HttpGet("StudentCourse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StudentCourse(int studentId)
        {
            try
            {
                var student = _context.StudentWithCourses.Include(x => x.Course).Where(x => x.StudentId == studentId).
                    Select(x => new
                    {
                        CourseName = x.Course.Name,
                        CourseId = x.CourseId
                    }).
                    ToList();
                return Ok(student);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
    }
}
