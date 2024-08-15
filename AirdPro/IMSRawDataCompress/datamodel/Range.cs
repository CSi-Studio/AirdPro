using System;

namespace AirdPro.IMSRawDataCompress.datamodel
{
    public class Range<T> where T : IComparable<T>
    {
        private T LowerBound { get; }
        private T UpperBound { get; }

        public Range(T lowerBound, T upperBound)
        {
            if (lowerBound.CompareTo(upperBound) > 0)
                throw new ArgumentException("Lower bound cannot be greater than upper bound.");

            LowerBound = lowerBound;
            UpperBound = upperBound;
        }

        public bool Contains(T value)
        {
            return value.CompareTo(LowerBound) >= 0 && value.CompareTo(UpperBound) <= 0;
        }

        static public Range<T> Closed(T lowerBound, T upperBound)
        {
            return new Range<T>(lowerBound, upperBound);
        }

        public override string ToString()
        {
            return $"[{LowerBound}, {UpperBound}]";
        }
    }
}
