using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoNotCopy.Api.Dto
{
    public class ExerciseModel
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public int CourseId { get; set; }
        public string FilePath { get; set; }

        public int ExeciseTime { get; set; }

    }
}
