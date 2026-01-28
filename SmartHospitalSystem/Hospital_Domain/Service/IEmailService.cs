using Hospital_Domain.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Domain.Service
{
    public interface IEmailService
    {
        void SendEmail(MailRequest mailRequest);
    }
}
