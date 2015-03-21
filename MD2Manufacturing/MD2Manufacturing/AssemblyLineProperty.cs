using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace MD2
{
    public class AssemblyLineProperty: Saveable
    {
        private float value;
        public AssemblyLineProperty(float initialValue)
        {
            this.value = initialValue;
        }
        public AssemblyLineProperty():this(1f)
        {

        }

        public static implicit operator float(AssemblyLineProperty p)
        {
            return p.Value;
        }

        public static AssemblyLineProperty operator -(AssemblyLineProperty p, float num)
        {
            float num2 = p - num;
            if (num2 <= 0)
                num2 = 0.01f;
            return new AssemblyLineProperty(num2);
        }

        public static AssemblyLineProperty operator +(AssemblyLineProperty p, float num)
        {
            return new AssemblyLineProperty(p + num);
        }

        private float Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
                if (this.value <= 0) this.value = 0.01f;
            }
        }

        public void ExposeData()
        {
            Scribe_Values.LookValue(ref this.value, "value", 1f);
        }
    }
}
