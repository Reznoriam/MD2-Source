using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace MD2
{
    public class JobGiver_ForDroids : JobGiver_WorkRoot
    {
        private List<WorkGiver> workGivers = new List<WorkGiver>();
        public IEnumerable<WorkGiver> AllWorkGivers
        {
            get
            {
                return this.workGivers;
            }
        }
        public override void ResolveReferences()
        {
            base.ResolveReferences();
            foreach (WorkGiverDef current in
                from d in DefDatabase<WorkGiverDef>.AllDefs
                orderby d.priorityInType descending
                select d)
            {
                WorkGiver item = (WorkGiver)Activator.CreateInstance(current.giverClass, new WorkGiverDef[]
				{
					current
				});
                this.workGivers.Add(item);
            }
        }

        private DroidKindDef droidKindDef(Pawn pawn)
        {
            return (DroidKindDef)pawn.kindDef;
        }

        protected override Job TryGiveTerminalJob(Pawn pawn)
        {
            if (!(pawn is Droid)) return null;

            List<WorkTypeDef> list = new List<WorkTypeDef>();
            if(Find.PlaySettings.useWorkPriorities)
            { 
                list = droidKindDef(pawn).allowedWorkTypeDefs.Where((WorkTypeDef def)=>pawn.workSettings.WorkIsActive(def)).ToList();
                list = list.OrderBy(a => pawn.workSettings.GetPriority(a)).ThenByDescending(b => b.naturalPriority).ToList();
            }
            else
            {
                list = (
                    from t in droidKindDef(pawn).allowedWorkTypeDefs.Where((WorkTypeDef def)=>pawn.workSettings.WorkIsActive(def))
                    orderby t.naturalPriority descending
                    select t).ToList();
            }
            foreach (WorkTypeDef current in list)
            {
                if (current.emergency == this.onlyEmergencyWork || current.mixedEmergency)
                {
                    int num = -999;
                    TargetInfo targetInfo = TargetInfo.Invalid;
                    WorkGiver workGiver = null;
                    for (int i = 0; i < this.workGivers.Count; i++)
                    {
                        WorkGiver giver = this.workGivers[i];
                        if (giver.def.workType == current)
                        {
                            if (giver.def.priorityInType != num && targetInfo.Valid)
                            {
                                break;
                            }
                            bool flag = current.emergency;
                            if (giver.def.forceNoEmergency)
                            {
                                flag = false;
                            }
                            if (flag == this.onlyEmergencyWork)
                            {
                                if (giver.MissingRequiredActivity(pawn) == null && !giver.ShouldSkip(pawn))
                                {
                                    try
                                    {
                                        if (giver.def.scanThings)
                                        {
                                            Predicate<Thing> predicate = (Thing t) => !t.IsForbidden(pawn.Faction) && giver.HasJobOnThing(pawn, t);
                                            Predicate<Thing> validator = predicate;
                                            Thing thing = GenClosest.ClosestThingReachable(pawn.Position, giver.PotentialWorkThingRequest, giver.PathMode, TraverseParms.For(pawn, Danger.Deadly, true), 9999f, validator, giver.PotentialWorkThingsGlobal(pawn), -1);
                                            if (thing != null)
                                            {
                                                targetInfo = thing;
                                                workGiver = giver;
                                            }
                                        }
                                        if (giver.def.scanCells)
                                        {
                                            IntVec3 position = pawn.Position;
                                            float num2 = 99999f;
                                            foreach (IntVec3 current2 in giver.PotentialWorkCellsGlobal(pawn))
                                            {
                                                float lengthHorizontalSquared = (current2 - position).LengthHorizontalSquared;
                                                if (lengthHorizontalSquared < num2 && giver.HasJobOnCell(pawn, current2))
                                                {
                                                    targetInfo = current2;
                                                    workGiver = giver;
                                                    num2 = lengthHorizontalSquared;
                                                }
                                            }
                                        }
                                        num = giver.def.priorityInType;
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Error(string.Concat(new object[]
										{
											pawn,
											" threw exception in WorkGiver ",
											giver.def.defName,
											": ",
											ex.ToString()
										}));
                                    }
                                    finally
                                    {
                                    }
                                }
                            }
                        }
                    }
                    if (targetInfo.Valid)
                    {
                        pawn.mindState.lastGivenWorkType = current;
                        Job result;
                        if (targetInfo.HasThing)
                        {
                            result = workGiver.JobOnThing(pawn, targetInfo.Thing);
                            return result;
                        }
                        result = workGiver.JobOnCell(pawn, targetInfo.Cell);
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
