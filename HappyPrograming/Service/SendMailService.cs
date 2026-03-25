namespace HappyPrograming.Service
{
    public class SendMailService
    {
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            await Task.Run(() => Console.WriteLine($"Gửi mail tới {email}: {body}"));
        }
    }
}
