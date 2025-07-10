namespace Marketplace.Models
{
    public class RegisterViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public string? NamaLengkap { get; set; }
        public string? NoHp { get; set; }
        public string? Alamat { get; set; }
    }
}
