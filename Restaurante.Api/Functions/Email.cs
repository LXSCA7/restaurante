using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurante.Api.Functions
{
    public class Email
    {
        public static bool VerifyMail(string email)
        {
            if (email.Any(c => c == '@') && email.Length > 1)
                return true;
            
            return false;
        }
    }
}