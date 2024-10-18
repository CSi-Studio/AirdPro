using AirdPro.csimzMLParser.affair;
using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.obo;
using AirdPro.csimzMLParser.util;
using HZH_Controls;
using log4net;
using log4net.Core;
using System;
using static AirdPro.csimzMLParser.obo.OBOTerm;

namespace AirdPro.csimzMLParser.mzml
{
    public abstract class CVParam : MzMLContent
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(CVParam));

        public enum CVParamType
        {
            DOUBLE,
            LONG,
            STRING,
            INTEGER,
            BOOLEAN,
            EMPTY
        }

        private const long serialVersionUID = 1L;

        public OBOTerm term;
        public OBOTerm units;

        public OBOTerm GetTerm()
        {
            return term;
        }

        public void SetTerm(OBOTerm term)
        {
            OBOTerm oldTerm = this.term;

            this.term = term;
            this.units = null;

            ResetValue();

            if (HasListeners())
                NotifyListeners(new OBOTermCVParamChangeAffair(this, oldTerm, term));
        }


        public abstract void ResetValue();

        public OBOTerm GetUnits()
        {
            return units;
        }

        public void SetUnits(OBOTerm units)
        {
            this.units = units;

            if (HasListeners())
                NotifyListeners(new CVParamChangeAffair(this));
        }



        public override string GetTagName()
        {
            return "cvParam";
        }

        public override string ToString()
        {
            string description = $"({term.GetID()}) {term.GetName()}";
            string value = GetValueAsString();

            if (value != null && !value.IsEmpty())
            {
                description += ":" + GetValueAsString();

                if (units != null)
                {
                    description += ":" + units.GetName();
                }
            }

            return description;
        }

        public abstract string GetValueAsString();
        public abstract double GetValueAsDouble();
        public abstract int GetValueAsInteger();
        public abstract long GetValueAsLong();

        public abstract void SetValueAsString(string newValue);

        public override string GetXMLAttributeText()
        {
            string attributes = $"cvRef=\"{XMLHelper.EnsureSafeXML(term.GetOntology().GetOntology().ToUpper())}\"";
            attributes += $" accession=\"{XMLHelper.EnsureSafeXML(term.GetID())}\"";
            attributes += $" name=\"{XMLHelper.EnsureSafeXML(term.GetName())}\"";

            string value = GetValueAsString();

            if (value != null && !value.Equals("null"))
            {
                attributes += $" value=\"{XMLHelper.EnsureSafeXML(value)}\"";
            }               

            if (units != null)
            {
                attributes += $" unitCvRef=\"{XMLHelper.EnsureSafeXML(units.GetOntology().GetOntology().ToUpper())}\"";
                attributes += $" unitAccession=\"{XMLHelper.EnsureSafeXML(units.GetID())}\"";
                attributes += $" unitName=\"{XMLHelper.EnsureSafeXML(units.GetName())}\"";
            }

            return attributes;
        }

        public static CVParamType GetCVParamType(OBOTerm term)
        {
            if (term == null || term.GetValueType() == null)
            {
                return CVParamType.EMPTY;
            }

            CVParamType type;

            switch (term.GetValueType())
            {
                case XMLType.STRING:
                    type = CVParamType.STRING;
                    break;
                case XMLType.FLOAT:
                case XMLType.DOUBLE:
                case XMLType.NON_NEGATIVE_FLOAT:
                    type = CVParamType.DOUBLE;
                    break;
                case XMLType.INT:
                    type = CVParamType.INTEGER;
                    break;
                case XMLType.BOOLEAN:
                    type = CVParamType.BOOLEAN;
                    break;
                case XMLType.NON_NEGATIVE_INTEGER:
                    type = CVParamType.LONG;
                    break;
                default:
                    LOGGER.Error($"Unknown CVParamType: {term.GetValueType()} (assigned to term {term.GetID()})");

                    InvalidFormatIssue issue = new InvalidFormatIssue(term, term.GetValueType());

                    throw new NonFatalParseException(issue);
            }

            return type;
        }

        public static CVParam CreateCVParam(OBOTerm term, OBOTerm units)
        {
            CVParam param;

            switch (GetCVParamType(term))
            {
                case CVParamType.STRING:
                    param = new StringCVParam(term, "", units);
                    break;
                case CVParamType.DOUBLE:
                    param = new DoubleCVParam(term, 0.0, units);
                    break;
                case CVParamType.LONG:
                    param = new LongCVParam(term, 0, units);
                    break;
                case CVParamType.INTEGER:
                    param = new IntegerCVParam(term, 0, units);
                    break;
                case CVParamType.BOOLEAN:
                    param = new BooleanCVParam(term, false, units);
                    break;
                default:
                    param = new EmptyCVParam(term, units);
                    break;
            }

            return param;
        }
    }
}
