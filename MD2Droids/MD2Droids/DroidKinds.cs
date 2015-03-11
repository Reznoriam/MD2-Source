using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public static class DroidKinds
    {
        public static DroidKindDef GetNamed(string defName)
        {
            switch (defName)
            {
                case "LogisticsDroid":
                    return DroidKinds.LogisticsDroid;
                case "MiningDroid":
                    return DroidKinds.MiningDroid;
                case "GrowerDroid":
                    return DroidKinds.GrowerDroid;
                case "MedicDroid":
                    return DroidKinds.MedicDroid;
                case "BuilderDroid":
                    return DroidKinds.BuilderDroid;
                case "CraftingDroid":
                    return DroidKinds.CraftingDroid;
                case "CookingDroid":
                    return DroidKinds.CookingDroid;
                case "BasicCombatDroid":
                    return DroidKinds.BasicCombatDroid;
                case "CrematoriusDroid":
                    return DroidKinds.CrematoriusDroid;
                default:
                    return null;
            }
        }
        public static DroidKindDef LogisticsDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2LogisticsDroid");
            }
        }

        public static DroidKindDef MiningDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2MiningDroid");
            }
        }

        public static DroidKindDef GrowerDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2GrowerDroid");
            }
        }

        public static DroidKindDef MedicDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2MedicDroid");
            }
        }

        public static DroidKindDef BuilderDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2BuilderDroid");
            }
        }

        public static DroidKindDef CraftingDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2CraftingDroid");
            }
        }

        public static DroidKindDef CookingDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2CookingDroid");
            }
        }

        public static DroidKindDef BasicCombatDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2BasicCombatDroid");
            }
        }

        public static DroidKindDef CrematoriusDroid
        {
            get
            {
                return (DroidKindDef)DefDatabase<PawnKindDef>.GetNamed("MD2CrematoriusDroid");
            }
        }
    }
}
