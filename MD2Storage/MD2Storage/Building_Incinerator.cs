using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class Building_Incinerator : Building_Storage
    {
        private List<IntVec3> cells;
        List<Thing> things;

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            cells = GenAdj.CellsOccupiedBy(this).ToList();
        }

        public override void Tick()
        {
            base.Tick();
            if (Find.TickManager.TicksGame % 50 == 0)
            {
                foreach (IntVec3 cell in cells)
                {
                    things = (
                        from t in Find.ThingGrid.ThingsAt(cell)
                        where slotGroup.Settings.AllowedToAccept(t)
                        select t).ToList();
                    foreach (Thing current in things)
                    {
                        current.Destroy();
                    }
                }
            }
        }

    }
}
