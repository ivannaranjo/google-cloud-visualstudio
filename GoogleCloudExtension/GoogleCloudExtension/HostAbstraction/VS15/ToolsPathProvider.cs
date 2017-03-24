using GoogleCloudExtension.Deployment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.HostAbstraction.VS15
{
    class ToolsPathProvider : IToolsPathProvider
    {
        private readonly string _edition;

        public ToolsPathProvider(string edition)
        {
            _edition = edition;
        }

        public string GetDotnetPath()
        {
            throw new NotImplementedException();
        }

        public string GetExternalToolsPath()
        {
            throw new NotImplementedException();
        }

        public string GetMsbuildPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            return Path.Combine(programFilesPath, $@"\Microsoft Visual Studio\2017\{_edition}\MSBuild\15.0\Bin\MSBuild.exe");
        }

        public string GetMsdeployPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Path.Combine(programFilesPath, @"IIS\Microsoft Web Deploy V3\msdeploy.exe");
        }
    }
}
