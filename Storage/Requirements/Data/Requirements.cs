using System;
using System.Collections.Generic;
using Storage.Requirements.Model.Entity;

namespace Storage.Requirements.Data
{
    public class Requirements
    {
        public ICollection<Requirement> GetRequirements()
        {
            // TODO: Remove this stub
            Project project = new Project { Name = "Lorem" };
            RequirementSource source = new RequirementSource { Name = "Hardcoded" };
            TmsTask tmsTask = new TmsTask { Number = "HRDC-0001" };

            return new List<Requirement>()
            {
                new Requirement()
                {
                    Number = "FR1",
                    Ccp = 5,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ObjectNumber = "1.0.0.1",
                    Status = "New",
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec.",
                    Project = project,
                    Source = source,
                    TmsTask = tmsTask
                },
                new Requirement()
                {
                    Number = "FR2",
                    Ccp = 1,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    ObjectNumber = "1.0.0.2",
                    Status = "New",
                    Text = "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                    Project = project,
                    Source = source,
                    TmsTask = tmsTask
                }
            };
        }
    }
}
