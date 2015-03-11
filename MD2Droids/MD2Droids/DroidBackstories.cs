using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace MD2
{
    public static class DroidBackstories
    {
        public static Backstory GetBackstoryFor(string backstoryName)
        {
            switch (backstoryName)
            {
                case "LogisticsDroid":
                    return DroidBackstories.LogisticsDroid;
                case "MiningDroid":
                    return DroidBackstories.MiningDroid;
                case "GrowerDroid":
                    return DroidBackstories.GrowerDroid;
                case "MedicDroid":
                    return DroidBackstories.MedicDroid;
                case "BuilderDroid":
                    return DroidBackstories.BuilderDroid;
                case "CraftingDroid":
                    return DroidBackstories.CraftingDroid;
                case "CookingDroid":
                    return DroidBackstories.CookingDroid;
                case "BasicCombatDroid":
                    return DroidBackstories.BasicCombatDroid;
                case "CrematoriusDroid":
                    return DroidBackstories.CrematoriusDroid;
                default:
                    return DroidBackstories.DefaultDroid;
            }
        }

        public static Backstory DefaultDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "DefaultDroid";
                b.title = "";
                b.titleShort = "";
                b.workDisables =
                    WorkTags.None;
                return b;
            }
        }

        public static Backstory LogisticsDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "LogisticsDroid";
                b.title = "Logistics Droid";
                b.titleShort = "Logistics Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.Cleaning, WorkTags.Hauling, WorkTags.ManualDumb, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory MiningDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "MiningDroid";
                b.title = "Mining Droid";
                b.titleShort = "Mining Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.ManualSkilled, WorkTags.Mining, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory GrowerDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "GrowerDroid";
                b.title = "Grower Droid";
                b.titleShort = "Grower Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.ManualSkilled, WorkTags.PlantWork, WorkTags.ManualDumb, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory MedicDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "MedicDroid";
                b.title = "Medic Droid";
                b.titleShort = "Medic Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.Caring, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory BuilderDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "BuilderDroid";
                b.title = "Builder Droid";
                b.titleShort = "Builder Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.ManualSkilled, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory CraftingDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "CraftingDroid";
                b.title = "Crafting Droid";
                b.titleShort = "Crafting Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.ManualSkilled, WorkTags.Crafting, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory CookingDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "CookingDroid";
                b.title = "Cooking Droid";
                b.titleShort = "Cooking Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.ManualSkilled, WorkTags.Cooking, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory BasicCombatDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "CombatDroid";
                b.title = "Combat Droid";
                b.titleShort = "Combat Droid";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.Violent, WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }

        public static Backstory CrematoriusDroid
        {
            get
            {
                Backstory b = new Backstory();
                b.baseDesc = "";
                b.bodyTypeGlobal = BodyType.Thin;
                b.uniqueSaveKey = "CrematoriusDroid";
                b.title = "Crematorius";
                b.titleShort = "Crematorius";
                List<WorkTags> list = new List<WorkTags>() { WorkTags.Firefighting, WorkTags.Scary };
                foreach (WorkTags current in Enum.GetValues(typeof(WorkTags)))
                {
                    if (!list.Contains(current))
                    {
                        b.workDisables |= current;
                    }
                }
                return b;
            }
        }
    }
}
