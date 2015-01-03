﻿using System;
using NUnit.Framework;
using Should;
using ZBuildLights.Core.Models;

namespace UnitTests.ZBuildLights.Core.Models
{
    public class MasterModelTests
    {
        [TestFixture]
        public class When_model_has_multiple_projects_with_multiple_groups_with_multiple_lights
        {
            [Test]
            public void Should_find_a_light_by_homeId_and_deviceId()
            {
                var model = new MasterModel();
                
                var project1 = model.AddProject(new Project {Name = "1"});
                
                var group1_1 = project1.AddGroup(new LightGroup {Name = "1.1"});
                var light1_1_1 = group1_1.AddLight(new Light(11, 1));
                var light1_1_2 = group1_1.AddLight(new Light(11, 2));
                var light1_1_3 = group1_1.AddLight(new Light(11, 3));
                
                var group1_2 = project1.AddGroup(new LightGroup {Name = "1.2"});
                var light1_2_1 = group1_2.AddLight(new Light(12, 1));
                var light1_2_2 = group1_2.AddLight(new Light(12, 2));
                var light1_2_3 = group1_2.AddLight(new Light(12, 3));
                
                var group1_3 = project1.AddGroup(new LightGroup {Name = "1.3"});
                var light1_3_1 = group1_3.AddLight(new Light(13, 1));
                var light1_3_2 = group1_3.AddLight(new Light(13, 2));
                var light1_3_3 = group1_3.AddLight(new Light(13, 3));

                var project2 = model.AddProject(new Project {Name = "1"});
                
                var group2_1 = project2.AddGroup(new LightGroup {Name = "2.1"});
                var light2_1_1 = group2_1.AddLight(new Light(21, 1));
                var light2_1_2 = group2_1.AddLight(new Light(21, 2));
                var light2_1_3 = group2_1.AddLight(new Light(21, 3));
                
                var group2_2 = project2.AddGroup(new LightGroup {Name = "2.2"});
                var light2_2_1 = group2_2.AddLight(new Light(22, 1));
                var light2_2_2 = group2_2.AddLight(new Light(22, 2));
                var light2_2_3 = group2_2.AddLight(new Light(22, 3));
                
                var group2_3 = project2.AddGroup(new LightGroup {Name = "2.3"});
                var light2_3_1 = group2_3.AddLight(new Light(23, 1));
                var light2_3_2 = group2_3.AddLight(new Light(23, 2));
                var light2_3_3 = group2_3.AddLight(new Light(23, 3));

                uint homeId = 22;
                byte deviceId = 2;

                var found = model.FindLight(homeId, deviceId);
                found.ZWaveHomeId.ShouldEqual(homeId);
                found.ZWaveDeviceId.ShouldEqual(deviceId);
                found.ParentGroup.ShouldBeSameAs(group2_2);
                found.ParentGroup.ParentProject.ShouldBeSameAs(project2);
            }
        }

        [TestFixture]
        public class When_model_has_multiple_projects_with_multiple_groups
        {
            [Test]
            public void Should_find_a_group_by_Id()
            {
                var model = new MasterModel();
                
                var project1 = model.AddProject(new Project {Name = "1"});
                var group1_1 = project1.AddGroup(new LightGroup {Name = "1.1", Id = Guid.NewGuid()});
                var group1_2 = project1.AddGroup(new LightGroup {Name = "1.2", Id = Guid.NewGuid()});
                var group1_3 = project1.AddGroup(new LightGroup {Name = "1.3", Id = Guid.NewGuid()});

                var project2 = model.AddProject(new Project {Name = "1"});
                var group2_1 = project2.AddGroup(new LightGroup {Name = "2.1", Id = Guid.NewGuid()});
                var group2_2 = project2.AddGroup(new LightGroup {Name = "2.2", Id = Guid.NewGuid()});
                var group2_3 = project2.AddGroup(new LightGroup {Name = "2.3", Id = Guid.NewGuid()});

                var found = model.FindGroup(group2_2.Id);
            }
        }

        [TestFixture]
        public class When_model_is_empty
        {
            [Test]
            public void Should_throw_an_exception_when_searching_for_a_light()
            {
                var model = new MasterModel();

                Exception thrown = null;

                try
                {
                    model.FindLight(1, 2);
                }
                catch (Exception e)
                {
                    thrown = e;
                }

                thrown.GetType().ShouldEqual(typeof(InvalidOperationException));
                thrown.Message.ShouldEqual("Could not find light with homeId: 1 and deviceId: 2");
            }

            [Test]
            public void Should_throw_an_exception_when_searching_for_a_group()
            {
                var model = new MasterModel();

                Exception thrown = null;

                var id = Guid.NewGuid();
                try
                {
                    model.FindGroup(id);
                }
                catch (Exception e)
                {
                    thrown = e;
                }

                thrown.GetType().ShouldEqual(typeof(InvalidOperationException));
                thrown.Message.ShouldEqual(string.Format("Could not find group with id: {0}", id));
            }
        }
    }
}