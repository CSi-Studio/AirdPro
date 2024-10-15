using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AirdPro.csimzMLParser.obo
{
    [Serializable]
    public class OBOTerm
    {
        private static readonly ILog LOGGER = LogManager.GetLogger(typeof(OBOTerm));

        private const long serialVersionUID = 1L;

        public OBO ontology;

        public readonly string id;

        public string name;

        public string nameSpace;

        public string description;

        public List<OBOTerm> has_units;

        public List<string> unitList;

        public List<string> is_a;

        public List<string> part_of;

        public List<OBOTerm> children;

        public List<OBOTerm> parents = [];

        public bool is_obsolete;

        public enum XMLType
        {
            STRING,
            BOOLEAN,
            DECIMAL,
            FLOAT,
            DOUBLE,
            DURATION,
            DATETIME,
            TIME,
            DATE,
            GYEARMONTH,
            GYEAR,
            GMONTHDAY,
            GDAY,
            GMONTH,
            HEXBINARY,
            BASE64BINARY,
            ANY_URI,
            QNAME,
            NOTATION,
            INTEGER,
            NON_POSITIVE_INTEGER,
            NEGATIVE_INTEGER,
            LONG,
            INT,
            SHORT,
            BYTE,
            NON_NEGATIVE_INTEGER,
            UNSIGNED_LONG,
            UNSIGNED_INT,
            UNSIGNED_SHORT,
            UNSIGNED_BYTE,
            POSITIVE_INTEGER,
            NON_NEGATIVE_FLOAT,
            NON_NEGATIVE_DOUBLE
        }

        protected XMLType ValueType { get; set; }

        public enum Synonym
        {
            EXACT,
            BROAD,
            NARROW,
            RELATED
        }

        public OBOTerm(OBO ontology, string id)
        {
            this.ontology = ontology;
            this.id = id;            
        }

        protected void AddIs_a(string relationship)
        {
            if (is_a is List<string>)
            {
                is_a.Add(relationship);
            }
            else if (is_a != null)
            {
                is_a = new List<string>(is_a) { relationship };
            }
            else
            {
                is_a = [relationship];
            }
        }

        protected void AddUnits(string units)
        {
            if (unitList is List<string>)
            {
                unitList.Add(units);
            }
            else if (unitList != null)
            {
                unitList = new List<string>(unitList) { units };
            }
            else
            {
                unitList = [units];
            }            
        }

        protected void AddPartOf(string relationship)
        {
            if (part_of is List<string>)
            {
                part_of.Add(relationship);
            }
            else if (part_of != null)
            {
                part_of = new List<string>(part_of) { relationship };
            }
            else
            {
                part_of = [relationship];
            }
        }

        public void Parse(string strippedLine)
        {
            int indexOfColon = strippedLine.IndexOf(':');
            string tag = strippedLine.Substring(0, indexOfColon).Trim();
            string value = strippedLine.Substring(indexOfColon + 1).Trim();

            // Strip comments
            int indexOfExclamation = value.IndexOf('!');

            if (indexOfExclamation > -1)
            {
                value = value.Substring(0, indexOfExclamation).Trim();
            }

            switch (tag.ToLower())
            {
                case "name":
                    name = value;
                    break;
                case "namespace":
                    nameSpace = value;
                    break;
                case "def":
                    description = value;
                    break;
                case "relationship":
                    int indexOfSpace = value.IndexOf(' ');
                    string relationshipTag = value.Substring(0, indexOfSpace).Trim();
                    string relationshipValue = value.Substring(indexOfSpace + 1).Trim();

                    if ("is_a".Equals(relationshipTag))
                    {
                        AddIs_a(relationshipValue);
                    }
                    else if ("has_units".Equals(relationshipTag))
                    {
                        AddUnits(relationshipValue);
                    }
                    else if ("part_of".Equals(relationshipTag))
                    {
                        AddPartOf(relationshipValue);
                    }
                    break;
                case "is_a":
                    AddIs_a(value);
                    break;
                case "is_obsolete":
                    is_obsolete = bool.Parse(value);
                    break;
                case "xref":
                    if (value.Contains("value-type:xsd:"))
                    {
                        string[] substrings = value.Replace("value-type:xsd:", "").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        switch (substrings[0])
                        {
                            case "string":
                                ValueType = XMLType.STRING;
                                break;
                            case "integer":
                                ValueType = XMLType.INTEGER;
                                break;
                            case "int":
                                ValueType = XMLType.INT;
                                break;
                            case "decimal":
                                ValueType = XMLType.DECIMAL;
                                break;
                            case "negativeinteger":
                                ValueType = XMLType.NEGATIVE_INTEGER;
                                break;
                            case "positiveinteger":
                                ValueType = XMLType.POSITIVE_INTEGER;
                                break;
                            case "nonnegativeinteger":
                                ValueType = XMLType.NON_NEGATIVE_INTEGER;
                                break;
                            case "boolean":
                                ValueType = XMLType.BOOLEAN;
                                break;
                            case "date":
                                ValueType = XMLType.DATE;
                                break;
                            case "datetime":
                                ValueType = XMLType.DATETIME;
                                break;
                            case "float":
                                ValueType = XMLType.FLOAT;
                                break;
                            case "nonnegativefloat":
                                ValueType = XMLType.NON_NEGATIVE_FLOAT;
                                break;
                            case "nonnegativedouble":
                                ValueType = XMLType.NON_NEGATIVE_DOUBLE;
                                break;
                            case "double":
                                ValueType = XMLType.DOUBLE;
                                break;
                            case "anyuri":
                                ValueType = XMLType.ANY_URI;
                                break;
                            default:
                                LOGGER.InfoFormat("Unknown value-type encountered '{0}' @ {1}", value, id);
                                break;
                        }
                    }
                    else
                    {   
                        LOGGER.InfoFormat("Unknown xref encountered '{0}' @ {1}", value, id);
                    }
                    break;
                default:
                    LOGGER.InfoFormat("Tag not implemented '{0}'", tag);
                    break;
            }
        }

        public void AddChild(OBOTerm child)
        {
            if (children is List<OBOTerm>)
            {
                children.Add(child);
            }
            else if (children != null)
            {
                children = new List<OBOTerm>(children) { child };
            }
            else
            {
                children = [child];
            }
        }

        public XMLType GetValueType()
        {
            return ValueType;
        }

        public bool IsObsolete()
        {
            return is_obsolete;
        }

        public void AddParent(OBOTerm parent)
        {
            if (parents.Count > 1)
            {
                parents.Add(parent);
            }
            else if (parents.Count == 1)
            {
                parents = new List<OBOTerm>(parents) { parent };
            }
            else
            {
                parents = [parent];
            }
        }

        public bool IsParentOf(string id)
        {           
            foreach(OBOTerm child in GetAllChildren(false))
            {
                if (child.id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsChildOf(string id)
        {
            foreach(OBOTerm parent in GetAllParents(false))
            {
                LOGGER.InfoFormat("In isChildOf() checking parent {0}", parent);
                if (parent.id.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }

        public List<OBOTerm> GetChildren()
        {
            return children;
        }

        public List<OBOTerm> GetAllChildren(bool includeThis)
        {
            List<OBOTerm> allChildren = [];

            if (includeThis)
            {
                allChildren.Add(this);
            }

            if(children != null)
            {
                foreach(OBOTerm child in children)
                {
                    child.GetAllChildren(allChildren, true);
                }
            }

            return allChildren;
        }

        private void GetAllChildren(List<OBOTerm> allChildren, bool includeThis)
        {
            if (includeThis)
            {
                allChildren.Add(this);
            }

            if (children != null)
            {
                foreach (OBOTerm child in children)
                {
                    child.GetAllChildren(allChildren, true);
                }
            }
        }

        public List<OBOTerm> GetAllParents(bool includeThis)
        {
            List<OBOTerm> allParents = [];
            LOGGER.InfoFormat("Getting all parents of {0}, which has {1} parent(s)", id, parents.Count);
            GetAllParents(allParents, includeThis);
            return allParents;
        }

        private void GetAllParents(List<OBOTerm> allParents, bool includeThis)
        {
            if (includeThis)
            {
                allParents.Add(this);
            }

            foreach (OBOTerm parent in parents)
            {
                parent.GetAllParents(allParents, true);
            }
        }

        public bool HasParent(OBOTerm term)
        {
            return GetAllParents(false).Contains(term);
        }

        public List<string> GetIsA()
        {
            return is_a;
        }

        public void ClearIsA()
        {
            is_a.Clear();
        }

        public List<string> GetPartOf()
        {
            return part_of;
        }

        public string GetName()
        {
            return name;
        }

        public string GetDescription()
        {
            return description;
        }

        public List<OBOTerm> GetUnits()
        {
            return has_units;
        }

        public string GetNamespace()
        {
            return nameSpace;
        }

        public OBO GetOntology()
        {
            return ontology;
        }

        public void AddUnit(OBOTerm unit)
        {
            if (has_units is List<OBOTerm>)
            {
                has_units.Add(unit);
            }
            else if (has_units != null)
            {
                has_units = new List<OBOTerm>(has_units) { unit };
            }
            else
            {
                has_units = new List<OBOTerm> { unit };
            }
        }

        public override string ToString()
        {
            return $"({id}) {name}";
        }

        public override bool Equals(object o)
        {
            if (o == this)
            {
                return true;
            }

            if (!(o is OBOTerm term))
            {
                return false;
            }

            bool namespaceOK = (nameSpace == null && term.nameSpace == null) || (nameSpace != null && nameSpace.Equals(term.nameSpace));

            return term.id.Equals(id) && namespaceOK;
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = 23 * hash + (id?.GetHashCode() ?? 0);
            return hash;
        }
    }
}
