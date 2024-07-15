using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restaurante.Api.Functions
{
    public class Phone
    {
        public bool VerifyPhone(string phone, out string newPhone)
        {
            newPhone = "";
            if (phone.Any(c => char.IsLetter(c) == true))
                return false;

            string model = "+55";
            if (phone.Length != 14)
                phone = model + phone;
            
            if (phone[0] == '+' && phone[0] == '5' && phone[2] == '5' && phone.Length == 14)
            {
                newPhone = phone;
                return true;
            }
            return true;
        }
    }
}