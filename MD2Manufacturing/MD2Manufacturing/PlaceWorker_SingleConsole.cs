using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class PlaceWorker_SingleConsole : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(EntityDef checkingDef, IntVec3 loc, IntRot rot)
        {
            IEnumerable<ManufacturingControlConsole> console = Find.ListerBuildings.AllBuildingsColonistOfClass<ManufacturingControlConsole>();
            if(console.Count()==0)
            {
                return AcceptanceReport.WasAccepted;
            }
            return "Only one of this building may be built at any one time";
        }
    }
}
