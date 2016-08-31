﻿using EnvDTE;
using Google.Apis.Compute.v1.Data;
using GoogleCloudExtension.DataSources;
using GoogleCloudExtension.GCloud;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.Utils
{
    internal static class AspNetPublisher
    {
        private static readonly Lazy<string> s_msbuildPath = new Lazy<string>(GetMsbuildPath);
        private static readonly Lazy<string> s_msdeployPath = new Lazy<string>(GetMsdeployPath);

        public static async Task PublishAppAsync(
            EnvDTE.Project project,
            Instance targetInstance,
            WindowsInstanceCredentials credentials,
            Action<string> outputAction)
        {
            var stageDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(stageDirectory);

            var publishSettingsPath = Path.GetTempFileName();
            var publishSettingsContent = targetInstance.GeneratePublishSettings(credentials.User, credentials.Password);
            File.WriteAllText(publishSettingsPath, publishSettingsContent);

            if (!await CreateAppBundleAsync(project, stageDirectory, outputAction))
            {
                outputAction($"Failed to publish project {project.Name}");
                return;
            }

            await DeployAppAsync(stageDirectory, publishSettingsPath, outputAction);

            File.Delete(publishSettingsPath);
        }

        private static async Task<bool> DeployAppAsync(string stageDirectory, string publishSettingsPath, Action<string> outputAction)
        {
            var arguments = "-verb:sync " +
                $@"-source:contentPath=""{stageDirectory}"" " +
                $@"-dest:contentPath=""Default Web Site"",publishSettings=""{publishSettingsPath}"" " +
                "-allowUntrusted";

            outputAction($"Publishing projet with command:");
            outputAction($"msdeploy.exe {arguments}");
            var result = await ProcessUtils.RunCommandAsync(s_msdeployPath.Value, arguments, (o, e) => outputAction(e.Line));
            if (result)
            {
                outputAction("Command succeeded.");
            }
            else
            {
                outputAction("Command failed.");
            }

            return result;
        }

        private static async Task<bool> CreateAppBundleAsync(EnvDTE.Project project, string stageDirectory, Action<string> outputAction)
        {
            var arguments = $@"""{project.FullName}""" + " " +
                "/p:Configuration=Release " +
                "/p:Platform=AnyCPU " +
                "/t:WebPublish " +
                "/p:WebPublishMethod=FileSystem " +
                "/p:DeleteExistingFiles=True " +
                $@"/p:publishUrl=""{stageDirectory}""";

            outputAction($"Execution command:");
            outputAction($"msbuild.exe {arguments}");
            var result = await ProcessUtils.RunCommandAsync(s_msbuildPath.Value, arguments, (o, e) => outputAction(e.Line));
            if (result)
            {
                outputAction("Coommand succeeded.");
            }
            else
            {
                outputAction("Command failed.");
            }

            return result;
        }

        private static string GetMsbuildPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            return Path.Combine(programFilesPath, @"MSBuild\14.0\Bin\MSBuild.exe");
        }

        private static string GetMsdeployPath()
        {
            var programFilesPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            return Path.Combine(programFilesPath, @"IIS\Microsoft Web Deploy V3\msdeploy.exe");
        }
    }
}