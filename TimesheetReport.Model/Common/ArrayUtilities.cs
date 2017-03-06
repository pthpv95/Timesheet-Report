using System.Collections.Generic;
using System.Linq;

namespace SingLife.PriceComparison.Model.Common
{
    public static class ArrayUtilities
    {
        public static bool ByteArraysAreEqual(byte[] left, byte[] right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            if ((left == null && right != null)
                || (left != null && right == null))
            {
                return false;
            }

            if (left.LongLength != right.LongLength)
            {
                return false;
            }

            for (int index = 0; index < left.LongLength; index++)
            {
                if (left[index] != right[index])
                {
                    return false;
                }
            }

            return true;
        }

        public static int CalculateHashCode(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
            {
                return 0;
            }

            IEnumerable<object> bytes = byteArray
                .Select(b => (object)b);

            return HashCalculator.CalculateHashFor(bytes);
        }
    }
}
