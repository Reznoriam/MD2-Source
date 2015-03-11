using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace MD2
{
    public class Extractor : Building
    {
        private static readonly Texture2D chooseIcon = ContentFinder<Texture2D>.Get("UI/Commands/ChooseResourceUI");
        public CompPowerTrader power;
        private List<MinerResourceDef> availableList;
        private int resourceIndex = 0;

        public override void SpawnSetup()
        {
            base.SpawnSetup();
            this.power = base.GetComp<CompPowerTrader>();
            if (HasFissure)
            {
                FirstSetup();
            }
        }

        private void FirstSetup()
        {
            switch (CurFissureSize)
            {
                case FissureSize.Small:
                    {
                        availableList = (
                            from t in this.ResourceList()
                            where t.fissureSizeRequired == FissureSize.Small
                            select t).ToList();
                        break;
                    }
                case FissureSize.Medium:
                    {
                        availableList = (
                            from t in this.ResourceList()
                            where t.fissureSizeRequired == FissureSize.Small || t.fissureSizeRequired == FissureSize.Medium
                            select t).ToList();
                        break;
                    }
                case FissureSize.Large:
                    {
                        availableList = (
                            from t in this.ResourceList()
                            where t.fissureSizeRequired == FissureSize.Small || t.fissureSizeRequired == FissureSize.Medium || t.fissureSizeRequired == FissureSize.Large
                            select t).ToList();
                        break;
                    }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.LookValue<int>(ref this.resourceIndex, "count");
        }

        public override void Tick()
        {
            base.Tick();
            if (!this.HasPower || !this.HasFissure)
            {
                return;
            }
            if (Find.TickManager.TicksGame % 200 == 0)
            {
                GenTemperature.PushHeat(this, 1f);
            }
            if (Rand.Value > 0.3f)
            {
                MoteThrower.ThrowDustPuff(this.TrueCenter(), 0.3f);
            }
            int tick = availableList[resourceIndex].ticksToProduce;
            if (Game.godMode)
                tick = 300;
            if (Find.TickManager.TicksGame % tick == 0)
            {
                Dig();
                //int num = Rand.Range(1, 100);
                //if (num < 2)
                //{
                //    string ms = "While digging for ores, your extractor has discovered a ruby!";
                //    Messages.Message(ms, MessageSound.Benefit);
                //    SpawnRadial.Spawn(MD2Defs.Ruby, 1, this.Position);
                //}
            }
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (base.GetGizmos() != null)
            {
                foreach(var c in base.GetGizmos())
                {
                    yield return c;
                }
            }
            Command_Action com = new Command_Action()
{
    action = () =>
        {
            CycleThroughAvailbleResources();
            //Log.Message(curSelResource.ToString());
        },
    activateSound = SoundDefOf.Click,
    defaultDesc = "Click to change which resource to produce",
    defaultLabel = "Change resource",
    hotKey = Keys.Named("ExtractorChangeResource"),
    icon = chooseIcon,
    groupKey = 313740004
};
            if (HasFissure)
            {
                com.disabled = false;
            }
            else
            {
                com.disabled = true;
            }
            yield return com;
        }

        private void CycleThroughAvailbleResources()
        {
            if (availableList.Count == 1)
            {
                return;
            }
            else
            {
                resourceIndex++;
                if (resourceIndex >= availableList.Count)
                {
                    resourceIndex = 0;
                }
            }
        }

        public override string GetInspectString()
        {

            StringBuilder str = new StringBuilder();
            str.AppendLine(base.GetInspectString());
            str.AppendLine(String.Format("Fissure size: {0}", CurFissureSize.ToString()));
            str.AppendLine(String.Format("Selected resource: {0}", availableList[resourceIndex].label));
            return str.ToString();
        }

        public IntVec3 SpawnPos
        {
            get
            {
                int count = 0;
                while (count < 30)
                {
                    IntVec3 pos = this.Position + GenRadial.RadialPattern[count];
                    if (!pos.Standable() || !pos.InBounds())
                        count++;
                    else
                        return pos;
                }
                return this.Position;
            }
        }

        public void Dig()
        {
            if (HasPower && HasFissure)
            {
                //Get the number to spawn
                int num = (int)(Rand.Range(availableList[resourceIndex].spawnRangeMin, availableList[resourceIndex].spawnRangeMax) * SizeMultiplier);
                //Set the spawn position to the interaction cell
                IntVec3 pos = this.InteractionCell;
                //Try to find the def for the conveyor belt loader
                ThingDef loaderDef = DefDatabase<ThingDef>.GetNamed("A2BLoader", false);
                //If the def exists, then search for any that are connected to the extractor
                if (loaderDef != null)
                {
                    //Look at each cell around the extractor
                    foreach (IntVec3 current in GenAdj.CellsAdjacentCardinal(this))
                    {
                        //Search for a loader in that cell
                        Thing thing3 = Find.ThingGrid.ThingAt(current, loaderDef);
                        //If one is found, set the spawn position to the loader position and exit the loop
                        if (thing3 != null)
                        {
                            pos = thing3.Position;
                            break;
                        }
                    }
                }

                //Look for some of the resource at the spawn position
                Thing thing2 = Find.ThingGrid.ThingAt(pos, availableList[resourceIndex].resourceDefName);
                //If some is found, the resource to be spawned will be placed in this stack
                if (thing2 != null)
                    thing2.stackCount += num;
                else
                {
                    //If there is no resource already there, then make a new stack at the position
                    Thing thing = ThingMaker.MakeThing(availableList[resourceIndex].resourceDefName);
                    thing.stackCount = num;
                    //Spawn the resource
                    GenSpawn.Spawn(thing, pos);
                }

            }
        }

        public float SizeMultiplier
        {
            get
            {
                switch (CurFissureSize)
                {
                    case FissureSize.Small:
                        return 1f;
                    case FissureSize.Medium:
                        return 1.1f;
                    case FissureSize.Large:
                        return 1.2f;
                    default:
                        return 1f;
                }
            }
        }

        public IEnumerable<MinerResourceDef> ResourceList()
        {
            return DefDatabase<MinerResourceDef>.AllDefs;
        }


        public bool HasPower
        {
            get
            {
                return this.power != null && power.PowerOn;
            }
        }
        public FissureSize CurFissureSize
        {
            get
            {
                FissureClass thing = (FissureClass)Find.ThingGrid.ThingAt(this.Position, ThingDef.Named("mipFissure"));
                if (thing != null)
                    return thing.size;
                else
                    return FissureSize.None;
            }
        }

        public bool HasFissure
        {
            get
            {
                return this.CurFissureSize != FissureSize.None;
            }
        }
    }
}
