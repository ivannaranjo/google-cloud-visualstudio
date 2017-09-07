using GoogleCloudExtension.GCloud.Models;
using GoogleCloudExtension.Utils;

namespace GoogleCloudExtension.CloudExplorerSources.Gcr
{
    public  class GcrImageItem : PropertyWindowItemBase
    {
        private GcrImage _image;

        public GcrImageItem(GcrImage image)
            : base(className: "GCR Image", componentName: image.Name)
        {
            _image = image;
        }

        public string Name => _image.Name;
    }
}