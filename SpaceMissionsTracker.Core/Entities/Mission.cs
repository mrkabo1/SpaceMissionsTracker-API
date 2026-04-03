using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.Entities
{
    public class Mission
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RocketId { get; set; }
        public DateTime LaunchDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;

        public Rocket? Rocket { get; set; }
        public ICollection<MissionAstronaut> MissionAstronauts { get; set; } = new List<MissionAstronaut>();
    }
}
