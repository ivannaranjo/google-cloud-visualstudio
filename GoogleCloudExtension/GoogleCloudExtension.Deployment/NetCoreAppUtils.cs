﻿using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.Deployment
{
    internal static class NetCoreAppUtils
    {
        private const string DockerfileName = "Dockerfile";

        private static readonly Lazy<string> s_dotnetPath = new Lazy<string>(GetDotnetPath);

        /// <summary>
        /// This template is the smallest possible Dockerfile needed to deploy an ASP.NET Core app to
        /// App Engine Flex environment. It invokes the entry point .dll given by {0}, sets up the environment
        /// so the app listens on port 8080.
        /// All of the files composing the app are copied to the /app path, then it is set as the working directory.
        /// </summary>
        private const string DockerfileDefaultContent =
            "FROM b.gcr.io/aspnet-docker/aspnet:1.0.3\n" +
            "COPY . /app\n" +
            "WORKDIR /app\n" +
            "EXPOSE 8080\n" +
            "ENV ASPNETCORE_URLS=http://*:8080\n" +
            "ENTRYPOINT [\"dotnet\", \"{0}.dll\"]\n";

        internal static Task<bool> CreateAppBundleAsync(string projectPath, string stageDirectory, Action<string> outputAction)
        {
            var arguments = $"publish \"{projectPath}\" " +
                $"-o \"{stageDirectory}\" " +
                "-c Release";
            var externalTools = GetExternalToolsPath();
            var env = new Dictionary<string, string>
            {
                { "PATH", $"{Environment.GetEnvironmentVariable("PATH")};{externalTools}" },
            };

            Debug.WriteLine($"Using tools from {externalTools}");
            outputAction($"dotnet {arguments}");
            return ProcessUtils.RunCommandAsync(s_dotnetPath.Value, arguments, (o, e) => outputAction(e.Line), env);
        }

        internal static void CopyOrCreateDockerfile(string projectPath, string stageDirectory)
        {
            var sourceDir = Path.GetDirectoryName(projectPath);
            var sourceDockerfile = Path.Combine(sourceDir, DockerfileName);
            var targetDockerfile = Path.Combine(stageDirectory, DockerfileName);

            if (File.Exists(sourceDockerfile))
            {
                File.Copy(sourceDockerfile, targetDockerfile, overwrite: true);
            }
            else
            {
                var content = String.Format(DockerfileDefaultContent, CommonUtils.GetProjectName(projectPath));
                File.WriteAllText(targetDockerfile, content);
            }
        }

        private static string GetExternalToolsPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            return Path.Combine(programFilesPath, @"Microsoft Visual Studio 14.0\Web\External");
        }

        private static string GetDotnetPath()
        {
            var programFilesPath = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
            return Path.Combine(programFilesPath, @"dotnet\dotnet.exe");
        }
    }
}
