using System;

namespace AirdPro.IMSRawDataCompress.datamodel.enums
{
    public class PolarityType
    {
        public int Sign { get; }
        public string CharValue { get; }

        public PolarityType(int sign, string charValue)
        {
            Sign = sign;
            CharValue = charValue;
        }

        // 枚举成员
        public enum Type
        {
            POSITIVE,
            NEGATIVE,
            NEUTRAL,
            ANY,
            UNKNOWN
        }

        public static readonly PolarityType POSITIVE = new PolarityType(+1, "+");
        public static readonly PolarityType NEGATIVE = new PolarityType(-1, "-");
        public static readonly PolarityType NEUTRAL = new PolarityType(0, "n");
        public static readonly PolarityType ANY = new PolarityType(0, "Any");
        public static readonly PolarityType UNKNOWN = new PolarityType(0, "?");

        public static PolarityType parseFromString(String charValue)
        {
            if (charValue == null)
            {
                return UNKNOWN;
            }

            charValue = charValue.ToLower(); // 将输入字符串转换为小写以便比较
            switch (charValue)
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
                case "any":
                    return PolarityType.ANY;
                default:
                    return PolarityType.UNKNOWN;
            }
        }

    }
}
