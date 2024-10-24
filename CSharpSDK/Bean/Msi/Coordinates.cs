using System;
using System.Collections.Generic;
using System.Text;

namespace AirdSDK.Bean.Msi
{
    public class Coordinates
    {
        private int x { set; get;}
        private int y {set; get;}
        private int z {set; get;}
        public Coordinates(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            return x + ";" + y + ";" + z + ";";
        }

        public override bool Equals(object obj)
        {
            return obj is Coordinates coordinates &&
                x == coordinates.x &&
                y == coordinates.y &&
                z == coordinates.z;
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + x;
            result = prime * result + y;
            result = prime * result + z;
            return result;
        }
    }
}
