using System;
using System.Collections.Generic;

namespace Common
{
    public class Fixture
    {
        public int FixtureId { get; set; }

        public Team HomeTeam { get; set; }
        public List<Team> AwayTeam { get; set; }

        public string Venue { get; set; }
        public DateTime StartTime { get; set; }

        public bool IsSettled { get; set; }
    }

    public class Team
    {
        private string teamName;

        public string TeamName
        {
            get { return teamName; }
            set { teamName = value; }
        }
    }
}
