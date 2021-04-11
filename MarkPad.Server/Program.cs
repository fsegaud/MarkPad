// Program.cs

namespace MarkPad.Server
{
    internal class Program
    {
        public const string Version = "v0.1-dev";

        public static bool ExitRequest = false;

        private static void Main(string[] args)
        {
            if (args.Length == 2 && (args[0] == "-p" || args[0] == "--hash-password"))
            {
                System.Console.WriteLine(PasswordHelper.GetHash(args[1]));
                return;
            }

            System.Console.Write($"Loading configuration... ");
            bool result = Config.Load();
            System.Console.WriteLine(result ? "Done." : "Failed, falling back on default config.");

            System.Console.Write($"Connecting to database '{Config.DatabasePath}'... ");
            Database.Connect(Config.DatabasePath, Config.DatabasePrevPath);
            //Database.CreateTestDataSet(true);
            System.Console.WriteLine("Done.");

            Nancy.Hosting.Self.HostConfiguration cfg = new Nancy.Hosting.Self.HostConfiguration();
            cfg.UrlReservations.CreateAutomatically = true;
            cfg.MaximumConnectionCount = Config.MaxConnections;

            using (Nancy.Hosting.Self.NancyHost host = new Nancy.Hosting.Self.NancyHost(cfg, new System.Uri(Config.ListenUri)))
            {
                System.Console.Write($"Listening on '{Config.ListenUri}'... ");
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