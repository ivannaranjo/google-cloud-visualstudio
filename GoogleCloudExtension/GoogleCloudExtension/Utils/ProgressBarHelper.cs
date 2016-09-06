﻿// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Microsoft.VisualStudio.Shell.Interop;
using System;

namespace GoogleCloudExtension.Utils
{
    public class ProgressBarHelper : IProgress<double>, IDisposable
    {
        private const uint Total = 1000;

        private readonly IVsStatusbar _statusbar;
        private readonly string _label;
        private uint _cookie;

        public ProgressBarHelper(IVsStatusbar statusbar, string label)
        {
            _statusbar = statusbar;
            _label = label ?? "";

            _statusbar.Progress(ref _cookie, 1, "", 0, 0);
        }

        #region IDisposable

        public void Dispose()
        {
            _statusbar.Progress(ref _cookie, 0, "", 0, 0);
        }

        #endregion

        #region IProgress<double>

        void IProgress<double>.Report(double value)
        {
            _statusbar.Progress(ref _cookie, 1, _label, (uint)(value * Total), Total);
        }

        #endregion
    }
}