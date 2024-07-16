using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurante.Api.Functions
{
    public class Phone
    {
        public static bool VerifyPhone(string phone, out string newPhone)
        {
            newPhone = "";
            if (phone.Any(c => char.IsLetter(c) == true))
                return false;

            string model = "+55";
            if (phone.Length != 14)
                phone = model + phone;
            
            if (phone.Length == 14 && phone.StartsWith("+55"))
            {
                newPhone = phone;
                return true;
            }

            return false;
        }
    }
}