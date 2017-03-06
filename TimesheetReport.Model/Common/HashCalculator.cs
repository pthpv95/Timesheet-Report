using System.Collections.Generic;
using System.Linq;

namespace SingLife.PriceComparison.Model.Common
{
    public static class HashCalculator
    {
        /// <summary>
        /// Calculate a hash value representing the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">An enumerable of values to calculate the hash.</param>
        /// <returns>An integer representing the hash value.</returns>
        public static int CalculateHashFor(IEnumerable<object> values)
        {
            unchecked
            {
                var hashCode = 0;

                foreach (var value in values)
                {
                    hashCode = (hashCode * 397) ^ (value != null ? value.GetHashCode() : 0);
                }

                return hashCode;
            }
        }

        /// <summary>
        /// Calculate a hash value representing the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">An array of values to calculate the hash.</param>
        /// <returns>An integer representing the hash value.</returns>
        public static int CalculateHashFor(params object[] values)
        {
            return CalculateHashFor(values.AsEnumerable());
        }
    }
}