using DoNotCopy.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoNotCopy.Core.Entities
{
    public class Exercise : IAuditable
    {
        public Exercise()
        {
            this.ImageFiles = new HashSet<ExerciseFile>();
        }
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public int CourseId { get; set; }
        public string Subject { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<ExerciseFile> ImageFiles { get; set; }
        public virtual Course Course { get; set; }
        public int ExeciseTime { get; set; }


    }
}
