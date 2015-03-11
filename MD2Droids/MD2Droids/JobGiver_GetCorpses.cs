
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace MD2
{
    public class JobGiver_GetCorpses : ThinkNode_JobGiver
    {
        protected override Job TryGiveTerminalJob(Pawn pawn)
        {
            if (pawn.GetType() != typeof(Crematorius))
            {
                return null;
            }
            Crematorius droid = (Crematorius)pawn;
            if (droid.Active)
            {
                Corpse corpse = (Corpse)CorpseFinderUtility.FindClosestCorpse(droid);
                if (corpse == null)
                    return null;
                return new Job(DroidJobDefs.CremateJob, corpse);
            }
            return null;
        }
    }
}
