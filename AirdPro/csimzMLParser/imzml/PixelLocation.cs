using System;

namespace AirdPro.csimzMLParser.imzml
{
    [Serializable]
    public class PixelLocation
    {
        /**
         * 序列版本ID。
         */
        private static readonly long serialVersionUID = 1L;

        /**
         * x坐标。
         */
        private readonly int x;
        /**
         * y坐标。
         */
        private readonly int y;
        /**
         * z坐标。
         */
        private readonly int z;

        /**
         * 构造PixelLocation (x, y, z)。
         *
         * @param x x坐标
         * @param y y坐标
         * @param z z坐标
         */
        public PixelLocation(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /**
         * 获取x坐标。
         *
         * @return x坐标
         */
        public int GetX()
        {
            return x;
        }

        /**
         * 获取y坐标。
         *
         * @return y坐标
         */
        public int GetY()
        {
            return y;
        }

        /**
         * 获取z坐标。
         *
         * @return z坐标
         */
        public int GetZ()
        {
            return z;
        }

        /**
         * 测试一个对象是否可以等于此类（即是否为PixelLocation类型）。
         *
         * @param other 要测试的对象
         * @return 如果是PixelLocation类型则返回true，否则返回false
         */
        public bool CanEqual(object other)
        {
            return (other is PixelLocation);
        }

        public override bool Equals(object other)
        {
            bool result = false;
            if (other is PixelLocation)
            {
                PixelLocation that = (PixelLocation)other;
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
