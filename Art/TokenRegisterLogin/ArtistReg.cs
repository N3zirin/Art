namespace Art.TokenRegisterLogin
{
    public class ArtistReg
    {
        public string nAME { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
