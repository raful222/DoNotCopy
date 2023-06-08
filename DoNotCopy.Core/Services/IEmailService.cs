using DoNotCopy.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoNotCopy.Core.Services
{
    public interface IEmailService
    {
        Task<bool> SendAsync(MailDefinition message);
    }
}
