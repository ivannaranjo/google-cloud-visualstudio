using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.Deployment
{
    public interface IToolsPathProvider
    {
        /// <summary>
        /// Returns the path to use for external tools to be used during deployment.
        /// </summary>
        string GetExternalToolsPath();

        /// <summary>
        /// Returns the path to the dotnet.exe tool to use during deployment.
        /// </summary>
        string GetDotnetPath();
    }
}
