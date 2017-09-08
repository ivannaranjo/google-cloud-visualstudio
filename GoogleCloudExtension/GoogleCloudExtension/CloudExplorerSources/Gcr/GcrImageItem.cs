using GoogleCloudExtension.DataSources.Docker;
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
            : base(className: "Docker image", componentName: hash)
        {
            _hash = hash;
            _fullPath = fullPath;
            _image = image;
        }

        public string Size => StringFormatUtils.FormatByteSize(_image.Size);

        public string Hash => _hash;

        public string FullPath => _fullPath;
    }
}