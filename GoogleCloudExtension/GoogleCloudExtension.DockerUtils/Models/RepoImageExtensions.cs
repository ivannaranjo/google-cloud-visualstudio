using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.DockerUtils.Models
{
    public static class RepoImageExtensions
    {
        private static readonly DateTime UnixEpoch =
            new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime GetCreatedDate(this RepoImage self) => DateFromMilliseconds(self.Created);

        public static DateTime GetUploadedDate(this RepoImage self) => DateFromMilliseconds(self.Uploaded);

        private static DateTime DateFromMilliseconds(ulong milliseconds) => UnixEpoch.AddMilliseconds(milliseconds);
    }
}
