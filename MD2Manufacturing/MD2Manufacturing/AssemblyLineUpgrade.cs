using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class AssemblyLineUpgrade : Saveable
    {
        private AssemblyLineUpgradeDef def;
        private int workRemaining = 0;

        public AssemblyLineUpgrade(AssemblyLineUpgradeDef def)
        {
            this.def = def;
            this.workRemaining = def.workTicksAmount;
        }

        public bool Tick()
        {
            workRemaining--;
            if(workRemaining<=0)
                return true;
            return false;
        }

        public AssemblyLineUpgradeDef Def
        {
            get
            {
                return this.def;
            }
        }

        public void ExposeData()
        {
            Scribe_Defs.LookDef(ref this.def, "def");
            Scribe_Values.LookValue(ref this.workRemaining, "workRemaining");
        }
    }
}
