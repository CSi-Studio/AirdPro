using Enum = System.Enum;

namespace AirdPro.Domains.Msi
{
    public enum PolarityType
    {
        POSITIVE = +1,
        NEGATIVE = -1,
        NEUTRAL = 0,
        ANY = 0,
        UNKNOWN = 0
    }

    public static class PolarityTypeExtensions
    {
        public static PolarityType ParseFromString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return PolarityType.UNKNOWN;
            }

            switch (str.ToLower())
            {
                case "+":
                case "positive":
                case "pos":
                case "+1":
                case "1+":
                case "1":
                    return PolarityType.POSITIVE;
                case "-":
                case "negative":
                case "neg":
                case "-1":
                case "1-":
                    return PolarityType.NEGATIVE;
                case "any polarity":
                    return PolarityType.ANY;
                default:
                    return PolarityType.UNKNOWN;
            }
        }

        public static PolarityType FromSingleChar(string s)
        {
            foreach (PolarityType p in Enum.GetValues(typeof(PolarityType)))
            {
                if (p.ToString() == s)
                {
                    return p;
                }
            }
            return PolarityType.UNKNOWN;
        }

        public static PolarityType FromInt(int i)
        {
            if (i == 0)
            {
                return PolarityType.UNKNOWN;
            }
            else if (i < 0)
            {
                return PolarityType.NEGATIVE;
            }
            else
            {
                return PolarityType.POSITIVE;
            }
        }

        public static string AsSingleChar(this PolarityType polarity)
        {
            return polarity.ToString();
        }

        public static int GetSign(this PolarityType polarity)
        {
            return (int)polarity;
        }

        public static string ToLabel(this PolarityType polarity)
        {
            if (polarity == PolarityType.ANY)
            {
                return "Any polarity";
            }
            return char.ToUpper(polarity.ToString()[0]) + polarity.ToString().Substring(1).ToLower();
        }

        public static string ToString(this PolarityType polarity)
        {
            return polarity.AsSingleChar();
        }

        public static bool IsDefined(this PolarityType polarity)
        {
            return polarity == PolarityType.POSITIVE || polarity == PolarityType.NEGATIVE;
        }

        public static bool IncludesPositive(this PolarityType polarity)
        {
            return polarity == PolarityType.POSITIVE || polarity == PolarityType.ANY;
        }

        public static bool IncludesNegative(this PolarityType polarity)
        {
            return polarity == PolarityType.NEGATIVE || polarity == PolarityType.ANY;
        }

        public static bool IncludesCharge(this PolarityType polarity, int charge)
        {
            return polarity == PolarityType.ANY || (polarity == PolarityType.NEGATIVE && charge < 0) || (polarity == PolarityType.POSITIVE && charge > 0);
        }
    }
}
