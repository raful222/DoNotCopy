using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Dto
{
    public class CourseModel
    {
        public string Name { get; set; }
        public int LecturerId { get; set; }
        public int CourseId { get; set; }
    }
}
