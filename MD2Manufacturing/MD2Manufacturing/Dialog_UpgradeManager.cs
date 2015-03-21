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
        Vector2 scrollPosition = default(Vector2);
        public Dialog_UpgradeManager(AssemblyLine parent, string title)
            : base("UpgradeManagerHelp".Translate(), parent, title)
        {
            this.doCloseButton = true;
        }

        protected override void FillWindow(Rect inRect)
        {
            base.FillWindow(inRect);

            Rect rect = new Rect(0, base.currentY, inRect.width, (inRect.height - base.currentY) / 2);
            base.currentY += rect.yMax;
            parent.UpgradeManager.OnGUI(rect, scrollPosition);

            base.ResetVariables();
        }
    }
}
