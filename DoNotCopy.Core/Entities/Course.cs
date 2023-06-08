using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoNotCopy.Core.Entities
{
    public class Course : IAuditable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int LecturerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual Lecturer Lecturer { get; set; }

    }
}
