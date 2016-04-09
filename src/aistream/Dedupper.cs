using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aistream
{
    class Dedupper<T>
    {
        public const int setSize = 100;

        private HashSet<T> set1 = new HashSet<T>();
        private HashSet<T> set2 = new HashSet<T>();

        public bool IsDupe(T id)
        {
            bool set1Has = set1.Contains(id);
            bool set2Has = set2.Contains(id);
            bool isDupe = false;

            if (!set1Has && !set2Has)
            {
                if (set1.Count < setSize)
                {
                    set1.Add(id);
                }
                else
                {
                    set2.Add(id);
                }
            }
            else
            {
                isDupe = true;
            }

            if (set2.Count > setSize)
            {
                set1 = set2;
                set2 = new HashSet<T>();
            }

            return isDupe;
        }
    }
}
