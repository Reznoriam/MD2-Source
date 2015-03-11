using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;


namespace MD2
{
    public class DroidKindDef : PawnKindDef
    {
        public List<WorkTypeDef> allowedWorkTypeDefs;
        public string backstoryName;
        public string headGraphicPath;
        public float maxEnergy;
        public SettingsDef Settings;
    }
}
