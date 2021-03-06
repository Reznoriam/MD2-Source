﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class PlaceWorker_MaximumFive : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(EntityDef checkingDef, IntVec3 loc, IntRot rot)
        {
            ThingDef def = checkingDef as ThingDef;
            if(def!=null)
            {
                int num = Find.ListerBuildings.allBuildingsColonist.Where((Building b) => b.def == def).Count();
                num+=Find.ListerThings.ThingsOfDef(def.blueprintDef).Count;
                if(num<5)
                {
                    return true;
                }
                return "MaximumFiveReportString".Translate();
            }
            return "Def not found";
        }
    }
}
