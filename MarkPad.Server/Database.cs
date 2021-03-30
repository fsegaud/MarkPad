// Database.cs

namespace MarkPad.Server
{
    using System.Collections.Generic;

    public class Database
    {
        private static SQLite.SQLiteConnection connection;

        public static void Connect(string dbPath, string dbBackupPath = null)
        {
            if (!string.IsNullOrEmpty(dbBackupPath) && System.IO.File.Exists(dbPath))
            {
                System.IO.File.Copy(dbPath, dbBackupPath, true);
            }

            Database.connection = new SQLite.SQLiteConnection(dbPath);
            Database.connection.CreateTable<Post>();
        }

        public static void CreateTestDataSet(bool dropExistingData = false)
        {
            if (dropExistingData)
            {
                Database.connection.DropTable<Post>();
                Database.connection.CreateTable<Post>();
            }

            Post post = new Post("", "rootfile");
            post.Write("# Root file\n\nThis is the root file.");
            Database.InsertPost(post);
            post = new Post("path/to", "file");
            post.Write("# File\n\nLocated in the `path/to` directory.");
            Database.InsertPost(post);
            post = new Post("path/to", "another-file");
            post.Write("# Another File\n\nLocated in the `path/to` pâtes été ça.");
            Database.InsertPost(post);
        }

        public static void Disconnect()
        {
            Database.connection.Close();
        }

        public static Post GetPost(int id)
        {
            try
            {
                return Database.connection.Get<Post>(p => p.Id == id);
            }
            catch (System.InvalidOperationException e)
            {
                return null;
            }
        }

        public static Post GetPost(string path, string name)
        {
            path = path.FixPathSlashes();
            try
            {
                return Database.connection.Get<Post>(p => p.Path == path && p.Name == name);
            }
            catch (System.InvalidOperationException)
            {
                return null;
            }
        }

        public static Post GetPost(string fullpath)
        {
            fullpath = fullpath.FixPathSlashes();
            return GetPost(System.IO.Path.GetDirectoryName(fullpath), System.IO.Path.GetFileName(fullpath));
        }

        public static IEnumerable<Post> GetPosts()
        {
            return Database.connection.Query<Post>("SELECT * FROM Posts;");
        }

        public static void InsertPost(Post post)
        {
            Database.connection.Insert(post);
        }

        public static void UpdatePost(Post post)
        {
            Database.connection.Update(post);
        }
    }

    [SQLite.Table("Posts")]
    public class Post
    {
        public Post()
        {
        }

        public Post(string path, string name)
        {
            this.Path = path.EscapeBlankSpaces();
            this.Name = name.EscapeBlankSpaces();

            this.Guid = System.Guid.NewGuid();
            this.Created = this.Modified = System.DateTime.Now;

            this.Shared = false;
        }

        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id
        {
            get;
            set;
        }

        [SQLite.MaxLength(64)]
        public string Name
        {
            get;
            set;
        }

        [SQLite.MaxLength(256)]
        public string Path
        {
            get;
            set;
        }

        [SQLite.Ignore] public string FullPath => System.IO.Path.Combine(this.Path, this.Name).FixPathSlashes();

        public System.Guid Guid
        {
            get;
            set;
        }

        [SQLite.Ignore] public string ShortGuid => this.Guid.ToString().Substring(0, 8);

        public System.DateTime Created
        {
            get;
            set;
        }

        public System.DateTime Modified
        {
            get;
            set;
        }

        public bool Shared
        {
            get;
            set;
        }
    }

    public static class PostExtension
    {
        public static string Read(this Post post)
        {
            string filePath = System.IO.Path.Combine(Program.PostDirectory, $"{post.Guid}{Program.PostExtension}");
            if (!System.IO.File.Exists(filePath))
            {
                return null;
            }

            return System.IO.File.ReadAllText(filePath);
        }

        public static void Write(this Post post, string content)
        {
            if (!System.IO.Directory.Exists(Program.PostDirectory))
            {
                System.IO.Directory.CreateDirectory(Program.PostDirectory);
            }

            string filePath = System.IO.Path.Combine(Program.PostDirectory, $"{post.Guid}{Program.PostExtension}");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            System.IO.File.WriteAllText(filePath, content);
        }
    }

    public static class StringExtension
    {
        public static string FixPathSlashes(this string str)
        {
            return str.Replace('\\', '/');
        }

        public static string EscapeBlankSpaces(this string str)
        {
            return str.Replace(' ', '-');
        }
    }
}