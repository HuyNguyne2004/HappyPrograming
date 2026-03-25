namespace HappyPrograming.Service.Interface
{
    public interface IOtpService
    {
        string GenerateOTP(string email);
        bool VerifyOTP(string email, string otp);
    }
}
