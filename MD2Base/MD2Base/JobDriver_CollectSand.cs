using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace MD2
{
    public class JobDriver_CollectSand : JobDriver
    {
        private const TargetIndex CellInd = TargetIndex.A;

        public JobDriver_CollectSand(Pawn pawn):base(pawn)
        {

        }

        //public TargetInfo CellInd
        //{
        //    get
        //    {
        //        return base.GetActor().jobs.curJob.GetTarget(TargetIndex.A);
        //    }
        //}

        public override string GetReport()
        {
            return pawn.jobs.curJob.reportString;
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnBurningImmobile(CellInd);

            yield return Toils_Reserve.Reserve(CellInd, ReservationType.Total);

            yield return Toils_Goto.GotoCell(CellInd, PathMode.OnCell);

            yield return Toils_General.Wait(500);

            yield return Toils_MD2General.MakeAndSpawnThing(ThingDef.Named("MD2SandPile"), 100);

            yield return Toils_MD2General.RemoveDesignationAtPosition(GetActor().jobs.curJob.GetTarget(CellInd).Cell, DefDatabase<DesignationDef>.GetNamed("MD2CollectSand"));
        }
    }
}
