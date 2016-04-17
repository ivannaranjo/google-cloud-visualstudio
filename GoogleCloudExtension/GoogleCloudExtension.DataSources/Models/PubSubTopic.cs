﻿// Copyright 2016 Google Inc. All Rights Reserved.
// Licensed under the Apache License Version 2.0.

using Newtonsoft.Json;

namespace GoogleCloudExtension.DataSources.Models
{
    public class PubSubTopic
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}