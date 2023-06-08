using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DoNotCopy.Core.Entities
{
    public class Student : IAuditable
    {
        public Student()
        {
            this.ImageFiles = new HashSet<StudentFile>();
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string IdentityCard { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public virtual ICollection<StudentFile> ImageFiles { get; set; }

    }
}
