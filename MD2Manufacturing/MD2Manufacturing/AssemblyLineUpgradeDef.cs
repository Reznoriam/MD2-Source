using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class AssemblyLineUpgradeDef : Def
    {
        public float speedMult;
        public float efficiencyMult;
        public string Name;
        public string Description;
        public Action specialAction;
    }
}
