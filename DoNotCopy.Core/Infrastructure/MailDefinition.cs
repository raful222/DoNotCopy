using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace DoNotCopy.Core.Infrastructure
{
    public class MailDefinition
    {
        public class MemoryFile
        {
            public string Name { get; set; }

            public byte[] Bytes { get; set; }

            public string ContentType { get; set; }
        }

        public MailDefinition()
        {
            this.Destination = new List<MailAddress>();
            this.CarbonCopy = new List<MailAddress>();
            this.BlindCarbonCopy = new List<MailAddress>();
            this.Inline = new List<string>();
            this.InlineBytes = new List<MemoryFile>();
            this.Attachment = new List<string>();
            this.AttachmentBytes = new List<MemoryFile>();
        }

        public MailDefinition(MailAddress destination)
        {
            this.Destination = new List<MailAddress> { destination };
            this.CarbonCopy = new List<MailAddress>();
            this.BlindCarbonCopy = new List<MailAddress>();
            this.Inline = new List<string>();
            this.InlineBytes = new List<MemoryFile>();
            this.Attachment = new List<string>();
            this.AttachmentBytes = new List<MemoryFile>();
        }

        public virtual ICollection<MailAddress> Destination { get; set; }

        public virtual ICollection<MailAddress> CarbonCopy { get; set; }

        public virtual ICollection<MailAddress> BlindCarbonCopy { get; set; }

        public virtual string Subject { get; set; }

        public virtual string Body { get; set; }

        public virtual string Text { get; set; }

        public virtual ICollection<string> Inline { get; set; }

        public virtual ICollection<string> Attachment { get; set; }

        public virtual ICollection<MemoryFile> InlineBytes { get; set; }

        public virtual ICollection<MemoryFile> AttachmentBytes { get; set; }
    }
}