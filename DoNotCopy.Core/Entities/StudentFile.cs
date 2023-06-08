using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoNotCopy.Core.Entities
{
    public class StudentFile
    {
        [Key]
        public Guid Id { get; set; }
        public int StudentId { get; set; }
        public int ExerciseId { get; set; }
        public virtual File File { get; set; }
        public virtual Exercise Exercise { get; set; }
        public virtual Student Student { get; set; }
        public string Alt { get; set; }
        public decimal Grade { get; set; }
    }
}
