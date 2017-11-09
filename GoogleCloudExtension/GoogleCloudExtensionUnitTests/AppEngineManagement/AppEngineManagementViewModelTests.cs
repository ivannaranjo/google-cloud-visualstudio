﻿using GoogleCloudExtension.AppEngineManagement;
using GoogleCloudExtension.DataSources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleCloudExtensionUnitTests.AppEngineManagement
{
    /// <summary>
    /// Test class for <seealso cref="AppEngineManagementViewModel"/>
    /// </summary>
    [TestClass]
    public class AppEngineManagementViewModelTests
    {
        private const string TestProjectId = "TestProjectId";

        private static readonly List<string> s_mockFlexLocations = new List<string>
        {
            "us-something",
            "mars-other",
            "antartica-west-1d",
        };
        private static readonly List<string> s_sortedMockFlexLocations = s_mockFlexLocations.OrderBy(x => x).ToList();

        private TaskCompletionSource<IList<string>> _flexLocationsSource;
        private Mock<IGaeDataSource> _mockedGaeDataSource;
        private AppEngineManagementViewModel _testedViewModel;

        [TestInitialize]
        public void Initialize()
        {
            _flexLocationsSource = new TaskCompletionSource<IList<string>>();
            _mockedGaeDataSource = new Mock<IGaeDataSource>();
            _mockedGaeDataSource.Setup(ds => ds.GetFlexLocationsAsync()).Returns(() => _flexLocationsSource.Task);

            _testedViewModel = new AppEngineManagementViewModel(TestProjectId, _mockedGaeDataSource.Object);
        }

        [TestMethod]
        public void InitialStateTests()
        {
            Assert.IsTrue(_testedViewModel.Locations.IsPending);
            Assert.AreEqual(TestProjectId, _testedViewModel.ProjectId);
            Assert.AreEqual(AppEngineManagementViewModel.s_loadingPlaceholder.First(), _testedViewModel.SelectedLocation);
            CollectionAssert.AreEqual(AppEngineManagementViewModel.s_loadingPlaceholder.ToList(), _testedViewModel.Locations.Value.ToList());
        }

        [TestMethod]
        public async Task LocationsSortedTest()
        {
            _flexLocationsSource.SetResult(s_mockFlexLocations);
            await _testedViewModel.Locations.ValueTask;

            CollectionAssert.AreEqual(s_sortedMockFlexLocations, _testedViewModel.Locations.Value.ToList());
        }

        [TestMethod]
        public async Task ResultTest()
        {
            _flexLocationsSource.SetResult(s_mockFlexLocations);
            await _testedViewModel.Locations.ValueTask;

            _testedViewModel.SelectedLocation = s_mockFlexLocations.First();

            Assert.IsTrue(_testedViewModel.ActionCommand.CanExecute(null));
            _testedViewModel.ActionCommand.Execute(null);

            Assert.AreEqual(s_mockFlexLocations.First(), _testedViewModel.Result);
        }
    }
}
