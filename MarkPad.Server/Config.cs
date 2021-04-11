// Config.cs

namespace MarkPad.Server
{
    public static class Config
    {
        private const string FilePath = "config.json";

        public static string Title => Config.userConfig != null ? Config.userConfig.Title : "MarkPad";
        public static string ListenUri => Config.userConfig != null ? Config.userConfig.ListenUri : "http://localhost";
        public static int MaxConnections => Config.userConfig != null ? Config.userConfig.MaxConnections : 16;
        public static bool EnableTraces => Config.userConfig != null ? Config.userConfig.EnableTraces : false;

        public static bool RequireAuth => Config.userConfig != null ? Config.userConfig.RequireAuth : true;
        public static string Username => Config.userConfig != null ? Config.userConfig.Username : "admin";
        public static string Password => Config.userConfig != null ? Config.userConfig.Password : "yKkTF9ZUGOJIltYsYQyy6/qPW0EGK5+9k3ZrtPbLTh1hEqT9";

        public static string DatabasePath => Config.userConfig != null ? Config.userConfig.DatabasePath : "markpad.db";
        public static string DatabasePrevPath => Config.userConfig != null ? Config.userConfig.DatabasePrevPath : "markpad.prev.db";
        public static string PostDirectory => Config.userConfig != null ? Config.userConfig.PostDirectory : "posts";
        public static string PostExtension => Config.userConfig != null ? Config.userConfig.PostExtension : "md";
        public static string[] UploadExtensions => Config.userConfig != null ? Config.userConfig.UploadExtensions : new [] { "jpg", "jpeg", "png", "gif", "webp", "svg" };

        private static UserConfig userConfig;

        public static bool Load()
        {
            string json;

            if (System.IO.File.Exists(Config.FilePath))
            {
                json = System.IO.File.ReadAllText(Config.FilePath);
                Config.userConfig = Newtonsoft.Json.JsonConvert.DeserializeObject<UserConfig>(json);
                Config.userConfig.ListenUri = Config.userConfig.ListenUri.TrimEnd('/');

                return true;
            }

            UserConfig cfg = new UserConfig()
            {
                Title = Config.Title,
                ListenUri = Config.ListenUri,
                MaxConnections = Config.MaxConnections,
                EnableTraces = Config.EnableTraces,

                RequireAuth = Config.RequireAuth,
                Username = Config.Username,
                Password = Config.Password,

                DatabasePath = Config.DatabasePath,
                DatabasePrevPath = Config.DatabasePrevPath,
                PostDirectory = Config.PostDirectory,
                PostExtension = Config.PostExtension,
                UploadExtensions = Config.UploadExtensions
            };

            json = Newtonsoft.Json.JsonConvert.SerializeObject(cfg, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(Config.FilePath, json);

            return false;
        }

        public static string GetConfigFileContent()
        {
            if (System.IO.File.Exists(Config.FilePath))
            {
                return System.IO.File.ReadAllText(Config.FilePath);
            }

            return "FileNotFound";
        }

        private class UserConfig
        {
            public string Title;
            public string ListenUri;
            public int MaxConnections;
            public bool EnableTraces;

            public bool RequireAuth;
            public string Username;
            public string Password;

            public string DatabasePath;
            public string DatabasePrevPath;
            public string PostDirectory;
            public string PostExtension;
            public string[] UploadExtensions;
        }
    }
}
