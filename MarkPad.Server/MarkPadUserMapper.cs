// MarkPadUserMapper.cs

namespace MarkPad.Server
{
    public class MarkPadUserMapper : Nancy.Authentication.Forms.IUserMapper
    {
        private static System.Guid guid = System.Guid.NewGuid();

        public static System.Guid? ValidateUser(string username, string password)
        {
            if (username == Program.Username && PasswordHelper.Check(password, Program.Password))
            {
                return MarkPadUserMapper.guid;
            }

            return null;
        }

        public System.Security.Claims.ClaimsPrincipal GetUserFromIdentifier(System.Guid identifier, Nancy.NancyContext context)
        {
            if (identifier == MarkPadUserMapper.guid)
            {
                return new System.Security.Claims.ClaimsPrincipal(new System.Security.Principal.GenericIdentity(Program.Username));
            }

            return null;
        }
    }
}