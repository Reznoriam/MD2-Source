using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace MD2
{
    public class ManufacturingPlantDialog : Layer
    {
        protected AssemblyLine parent;
        public ManufacturingPlantDialog(string helpMessage, AssemblyLine parent, string title)
            : this(600, 700, helpMessage, parent, title)
        {
        }

        public ManufacturingPlantDialog(float width, float height, string helpMessage, AssemblyLine parent, string title = "")
        {
            base.SetCentered(width, height);
            this.category = LayerCategory.GameDialog;
            this.absorbAllInput = true;
            this.forcePause = true;
            this.clearNonEditDialogs = false;
            this.closeOnEscapeKey = true;
            this.doCloseX = true;
            this.message = helpMessage;
            this.title = title;
            this.parent = parent;
        }

        private void Setup(string helpMessage)
        {
            base.SetCentered(600f, 700f);
            this.category = LayerCategory.GameDialog;
            this.absorbAllInput = true;
            this.forcePause = true;
            this.clearNonEditDialogs = false;
            this.closeOnEscapeKey = true;
            this.doCloseX = true;
            this.message = helpMessage;
        }

        protected string title = "";
        protected string message = "";
        protected float currentY = 0f;
        protected override void FillWindow(Rect inRect)
        {
            if (!title.NullOrEmpty())
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Text.Font = GameFont.Medium;
                Rect labelRect = new Rect(0f, 0f, inRect.width, 40f);
                Widgets.Label(labelRect, title);
                currentY += 45f;
                Widgets.DrawLineHorizontal(0f, currentY, inRect.width);
                Text.Anchor = TextAnchor.UpperLeft;
                Text.Font = GameFont.Small;
            }
            if (!message.NullOrEmpty())
            {
                Rect helpRect = new Rect(0, 0, 20, 20);
                if (Widgets.TextButton(helpRect, "?"))
                {
                    Find.LayerStack.Add(new Dialog_Message(message, "Help"));
                }
            }
        }
        protected virtual void ResetVariables()
        {
            currentY = 0f;
        }
    }
}
