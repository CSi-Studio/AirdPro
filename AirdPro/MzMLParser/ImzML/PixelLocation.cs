using System;

namespace AirdPro.csimzMLParser.imzml
{
    [Serializable]
    public class PixelLocation(int x, int y, int z)
    {
        private readonly int x = x;
        private readonly int y = y;
        private readonly int z = z;

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }
               
        public int GetZ()
        {
            return z;
        }

        public bool CanEqual(object other)
        {
            return (other is PixelLocation);
        }

        public override bool Equals(object other)
        {
            bool result = false;
            if (other is PixelLocation that)
            {
                result = (that.CanEqual(this) && this.GetX() == that.GetX() && this.GetY() == that.GetY() && this.GetZ() == that.GetZ());
            }
            return result;
        }

        public override int GetHashCode()
        {
            return (41 * (41 * (41 + GetX()) + GetY()) + GetZ());
        }
    }
}
