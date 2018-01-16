﻿using Google.Apis.Auth.OAuth2;
using GoogleCloudExtension.DockerUtils.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtension.DockerUtils
{
    public class DockerRepoDataSource
    {
        private readonly string _projectId;
        private readonly GoogleCredential _credentials;
        private readonly HttpClient _client;

        public DockerRepoDataSource(string projectId, GoogleCredential credential)
        {
            _projectId = projectId;
            _credentials = credential;

            _client = new HttpClient();
        }

        public async Task<RepoTags> GetRepoTagsAsync(string repo, string root = "")
        {
            var name = GetFullName(root);
            var url = GetListTagsUrl(repo: repo, name: name);
            var userCredentials = await GetUserCredentialsAsync();

            using (var request = new HttpRequestMessage { Method = HttpMethod.Get, RequestUri = new Uri(url) })
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", userCredentials);
                using (var response = await _client.SendAsync(request))
                {
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<RepoTags>(result);
                }
            }
        }

        public string GetFullPath(string repo, string name, string hash) => $"{repo}/{_projectId}/{name}@{hash}";

        public string GetDockerImageName(string repo, string name, string tag) => $"{repo}/{_projectId}/{name}:{tag}";

        private async Task<string> GetUserCredentialsAsync()
        {
            var token = await _credentials.UnderlyingCredential.GetAccessTokenForRequestAsync();
            var credentials = $"_token:{token}";
            var credentialsBytes = Encoding.ASCII.GetBytes(credentials);
            return Convert.ToBase64String(credentialsBytes);
        }

        private string GetListTagsUrl(string repo, string name)
        {
            return $"https://{repo}/v2/{name}/tags/list";
        }

        private string GetFullName(string root)
        {
            if (String.IsNullOrEmpty(root))
            {
                return _projectId;
            }
            else
            {
                return $"{_projectId}/{root}";
            }
        }
    }
}
