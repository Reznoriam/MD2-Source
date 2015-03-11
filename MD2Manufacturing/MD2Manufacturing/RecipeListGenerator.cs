using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public static class RecipeListGenerator
    {
        public static IEnumerable<RecipeDef> AllRecipes()
        {
            foreach(var def in DefDatabase<ManufacturingPlantRecipesDef>.AllDefs)
            {
                foreach(var recipe in def.recipes)
                {
                    yield return recipe;
                }
            }
            foreach(var def in DefDatabase<RecipeDef>.AllDefs.Where(d => d.defName.Contains("Make_Apparel") || d.defName.Contains("Make_MeleeWeapon") || d.defName.Contains("Make_Bow")))
            {
                yield return def;
            }
        }
    }
}
