using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace MD2
{
    public class CoalFeeder : Building_Storage
    {
        public Thing ContainedFuel
        {
            get
            {
                ThingDef def = ThingDef.Named("MD2Coal");
                return Find.ThingGrid.ThingAt(this.Position, def);
            }
        }

        public bool HasFuelItem
        {
            get
            {
                return ContainedFuel != null;
            }
        }
    }
}
