using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using RimWorld;

namespace MD2
{
    public class BillOfMaterials : Saveable
    {
        public BillOfMaterials(AssemblyLine line)
        {
            //Log.Error("Did this");
            this.line = line;
            foreach (ListItem item in AssemblyLine.Settings.BuildingCost)
            {
                acquiredMats.Add(item.thing, 0);
            }
        }

        private AssemblyLine line;
        private Dictionary<ThingDef, int> acquiredMats = new Dictionary<ThingDef, int>();
        private bool hasMats = false;

        public virtual void Tick()
        {
            if (Find.TickManager.TicksGame % 120 == 0)
                TryTakeNeededItems();
            CheckIfHaveAllMaterials();
        }

        public static List<ListItem> MaterialsRequired
        {
            get
            {
                if (Game.godMode && AssemblyLine.Settings.instaBuild)
                {
                    return new List<ListItem>();
                }
                return AssemblyLine.Settings.BuildingCost;
            }
        }

        private void CheckIfHaveAllMaterials()
        {
            if (Game.godMode && AssemblyLine.Settings.instaBuild)
            {
                hasMats = true;
            }
            else
            {
                foreach (var item in MaterialsRequired)
                {
                    int value = 0;
                    if (acquiredMats.TryGetValue(item.thing, out value))
                    {
                        if (value >= item.amount)
                        {
                            continue;
                        }
                        else
                        {
                            hasMats = false;
                            return;
                        }
                    }
                }
            }
            hasMats = true;
        }

        private void TryTakeNeededItems()
        {
            if (Game.godMode && AssemblyLine.Settings.instaBuild)
            {
                return;
            }
            foreach (var item in AssemblyLine.Settings.BuildingCost)
            {
                IEnumerable<Thing> things = (
                    from t in RimWorld.TradeUtility.AllLaunchableThings
                    where t.def == item.thing
                    select t).AsEnumerable();

                int remaining = 0;
                int value = 0;
                if (acquiredMats.TryGetValue(item.thing, out value))
                {
                    remaining = item.amount - value;
                }
                else
                {
                    acquiredMats.Add(item.thing, 0);
                    remaining = item.amount;
                }
                if (remaining > 0)
                {
                    foreach (var thing in things)
                    {
                        int num = UnityEngine.Mathf.Min(remaining, thing.stackCount);
                        thing.TakeFrom(num);
                        remaining -= num;
                        acquiredMats[item.thing] += num;
                        if (remaining <= 0)
                            break;
                    }
                }
            }
        }

        public int this[ThingDef key]
        {
            get
            {
                int i = 0;
                acquiredMats.TryGetValue(key, out i);
                return i;
            }
        }

        public bool HasMats
        {
            get
            {
                return this.hasMats;
            }
        }

        public void DropAcquiredMats()
        {
            List<Thing> list = new List<Thing>();
            foreach (ThingDef def in acquiredMats.Keys)
            {
                int remaining = 0;
                if (acquiredMats.TryGetValue(def, out remaining))
                {
                    remaining = (int)(remaining * 0.75);
                    while (remaining > 0)
                    {
                        int num = UnityEngine.Mathf.Min(def.stackLimit, remaining);
                        Thing thing = ThingMaker.MakeThing(def);
                        thing.stackCount = num;
                        remaining -= num;
                        list.Add(thing);
                    }
                }
            }

            foreach (var thing in list)
            {
                IntVec3 loc = RCellFinder.TradeDropSpot();
                DropPodUtility.MakeDropPodAt(loc, new DropPodInfo
                    {
                        SingleContainedThing = thing,
                        leaveSlag = false
                    });
            }
        }

        private bool SearchForItem(ListItem item)
        {
            return SearchForItem(item.thing, item.amount);
        }

        private bool SearchForItem(ThingDef def, int amount)
        {
            IEnumerable<Thing> things = (
                from t in RimWorld.TradeUtility.AllLaunchableThings
                where t.def == def
                select t).AsEnumerable();

            if (things.Count() > 0)
            {
                int count = 0;
                foreach (var thing in things)
                {
                    count += thing.stackCount;
                }
                if (count >= amount)
                    return true;
            }
            return false;
        }

        public void ExposeData()
        {
            Scribe_Collections.LookDictionary<ThingDef, int>(ref this.acquiredMats, "acquiredMats", LookMode.DefReference, LookMode.Value);
            Scribe_Values.LookValue(ref this.hasMats, "hasMats");
        }
    }
}
