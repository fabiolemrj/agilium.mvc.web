using agilium.api.business.Interfaces;

namespace agilium.api.manager.Logger
{
    public class UserResolverService
    {
        private readonly IUser _aspNetUser;

        public string username { get; set; }
        public UserResolverService()
        {

        }

        public UserResolverService(string name)
        {
            username = name;
        }


    }
}
