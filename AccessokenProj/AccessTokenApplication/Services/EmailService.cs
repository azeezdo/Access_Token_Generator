using System;
using System.Net;
using System.Net.Mail;
using AccessTokenDomain.Interfaces.IServices;
using AccessTokenDomain.Model.Request;
using AccessTokenDomain.Model.Response;

namespace AccessTokenApplication.Services
{
	public class EmailService : IEmailService
	{
        public async Task<CustomResponse> SendVerificationEmail(EmailModel model)
        {
            string fromEmail = "doazeez89@gmail.com";
            string fromPassword = ""; 
            string subject = "Account Verification";
            string baseUrl = "https://localhost:7055/swagger/index.html/verify";
            string verificationLink = $"{baseUrl}?email={Uri.EscapeDataString(model.EmailAddress)}&otp={model.OTP}";

            string body = $@"
            Hi {model.FirstName},<br><br>
            Please verify your email address by clicking the link below:<br>
            <a href='{verificationLink}'>Verify Email</a><br><br>
            Thank you!
        ";

            var smtpClient = new SmtpClient("smtp.gmail.com", 587) 
            {
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(fromEmail, model.EmailAddress, subject, body)
            {
                IsBodyHtml = true
            };

            try
            {
                smtpClient.Send(mailMessage);
                return new CustomResponse { ResponseCode = 200, ResponseMessage = "Verification email sent successfully." };
                
            }
            catch (Exception ex)
            {
                return new CustomResponse { ResponseCode = 400, ResponseMessage = "Error sending email: " + ex.Message };
            }
        }
    }
}

