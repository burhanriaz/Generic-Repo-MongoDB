using System;
using System.Net.Mail;

namespace Web.Domain.Utils.Validators.User
{
    public static class Email
    {
        public static bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}

