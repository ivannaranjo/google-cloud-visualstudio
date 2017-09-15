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
                if (!await ProgressHelper.UpdateProgress(
                        NetCoreAppUtils.CreateAppBundleAsync(project, stageDirectory, toolsPathProvider, outputAction),
                        progress,
                        from: 0.1, to: 0.3))
                {
                    Debug.WriteLine("Failed to create app bundle.");
                    return false;
                }

                NetCoreAppUtils.CopyOrCreateDockerfile(project, stageDirectory);
                var imageTag = CloudBuilderUtils.GetImageTag(
                    project: options.GCloudContext.ProjectId,
                    imageName: options.ImageName,
                    imageVersion: options.ImageTag);

                if (!await ProgressHelper.UpdateProgress(
                    GCloudWrapper.BuildContainerAsync(
                        imageTag: imageTag,
                        contentsPath: stageDirectory,
                        outputAction: outputAction,
                        context: options.GCloudContext),
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
