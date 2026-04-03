using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.Entities
{
    public class MissionAstronaut
    {

        public int MissionId { get; set; }
        public int AstronautId { get; set; }

        public Mission Mission { get; set; } = null!;
        public Astronaut Astronaut { get; set; } = null!;
    }
}
