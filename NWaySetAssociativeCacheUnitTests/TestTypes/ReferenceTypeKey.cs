using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWaySetAssociativeCacheUnitTests
{
    class ReferenceTypeKey : IEquatable<ReferenceTypeKey>
    {
        public int TestVal;

        public ReferenceTypeKey(int val)
        {
            TestVal = val;
        }

        public bool Equals(ReferenceTypeKey other)
        {
            if (other == null)
                return false;
            return TestVal == other.TestVal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != GetType())
                return false;
            return Equals(obj as ReferenceTypeKey);
        }

        public override int GetHashCode()
        {
            int hash = 19;
            hash = (hash * 13) + TestVal.GetHashCode();
            return hash;
        }
    }
}
