using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD2
{
    public abstract class ManufacturingPlantPage : Layer
    {
        protected string message = "";
        public ManufacturingPlantPage(string message)
        {
            this.message = message;
        }

        public ManufacturingPlantPage()
        {

        }

        protected override void FillWindow(Rect inRect)
        {
            if (!message.NullOrEmpty())
            {
                Rect helpRect = new Rect(0, 0, 20, 20);
                if (Widgets.TextButton(helpRect, "?"))
                {
                    Find.LayerStack.Add(new Dialog_Message(message, "Help"));
                }
            }
        }
    }
}
