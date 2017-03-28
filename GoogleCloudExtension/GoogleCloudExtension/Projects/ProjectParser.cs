using EnvDTE;
using GoogleCloudExtension.Deployment;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.Projects
{
    internal static class ProjectParser
    {
        // Extension used in .NET Core 1.0 project placeholders, the real project is in project.json.
        private const string XProjExtension = ".xproj";
        private const string ProjectJsonFileName = "project.json";

        // Extension used in .NET Core 1.0, 1.1... and .NET 4.x, etc...
        private const string CSProjExtension = ".csproj";

        /// <summary>
        /// Parses the given <seealso cref="Project"/> instance and returned a friendlier and more usable type to use for
        /// deplyment and other operations.
        /// </summary>
        /// <param name="project">The <seealso cref="Project"/> instance to parse.</param>
        /// <returns>The resulting <seealso cref="IParsedProject"/> or null if the project is not supported.</returns>
        public static IParsedProject ParseProject(Project project)
        {
            var extension = Path.GetExtension(project.FullName);
            switch (extension)
            {
                case XProjExtension:
                    Debug.WriteLine($"Processing a project.json: {project.FullName}");
                    return ParseProjectJson(project);

                case CSProjExtension:
                    Debug.WriteLine($"Processing a .csproj: {project.FullName}");
                    return ParseMsbuildProject(project);

                default:
                    return null;
            }
        }

        private static IParsedProject ParseMsbuildProject(Project project)
        {
            return new VsProject(project);
        }

        private static IParsedProject ParseProjectJson(Project project)
        {
            var projectDir = Path.GetDirectoryName(project.FullName);
            var projectJsonPath = Path.Combine(projectDir, ProjectJsonFileName);

            if (!File.Exists(projectJsonPath))
            {
                Debug.WriteLine($"Could not find {projectJsonPath}.");
                return null;
            }

            return new NetCoreProjectJsonProject(projectJsonPath);
        }
    }
}
