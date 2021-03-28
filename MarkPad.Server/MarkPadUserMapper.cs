// MarkPadUserMapper.cs

//namespace MarkPad.Server
//{
//    public class MarkPadUserMapper : Nancy.Authentication.Forms.IUserMapper
//    {
//        public static System.Guid? ValidateUser(string username, string password)
//        {
//            Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser(username);

//            if (user == null || !user.CheckPassword(password))
//            {
//                return null;
//            }

//            return user.Guid;
//        }

//        public System.Security.Claims.ClaimsPrincipal GetUserFromIdentifier(System.Guid identifier, Nancy.NancyContext context)
//        {
//            Nojira.Utils.Database.User user = Nojira.Utils.Database.GetUser(identifier);
//            if (user == null)
//            {
//                return null;
//            }

//            return new System.Security.Claims.ClaimsPrincipal(new System.Security.Principal.GenericIdentity(user.UserName));
//        }
//    }
//}
