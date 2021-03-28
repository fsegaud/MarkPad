// Program.cs

namespace MarkPad.Server
{
    internal class Program
    {
        public const string Version = "v0.1-dev";

        public const string Title = "MarkPad";
        public const string ListenUri = "http://localhost";
        public const string PublicUri = "http://localhost";
        public const string DatabasePath = "markpad.db";
        public const string DatabasePrevPath = "markpad.prev.db";
        public const string PostDirectory = "posts";
        public const string PostExtension = ".md";
        public const int MaxConnections = 16;
        public const bool EnableTraces = true;
        public const CssHelper.Skin DefaultSkin = CssHelper.Skin.Dark;

        public static bool ExitRequest = false;

        private static void Main(string[] args)
        {
            System.Console.Write($"Connecting to database '{DatabasePath}'... ");
            Database.Connect(DatabasePath, DatabasePrevPath);
            //Database.CreateTestDataSet(true);
            System.Console.WriteLine("Done.");

            Nancy.Hosting.Self.HostConfiguration cfg = new Nancy.Hosting.Self.HostConfiguration();
            cfg.UrlReservations.CreateAutomatically = true;
            cfg.MaximumConnectionCount = MaxConnections;

            using (Nancy.Hosting.Self.NancyHost host = new Nancy.Hosting.Self.NancyHost(cfg, new System.Uri(ListenUri)))
            {
                System.Console.Write($"Listening on '{ListenUri}'... ");
                host.Start();
                System.Console.WriteLine("Done.");

                while (!Program.ExitRequest)
                {
                    System.Threading.Thread.Sleep(100);
                }
            }

            Database.Disconnect();
        }
    }
}
