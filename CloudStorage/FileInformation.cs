using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CloudStorage
{
    class FileInformation : ObservableCollection<string>
    {
        string dir;

        Dictionary<string, string> files = new Dictionary<string, string>();
        public FileInformation(string dir)
        {
            this.dir = dir;

            Refresh();
            
        }

        public FileInformation Refresh()
        {
            ClearItems();
            this.files.Clear();

            string[] files = Directory.GetFiles(dir);

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);

                Add(fileInfo.Name);

                
                this.files.Add(fileInfo.Name, fileInfo.FullName);
            }

            return this;
        }


        public string GetFilePath(string fileName)
        {
            return files[fileName];
        }

        public void RemoveFilePath(string fileName)
        {
            files.Remove(fileName);
        }

    }
}
