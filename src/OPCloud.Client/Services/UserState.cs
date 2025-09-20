using OPCloud.Client.Models;

namespace OPCloud.Client.Services
{
    public class UserState
    {
        public CurrentUser User { get; private set; } = new CurrentUser
        {
            DisplayName = "Invitado",
            Avatar = "https://via.placeholder.com/40"
        };

        public event Action OnChange;

        public void SetUser(CurrentUser user)
        {
            User = user ?? new CurrentUser
            {
                DisplayName = "Invitado",
                Avatar = "https://via.placeholder.com/40"
            };
            NotifyStateChanged();

        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
