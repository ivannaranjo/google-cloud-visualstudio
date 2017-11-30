﻿// Copyright 2017 Google Inc. All Rights Reserved.
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

using Google.Apis.CloudResourceManager.v1.Data;
using GoogleCloudExtension.Accounts;
using GoogleCloudExtension.DataSources;
using GoogleCloudExtension.ManageAccounts;
using GoogleCloudExtension.Utils;
using GoogleCloudExtension.Utils.Async;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GoogleCloudExtension.PickProjectDialog
{
    /// <summary>
    /// View model for picking a project id.
    /// </summary>
    public class PickProjectIdViewModel : ViewModelBase
    {
        private IEnumerable<Project> _projects;
        private Project _selectedProject;
        private AsyncProperty _loadTask;
        private bool _allowAccountChange;
        private string _filter;

        private readonly IPickProjectIdWindow _owner;
        private readonly Func<IResourceManagerDataSource> _resourceManagerDataSourceFactory;
        private readonly Action _promptAccountManagement;

        /// <summary>
        /// Result of the view model after the dialog window is closed. Remains
        /// null until an action buttion is clicked.
        /// </summary>
        public Project Result { get; private set; }

        /// <summary>
        /// Command to open the manage users dialog.
        /// </summary>
        public ProtectedCommand ChangeUserCommand { get; }

        /// <summary>
        /// Command to confirm the selection of a project id.
        /// </summary>
        public ProtectedCommand OkCommand { get; }

        /// <summary>
        /// The list of projects available to the current user.
        /// </summary>
        public IEnumerable<Project> Projects
        {
            get { return _projects; }
            private set { SetValueAndRaise(ref _projects, value); }
        }

        /// <summary>
        /// The project selected from the list of current projects.
        /// </summary>
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                SetValueAndRaise(ref _selectedProject, value);
                OkCommand.CanExecuteCommand = !string.IsNullOrEmpty(SelectedProject?.ProjectId);
            }
        }

        /// <summary>
        /// The property that surfaces task completion information for the Load Projects task.
        /// </summary>
        public AsyncProperty LoadTask
        {
            get { return _loadTask; }
            set { SetValueAndRaise(ref _loadTask, value); }
        }

        public string Filter
        {
            get { return _filter; }
            set { SetValueAndRaise(ref _filter, value); }
        }

        public bool AllowAccountChange
        {
            get { return _allowAccountChange; }
            set { SetValueAndRaise(ref _allowAccountChange, value); }
        }

        public PickProjectIdViewModel(IPickProjectIdWindow owner, bool allowAccountChange)
            : this(owner, DataSourceFactories.CreateResourceManagerDataSource, ManageAccountsWindow.PromptUser)
        {
            AllowAccountChange = allowAccountChange;
        }

        /// <summary>
        /// For Testing.
        /// </summary>
        /// <param name="owner">The window or mock window that owns this ViewModel.</param>
        /// <param name="dataSourceFactory">The factory of the source of projects.</param>
        /// <param name="promptAccountManagement">Action to prompt managing accounts.</param>
        internal PickProjectIdViewModel(
            IPickProjectIdWindow owner,
            Func<IResourceManagerDataSource> dataSourceFactory,
            Action promptAccountManagement)
        {
            _owner = owner;
            _resourceManagerDataSourceFactory = dataSourceFactory;
            _promptAccountManagement = promptAccountManagement;

            ChangeUserCommand = new ProtectedCommand(OnChangeUserCommand);
            OkCommand = new ProtectedCommand(OnOkCommand, canExecuteCommand: false);
            StartLoadProjects();
        }

        public bool FilterItem(object item)
        {
            var project = item as Project;
            if (project == null)
            {
                return false;
            }

            // If there is no filter, allow the item.
            if (string.IsNullOrEmpty(Filter))
            {
                return true;
            }

            // Check name and project id for the filter.
            return project.ProjectId.Contains(Filter) || project.Name.Contains(Filter);
        }

        private void StartLoadProjects()
        {
            if (CredentialsStore.Default.CurrentAccount != null)
            {
                LoadTask = AsyncPropertyUtils.CreateAsyncProperty(LoadProjectsAsync());
            }
            else
            {
                LoadTask = null;
            }
        }

        private async Task LoadProjectsAsync()
        {
            // The projects list will be empty while we load.
            Projects = Enumerable.Empty<Project>();

            // Updat the to loaded list of projects.
            Projects = (await CredentialsStore.Default.CurrentAccountProjects) ?? Enumerable.Empty<Project>();

            // Choose project from the list.
            if (SelectedProject == null)
            {
                SelectedProject = Projects.FirstOrDefault(p => p.ProjectId == CredentialsStore.Default.CurrentProjectId);
            }
            else
            {
                SelectedProject = Projects.FirstOrDefault(p => p.ProjectId == SelectedProject.ProjectId);
            }
        }

        private void OnChangeUserCommand()
        {
            _promptAccountManagement();
            StartLoadProjects();
        }

        private void OnOkCommand()
        {
            Result = SelectedProject;
            _owner.Close();
        }
    }
}