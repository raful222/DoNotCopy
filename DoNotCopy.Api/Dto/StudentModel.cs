using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Dto
{
    public class StudentModel
    {
        public string Name { get; set; }
        public string IdentityCard { get; set; }
        public string Email { get; set; }
        public int CourseId { get; set; }

    }
}
