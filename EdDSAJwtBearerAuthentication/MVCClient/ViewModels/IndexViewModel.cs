namespace MVCClient.ViewModels
{
    public class IndexViewModel
    {
        public string FullUserName { get; set; }
        public string[] Roles { get; set; }
        public string Token { get; set; }
        public string InformationMessage { get; set; }
        public UserCredentials UserCredential { get; set; }
    }
}
