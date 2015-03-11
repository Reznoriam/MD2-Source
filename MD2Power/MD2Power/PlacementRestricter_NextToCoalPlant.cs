using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class PlacementRestricter_NextToCoalPlant : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(EntityDef checkingDef, IntVec3 loc, IntRot rot)
        {
            ThingDef def = ThingDef.Named("MD2CoalBurner");
            foreach (IntVec3 pos in GenAdj.CellsAdjacentCardinal(loc,IntRot.north,IntVec2.one))
            {
                foreach (Thing thing in Find.ThingGrid.ThingsAt(pos))
                {
                    if (thing.def == def)
                    {
                        return true;
                    }
                }
            }
            return "Must be placed next to a coal burner";
        }
    }
}
