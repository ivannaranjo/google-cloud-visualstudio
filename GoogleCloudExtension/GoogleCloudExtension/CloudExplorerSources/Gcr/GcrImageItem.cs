using GoogleCloudExtension.DockerUtils;
using GoogleCloudExtension.DockerUtils.Models;
using GoogleCloudExtension.GCloud.Models;
using GoogleCloudExtension.Utils;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    public  class GcrImageItem : PropertyWindowItemBase
    {
        private readonly string _hash;
        private readonly string _fullPath;
        private readonly RepoImage _image;

        public GcrImageItem(string hash, string fullPath, RepoImage image)
            : base(className: "Docker image", componentName: string.Join(",", image.Tags))
        {
            _hash = hash;
            _fullPath = fullPath;
            _image = image;
        }

        public string Size => StringFormatUtils.FormatByteSize(_image.Size);

        public string Hash => _hash;

        public string FullPath => _fullPath;

        public string Created => _image.GetCreatedDate().ToString();

        public string Uploaded => _image.GetUploadedDate().ToString();
    }
}