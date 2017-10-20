﻿namespace GoogleCloudExtension.ApiManagement
{
    public static class KnownApis
    {
        // The API necessary to use App Engine services.
        public const string AppEngineAdminApiName = "appengine.googleapis.com";

        // The API necessary to use GCE.
        public const string ComputeEngineApiName = "compute.googleapis.com";

        // The API necessary to use GCS.
        public const string CloudStorageApiName = "storage-api.googleapis.com";

        // The API necessary to use GKS.
        public const string ContainerEngineApiName = "container.googleapis.com";

        // The API necessary to use the Cloud Builder.
        public const string CloudBuildApiName = "cloudbuild.googleapis.com";

        // The API necessary to manage Cloud SQL instances.
        public const string CloudSQLApiName = "sqladmin.googleapis.com";

        // The API necessary to manage Pub/Sub subscriptions.
        public const string PubSubApiName = "pubsub.googleapis.com";
    }
}
