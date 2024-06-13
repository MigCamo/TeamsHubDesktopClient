using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsHubDesktopClient.Gateways.Provider
{
    public class FileManagementRESTProvider
    {
        public FileManagementRESTProvider() { }

        public bool AddFile() 
        { 
            return true; 
        }

        public bool DeleteFile() 
        { 
            return true; 
        }
        
        public bool GetAllFileByProject(int idProject) 
        { 
            return true; 
        }
        
        public bool GetFileInfo(int idFile) 
        {
            return true; 
        }

        public bool GetFile(string path)
        {
            return true;
        }


    }
}
