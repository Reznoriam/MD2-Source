using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace MD2
{
    class DroidChargePad : Building
    {
        public DroidChargerComp Charger
        {
            get
            {
                return this.GetComp<DroidChargerComp>();
            }
        }
        public bool IsAvailable(Pawn p)
        {
            CompPowerTrader power = this.GetComp<CompPowerTrader>();
            return power != null && power.PowerOn && Charger != null && Charger.CanUse(p);
        }
    }
}
