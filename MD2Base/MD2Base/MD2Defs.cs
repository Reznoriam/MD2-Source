using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public static class MD2Defs
    {
        public static ThingDef Ruby
        {
            get
            {
                return ThingDef.Named("MD2Ruby");
            }
        }

        public static ThingDef DSU1
        {
            get
            {
                return ThingDef.Named("mipDeepStorageI");
            }
        }

        public static ThingDef DSU2
        {
            get
            {
                return ThingDef.Named("mipDeepStorageII");
            }
        }

        public static ThingDef DSU3
        {
            get
            {
                return ThingDef.Named("mipDeepStorageIII");
            }
        }

        public static ThingDef Extractor
        {
            get
            {
                return ThingDef.Named("mipExtractor");
            }
        }

        public static ThingDef Fissure
        {
            get
            {
                return ThingDef.Named("mipFissure");
            }
        }

        public static ThingDef FissureGenerator
        {
            get
            {
                return ThingDef.Named("miFissureGenerator");
            }
        }

        public static ThingDef MechWallExtended
        {
            get
            {
                return ThingDef.Named("MechWallExtended");
            }
        }

        public static ThingDef MechWallRecessed
        {
            get
            {
                return ThingDef.Named("MechWallRecessed");
            }
        }
    }
}
