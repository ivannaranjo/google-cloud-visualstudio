using GoogleCloudExtension.Deployment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.Projects
{
    /// <summary>
    /// This class represents a .NET Core project based on .csproj.
    /// </summary>
    internal class NetCoreCsprojProject : IParsedProject
    {
        public string DirectoryPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string FullPath
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public KnownProjectTypes ProjectType
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
