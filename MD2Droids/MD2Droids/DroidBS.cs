using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace MD2
{
    internal static class DroidBS
    {
        public static PawnName DroidName(string name)
        {
            return new PawnName()
            {
                first = "Droid",
                last = "Droid",
                nick = name
            };
        }


        public static void AddBs(Backstory bs)
        {
            if (!BackstoryDatabase.allBackstories.ContainsKey(bs.uniqueSaveKey) && bs != null)
                BackstoryDatabase.AddBackstory(bs);
        }

        public static void AddDroidBs(string bsName)
        {
            if (!BackstoryDatabase.allBackstories.ContainsKey(bsName))
                AddBs(DroidBackstories.GetBackstoryFor(bsName));
        }

        public static void RemoveBs(Backstory bs)
        {
            if (bs != null)
                BackstoryDatabase.allBackstories.Remove(bs.uniqueSaveKey);
        }

        public static void RemoveDroidBs(string bsName)
        {
            RemoveBs(DroidBackstories.GetBackstoryFor(bsName));
        }

    }
}
