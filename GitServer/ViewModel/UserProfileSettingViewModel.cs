using System.ComponentModel.DataAnnotations;

namespace GitServer.ViewModel
{
    public class UserProfileSettingViewModel
    {
        [Required] public string Name { get; set; }

        [Required] public string Email { get; set; }
        public string Description { get; set; }
        public string WebSite { get; set; }

        public UserProfileSettingViewModel()
        {
        }

        public UserProfileSettingViewModel(string name, string email, string description, string webSite)
        {
            Name = name;
            Email = email;
            Description = description;
            WebSite = webSite;
        }
        public UserProfileSettingViewModel(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}