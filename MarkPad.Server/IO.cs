// IO.c s

using System.Linq;

namespace MarkPad.Server.IO
{
    using System.Collections.Generic;

    public class Directory
    {
        private Dictionary<string, Directory> directories = new Dictionary<string, Directory>();
        private Dictionary<string, Post> files = new Dictionary<string, Post>();

        public Directory[] Directories => this.directories.Values.ToArray();

        public Post[] Files => this.files.Values.ToArray();

        public Directory(string path, string name)
        {
            this.Path = path;
            this.Name = name;
        }

        public string Path
        {
            get;
        }

        public string Name
        {
            get;
        }

        public static Directory CreateHierarchy(Post[] files)
        {
            Directory root = new Directory(string.Empty, "/");
            root.Populate(files);
            return root;
        }

        public bool HasDirectory(string name)
        {
            return this.directories.ContainsKey(name);
        }

        public bool HasFile(string name)
        {
            return this.files.ContainsKey(name);
        }

        public Directory GetDirectory(string name)
        {
            this.directories.TryGetValue(name, out Directory directory);
            return directory;
        }

        public Directory CreateDirectory(string name)
        {
            Directory directory = new Directory(System.IO.Path.Combine(this.Path, name), name);
            this.directories.Add(name, directory);
            return directory;
        }

        public void Populate(Post[] files)
        {
            for (int pi = 0; pi < files.Length; pi++)
            {
                Directory currentDirectory = this;
                string[] tokens = files[pi].FullPath.Split(new char[]
                {
                    '/'
                }, System.StringSplitOptions.RemoveEmptyEntries);

                for (int ti = 0; ti < tokens.Length - 1; ti++)
                {
                    if (currentDirectory.HasDirectory(tokens[ti]))
                    {
                        currentDirectory = currentDirectory.GetDirectory(tokens[ti]);
                    }
                    else
                    {
                        currentDirectory = currentDirectory.CreateDirectory(tokens[ti]);
                    }
                }

                currentDirectory.files.Add(files[pi].Name, files[pi]);
            }
        }
    }
}
