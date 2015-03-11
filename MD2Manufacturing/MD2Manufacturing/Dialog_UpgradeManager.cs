using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD2
{
    public class Dialog_UpgradeManager : ManufacturingPlantDialog
    {
        public Dialog_UpgradeManager(string title)
            : base(title, "UpgradeManagerHelp".Translate())
        {
        }

        protected override void FillWindow(Rect inRect)
        {
            base.FillWindow(inRect);


            base.ResetVariables();
        }
    }
}
