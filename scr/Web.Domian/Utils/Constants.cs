using System.Collections.Generic;

namespace Web.Domain.Utils
{
    public static class Constants
    {
        public const string DefaultFromEmail = "burhanriaz35@gmail.com";
        public const string DefaultEmailDisplayName = "Web Project";

        public const int MinCohealerAge = 18;
        public const string TimeZoneIdDefault = "America/Los_Angeles";
        public const string LanguageCodeDefault = "en";
        public const int MaxSocialMediaLinksToHave = 15;
        public const int MaxSecurityQuestionsNumber = 5;
        public const int MinSecurityQuestionsNumber = 3;
        public const int CohealerDashboardContributionsCount = 10;
        public const int DateTimeSearchAccuracyMins = 1;
        public static readonly string[] BadWordsArray = { "fuck", "bitch" };
        public const string CohereDefaultLogo = "https://coherepublic-test.s3.amazonaws.com/626963f9381c7427157e4c3e/f46e486f-1209-451b-89a2-45d316ea6d2b.png";

        // Email templates Name 
        public static class EmailTemplates
        {
            public const string DirectoryBaseName = "EmailTemplates";
            public static class Account
            {
                public static string EmailConfirmation = $"{DirectoryBaseName}/{DirectoryName}/EmailConfirmation.html";
                public static string PasswordResetEmail = $"{DirectoryBaseName}/{DirectoryName}/PasswordResetEmail.html";

                private const string DirectoryName = nameof(Account);
            }
        }
  

        public static class EmailtemplateButtons
        {
            public static string GoogleCalendarButton = @"<div style='padding: 20px 0px'>
                    <a href='{GoogleImportLink}'
                       style='
                padding: 12px;
                background-color: #fff;
                border: 3px solid #0b6481;
                text-decoration: none;
                font-family: Roboto, sans-serif;
                font-weight: 800;
                color: #0b6481;
                width: 300px;
                display:block;
              '>Add session to calendar (Google Calendar)</a>
                </div>";
            public static string IcalButton = @"<div>
                    <a href='{IcalFileUrl}'
                       style='
                padding: 12px;
                background-color: #fff;
                border: 3px solid #0b6481;
                text-decoration: none;
                font-family: Roboto, sans-serif;
                font-weight: 800;
                color: #0b6481;
                width: 300px;
                display:block;
              '>Add session to calendar (iCal)</a>
                </div>";
            public static string AllSessionsButton = @"<div style='padding: 50px; text-align: center'>
                            <a href='{IcalFileUrl}'
                               style='
                    padding: 12px 30px;
                    background-color: #fff;
                    border: 3px solid #0b6481;
                    text-decoration: none;
                    font-family: Roboto, sans-serif;
                    font-weight: 800;
                    color: #0b6481;
                    font-size: 12px;
                  '>Add all sessions to calendar (iCal)</a>
                        </div>";
        }
    }
}
