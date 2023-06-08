using DoNotCopy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoNotCopy.Core.Entities
{
    public class ExerciseFile
    {
        [Key]
        public Guid Id { get; set; }

        public int ExerciseId { get; set; }

        public int Priority { get; set; }

        public bool SolutionTemplte { get; set; }

        public virtual File File { get; set; }

        public virtual Exercise Exercise { get; set; }
        public string Alt { get; set; }

    }
}
