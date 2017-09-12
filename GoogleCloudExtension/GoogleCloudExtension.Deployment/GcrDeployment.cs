using GoogleCloudExtension.GCloud;
using GoogleCloudExtension.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.Deployment
{
    public static class GcrDeployment
    {
        public class DeploymentOptions
        {
            public string ImageName { get; set; }

            public string ImageTag { get; set; }

            public GCloudContext GCloudContext { get; set; }
        }

        public static async Task<bool> PublishProjectAsync(
            IParsedProject project,
            DeploymentOptions options,
            IProgress<double> progress,
            IToolsPathProvider toolsPathProvider,
            Action<string> outputAction)
        {
            var stageDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(stageDirectory);
            progress.Report(0.1);

            using (var cleanup = new Disposable(() => CommonUtils.Cleanup(stageDirectory)))
            {
                var appRootPath = Path.Combine(stageDirectory, "app");
                var buildFilePath = Path.Combine(stageDirectory, "cloudbuild.yaml");

                if (!await ProgressHelper.UpdateProgress(
                        NetCoreAppUtils.CreateAppBundleAsync(project, appRootPath, toolsPathProvider, outputAction),
                        progress,
                        from: 0.1, to: 0.3))
                {
                    Debug.WriteLine("Failed to create app bundle.");
                    return false;
                }

                NetCoreAppUtils.CopyOrCreateDockerfile(project, appRootPath);
                var image = CloudBuilderUtils.CreateBuildFile(
                    project: options.GCloudContext.ProjectId,
                    imageName: options.ImageName,
                    imageVersion: options.ImageTag,
                    buildFilePath: buildFilePath);

                if (!await ProgressHelper.UpdateProgress(
                    GCloudWrapper.BuildContainerAsync(buildFilePath, appRootPath, outputAction, options.GCloudContext),
                    progress,
                    from: 0.4, to: 1.0))
                {
                    Debug.WriteLine("Failed to build image.");
                    return false;
                }
                progress.Report(1.0);
                return true;
            }
        }
    }
}
