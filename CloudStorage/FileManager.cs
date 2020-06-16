using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CloudStorage
{
    class FileManager
    {
        string dir;
        
        public FileInformation fileInfo
        {
            get;
            
        }
        public FileManager(string dir)
        {
            this.dir = dir;

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            fileInfo = new FileInformation(dir);
        }

        public bool DeleteFile(string fileName)
        {
            try
            {
                File.Delete(fileName);
                

                return true;

            }catch(Exception e)
            {
                return false;
            }
            
        }

        public string GetFilePath(string fileName)
        {
            return fileInfo.GetFilePath(fileName);
        }

    }
}
