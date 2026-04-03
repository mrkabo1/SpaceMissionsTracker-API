using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.Entities
{
    public class Astronaut
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public int BirthYear { get; set; }
        public ICollection<MissionAstronaut> MissionAstronauts { get; set; } = new List<MissionAstronaut>();
    }
}
