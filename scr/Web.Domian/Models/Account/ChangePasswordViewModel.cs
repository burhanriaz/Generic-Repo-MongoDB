namespace Web.Domain.Models.Account
{
    public class ChangePasswordViewModel
    {
        private string _emailLower;

        public string Email
        {
            get => _emailLower?.ToLower();
            set => _emailLower = value;
        }

        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
