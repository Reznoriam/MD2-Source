using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace MD2
{
    public class ModController : MonoBehaviour
    {
        public static readonly string GameObjectName = "MD2BaseModController";

        public virtual void Start()
        {
            this.enabled = true;
            AddDesignators();
            this.enabled = false;
        }

        private void AddDesignators()
        {
            var def = DefDatabase<DesignationCategoryDef>.GetNamed("Orders");
            if(def.resolvedDesignators!=null)
            {
                def.resolvedDesignators.Add(new Designator_CollectSand());
            }
        }
    }
}
