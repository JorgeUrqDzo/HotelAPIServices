using System.Collections.Generic;
using Hotel.Entities;
using Hotel.Services.Settings;

namespace Hotel.Services.Email
{
    public class ActivateNewUserEmailTemplate : IEmailTemplate
    {
        CustomSettings settings;
        User user;

        public ActivateNewUserEmailTemplate(User user, CustomSettings settings)
        {
            this.user = user;
            this.settings = settings;
        }

        public EmailMessage BuildMessage()
        {
            return new EmailMessage
            {
                To = new List<EmailAddress>() {new EmailAddress(user.Email, $"{user.FirstName} {user.LastName}")},
                Subject = "You have a new Hotel Api Services account",
                Content = $@"Hi {
                        user.FirstName
                    },<br><br>Welcome to Hotel Api Services!<br><br>You received this e-mail because one of our system administrators has created a new account for you.
                                Please click the following link to activate your account and set a new password.<br><br><a href='{
                        settings.PerfInsightsWebUrl
                    }/sessions/activate/{user.ActivationToken}'>Hotel Api Services Account Activation</a>
                                <br><br>Thank you!<br>Hotel Api Services Team.",
                IsHtml = true
            };
        }
    }
}