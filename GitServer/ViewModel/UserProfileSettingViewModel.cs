using System.ComponentModel.DataAnnotations;

namespace GitServer.ViewModel
{
    public class UserProfileSettingViewModel
    {
        [Required] public string Name { get; set; }

        [Required] public string Email { get; set; }

        public UserProfileSettingViewModel()
        {
        }

        public UserProfileSettingViewModel(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}