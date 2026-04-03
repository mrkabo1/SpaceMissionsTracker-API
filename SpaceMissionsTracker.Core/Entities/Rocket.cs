using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionsTracker.Core.Entities
{
    public class Rocket
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AgencyId { get; set; }
        public int FirstLaunch { get; set; }
        public Agency? Agency { get; set; }
    }
}
