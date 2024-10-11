namespace AirdPro.Domains.Msi
{
    public class Coordinates
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Coordinates(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return X + ";" + Y + ";" + Z + ";";
        }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime * result + X;
            result = prime * result + Y;
            result = prime * result + Z;
            return result;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            Coordinates other = (Coordinates)obj;
            return X == other.X && Y == other.Y && Z == other.Z;
        }
    }
}
