﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class PlaceWorker_OnMudAndWater : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(EntityDef checkingDef, IntVec3 loc, IntRot rot)
        {
            TerrainDef mudDef = DefDatabase<TerrainDef>.GetNamed("Mud");
            TerrainDef waterDef = DefDatabase<TerrainDef>.GetNamed("WaterShallow");
            if (Find.TerrainGrid.TerrainAt(loc) == mudDef || Find.TerrainGrid.TerrainAt(loc) == waterDef)
            {
                return true;
            }
            return "OnMudOrWaterReportString".Translate();
        }
    }
}
