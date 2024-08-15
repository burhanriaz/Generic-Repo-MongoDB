namespace Web.Domain.Models.Account
{
    public class EmailConfirmationViewModel
    {
        private string _emailLower;

        public string Email
        {
            get => _emailLower?.ToLower();
            set => _emailLower = value;
        }

        public string Token { get; set; }
    }
}
