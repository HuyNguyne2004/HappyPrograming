namespace HappyPrograming.Service
{
    public class OTPService
    {

        private static Dictionary<string, string> _otpStorage = new();

        public string GenerateOTP(string email)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            _otpStorage[email] = otp;
            return otp;
        }

        public bool ValidateOTP(string email, string inputOtp)
        {
            if (_otpStorage.ContainsKey(email) && _otpStorage[email] == inputOtp)
            {
                _otpStorage.Remove(email);
                return true;
            }
            return false;
        }
    }
}
