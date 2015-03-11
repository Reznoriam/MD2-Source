using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace MD2
{
    public class Crematorius : Droid
    {
        public bool stripBodies = true;
        private readonly Texture2D ShirtIcon = ContentFinder<Texture2D>.Get("UI/Commands/ShirtIcon");

        public override void Tick()
        {
            base.Tick();
            if (Find.TickManager.TicksGame % 100 == 0 && this.Active)
            {
                GenTemperature.PushHeat(this.Position, 5f);
            }
        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (base.GetGizmos() != null)
            {
                foreach (Command c in base.GetGizmos())
                {
                    yield return c;
                }
            }
            Command_Toggle a = new Command_Toggle();
            a.toggleAction = () =>
            {
                stripBodies = !stripBodies;
            };
            a.activateSound = SoundDefOf.Click;
            if (this.stripBodies)
            {
                a.defaultDesc = "Click to toggle off stripping bodies";
                a.defaultLabel = "Stripping bodies";
            }
            else
            {
                a.defaultDesc = "Click to toggle on stripping bodies";
                a.defaultLabel = "Not stripping bodies";
            }
            a.isActive = () => { return this.stripBodies; };
            a.hotKey = Keys.Named("CrematoriusToggleStripBodies");
            a.disabled = false;
            a.groupKey = 313740005;
            a.icon = this.ShirtIcon;
            yield return a;
        }

        public override string GetInspectString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(base.GetInspectString());
            if (this.stripBodies)
                s.AppendLine("Will strip bodies");
            else
                s.AppendLine("Will not strip bodies");
            return s.ToString();
        }
    }
}
