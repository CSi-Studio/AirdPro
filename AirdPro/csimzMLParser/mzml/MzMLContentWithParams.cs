using AirdPro.csimzMLParser.affair;
using HZH_Controls;
using pwiz.CLI.cv;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace AirdPro.csimzMLParser.mzml
{
    public abstract class MzMLContentWithParams : MzMLContent, IHasParams
    {
        private List<ReferenceableParamGroupRef> referenceableParamGroupRefs = [];
        private List<CVParam> cvParams = [];
        private List<UserParam> userParams = [];

        public MzMLContentWithParams()
        {
        }

        public MzMLContentWithParams(MzMLContentWithParams mzMLContent, ReferenceableParamGroupList rpgList)
        {
            if (mzMLContent.referenceableParamGroupRefs.IsEmpty())
            {
                referenceableParamGroupRefs = new List<ReferenceableParamGroupRef>(mzMLContent.referenceableParamGroupRefs.Count);

                if (rpgList != null)
                {
                    foreach (ReferenceableParamGroupRef rpgRef in mzMLContent.referenceableParamGroupRefs)
                    {
                        foreach (ReferenceableParamGroup rpg in rpgList)
                        {
                            if (rpg.GetID().Equals(rpgRef.GetReference().GetID()))
                            {
                                referenceableParamGroupRefs.Add(new ReferenceableParamGroupRef(rpg));
                                break;
                            }
                        }
                    }
                }
            }

            if (!mzMLContent.cvParams.IsEmpty())
            {
                cvParams = [];
                foreach (CVParam cvParam in mzMLContent.cvParams)
                {
                    if (cvParam is StringCVParam)
                    {
                        cvParams.Add(new StringCVParam((StringCVParam)cvParam));
                    }
                    else if (cvParam is LongCVParam)
                    {
                        cvParams.Add(new LongCVParam((LongCVParam)cvParam));
                    }
                    else if (cvParam is DoubleCVParam)
                    {
                        cvParams.Add(new DoubleCVParam((DoubleCVParam)cvParam));
                    }
                    else if (cvParam is IntegerCVParam)
                    {
                        cvParams.Add(new IntegerCVParam((IntegerCVParam)cvParam));
                    }
                    else if (cvParam is BooleanCVParam)
                    {
                        cvParams.Add(new BooleanCVParam((BooleanCVParam)cvParam));
                    }
                    else if (cvParam is EmptyCVParam)
                    {
                        cvParams.Add(new EmptyCVParam((EmptyCVParam)cvParam));
                    }
                    else
                    {
                        throw new ArgumentException("Unknown CVParam type, unable to replicate: " + cvParam.GetType());
                    }
                }
            }

            if (!mzMLContent.userParams.IsEmpty())
            {
                userParams = [];

                foreach (UserParam userParam in mzMLContent.userParams)
                {
                    userParams.Add(new UserParam(userParam));
                }
            }
        }

        public virtual void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            children.Add((IMzMLTag)referenceableParamGroupRefs);
            children.Add((IMzMLTag)cvParams);            
            children.Add((IMzMLTag)userParams);
        }

        public List<ReferenceableParamGroupRef> GetReferenceableParamGroupRefList()
        {
            return referenceableParamGroupRefs;
        }

        public ReferenceableParamGroup FindBestFittingRPG(ReferenceableParamGroupList rpgList)
        {
            if (rpgList == null)
                return null;

            ReferenceableParamGroup bestFittingGroup = null;

            foreach (ReferenceableParamGroup rpg in rpgList)
            {
                int numParamsFound = 0;

                foreach (CVParam cvParam in rpg.GetCVParamList())
                {
                    CVParam curParam = GetCVParam(cvParam.GetTerm().GetID());

                    if (curParam != null && ContainsCVParam(curParam))
                    {
                        string value1 = curParam.GetValueAsString();
                        string value2 = cvParam.GetValueAsString();

                        if ((value1 == null && value2 == null) || (value1 != null && value1.Equals(value2)))
                        {
                            numParamsFound++;
                        }
                    }
                }

                if (numParamsFound == rpg.GetCVParamCount())
                {
                    if (bestFittingGroup == null || numParamsFound > bestFittingGroup.GetCVParamCount())
                    {
                        bestFittingGroup = rpg;
                    }
                }
            }

            return bestFittingGroup;
        }

        public void ReplaceCVParamsWithRPG(ReferenceableParamGroup rpg)
        {
            foreach (CVParam replacementParam in rpg.GetCVParamList())
            {
                CVParam paramToRemove = GetCVParam(replacementParam.GetTerm().GetID());

                RemoveCVParam(paramToRemove);
            }

            AddReferenceableParamGroupRef(new ReferenceableParamGroupRef(rpg));
        }

        public virtual List<CVParam> GetCVParamList()
        {
            return cvParams;
        }

        public virtual List<UserParam> GetUserParamList()
        {
            return userParams;
        }

        public virtual void AddReferenceableParamGroupRef(ReferenceableParamGroupRef rpg)
        {
            bool exists = false;

            foreach (ReferenceableParamGroupRef rpgRef in GetReferenceableParamGroupRefList())
            {
                if (rpgRef.GetReference().GetID().Equals(rpg.GetReference().GetID()))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                if (referenceableParamGroupRefs.Count > 1)
                {
                    referenceableParamGroupRefs.Add(rpg);
                }
                else if (referenceableParamGroupRefs.Count == 1)
                {
                    referenceableParamGroupRefs = new List<ReferenceableParamGroupRef>(referenceableParamGroupRefs)
                    {
                        rpg
                    };
                }
                else
                {
                    referenceableParamGroupRefs = [rpg];
                }
            }
        }

        public void RemoveAllReferenceableParamGroupRefs()
        {
            referenceableParamGroupRefs = [];
        }

        public virtual int GetReferenceableParamGroupRefCount()
        {
            return GetReferenceableParamGroupRefList().Count;
        }

        public virtual ReferenceableParamGroupRef GetReferenceableParamGroupRef(int index)
        {
            return GetReferenceableParamGroupRefList()[index];
        }

        public virtual ReferenceableParamGroupRef GetReferenceableParamGroupRef(string id)
        {
            foreach (ReferenceableParamGroupRef rpgRef in referenceableParamGroupRefs)
            {
                if (rpgRef.GetReference().GetID().Equals(id))
                {
                    return rpgRef;
                }
            }

            return null;
        }

        public virtual void AddCVParam(CVParam cvParam)
        {
            if (cvParam != null)
            {
                if (cvParams.Count > 1)
                {
                    cvParams.Add(cvParam);
                }
                else if (cvParams.Count == 1)
                {
                    cvParams = new List<CVParam>(cvParams)
                    {
                        cvParam
                    };
                }
                else
                {
                    cvParams = [cvParam];
                }

                cvParam.SetParent(this);

                if (HasListeners()) 
                {
                    NotifyListeners(new CVParamAddedAffair(this, cvParam));
                }
                    
            }
        }

        public bool ContainsCVParam(CVParam param)
        {
            return cvParams.Contains(param);
        }

        public virtual void RemoveCVParam(int index)
        {
            CVParam paramRemoved = GetCVParam(index);
            GetCVParamList().RemoveAt(index);

            if (HasListeners())
                NotifyListeners(new CVParamRemovedAffair(this, paramRemoved));

            paramRemoved.SetParent(null);
        }

        public virtual void RemoveCVParam(CVParam param)
        {
            if (cvParams.Remove(param))
            {
                if (HasListeners())
                {
                    NotifyListeners(new CVParamRemovedAffair(this, param));
                }

                param.SetParent(null);
            }
        }

        public virtual void RemoveCVParam(string id)
        {
            List<CVParam> cvParamList = [];

            foreach (CVParam cvParam in cvParams)
            {
                if (cvParam.GetTerm().GetID().Equals(id))
                {
                    cvParamList.Add(cvParam);
                }
            }

            foreach (CVParam cvParam in cvParamList)
            {
                cvParams.Remove(cvParam);

                if (HasListeners())
                {
                    NotifyListeners(new CVParamRemovedAffair(this, cvParam));
                }

                cvParam.SetParent(null);
            }
        }

        public virtual void RemoveChildrenOfCVParam(string id, bool includeCurrent)
        {
            List<CVParam> children = GetChildrenOf(id, includeCurrent);

            foreach (CVParam cvParam in children)
            {
                cvParams.Remove(cvParam);

                if (HasListeners())
                    NotifyListeners(new CVParamRemovedAffair(this, cvParam));

                cvParam.SetParent(null);
            }
        }

        public virtual void AddUserParam(UserParam userParam)
        {
            if (userParam != null)
            {
                if (userParams.Count > 1)
                {
                    userParams.Add(userParam);
                }
                else if (userParams.Count == 1)
                {
                    userParams = new List<UserParam>(userParams)
                    {
                        userParam
                    };
                }
                else
                {
                    userParams = [userParam];
                }

                userParam.SetParent(this);
            }
        }

        public virtual void RemoveUserParam(int index)
        {
            UserParam removedParam = userParams[index];
            userParams.RemoveAt(index);
            removedParam.SetParent(null);
        }

        public virtual CVParam GetCVParam(string id)
        {
            foreach (ReferenceableParamGroupRef rpgRef in referenceableParamGroupRefs)
            {
                if (rpgRef == null)
                {
                    continue;
                }

                CVParam cvParam = rpgRef.GetReference().GetCVParam(id);

                if (cvParam != null)
                {
                    return cvParam;
                }
            }

            foreach (CVParam cvParam in cvParams)
            {
                if (cvParam.GetTerm().GetID().Equals(id))
                {
                    return cvParam;
                }
            }

            return null;
        }

        public virtual CVParam GetCVParam(int index)
        {
            return cvParams[index];
        }

        public int GetCVParamCount()
        {
            return cvParams.Count;
        }

        public virtual CVParam GetCVParamOrChild(string id)
        {
            foreach (ReferenceableParamGroupRef rpgRef in referenceableParamGroupRefs)
            {
                if (rpgRef == null)
                {
                    continue;
                }

                CVParam cvParam = rpgRef.GetReference().GetCVParam(id);

                if (cvParam != null)
                {
                    return cvParam;
                }

                List<CVParam> childList = rpgRef.GetReference().GetChildrenOf(id, false);

                if (!childList.IsEmpty())
                {
                    return childList[0];
                }
            }

            foreach (CVParam cvParam in cvParams)
            {
                if (cvParam.GetTerm().GetID().Equals(id))
                {
                    return cvParam;
                }
            }

            List<CVParam> children = GetChildrenOf(id, false);

            if (!children.IsEmpty())
            {
                return children[0];
            }

            return null;
        }

        public UserParam GetUserParam(string name)
        {
            foreach (ReferenceableParamGroupRef rpgRef in referenceableParamGroupRefs)
    {
                if (rpgRef == null)
                {
                    continue;
                }

                UserParam userParam = rpgRef.GetReference().GetUserParam(name);

                if (userParam != null)
                {
                    return userParam;
                }
            }

            foreach (UserParam userParam in userParams)
            {
                if (userParam.Name.Equals(name))
                {
                    return userParam;
                }
            }

            return null;
        }

        public UserParam GetUserParam(int index)
        {
            return userParams[index];
        }

        public List<CVParam> GetChildrenOf(string id, bool includeCurrent)
        {
            List<CVParam> children = [];

            foreach (ReferenceableParamGroupRef rpgRef in referenceableParamGroupRefs)
            {
                if (rpgRef == null)
                {
                    continue;
                }

                children.AddRange(rpgRef.GetReference().GetChildrenOf(id, includeCurrent));
            }

            foreach (CVParam cvParam in cvParams)
            {
                if (cvParam.GetTerm().IsChildOf(id))
                {
                    children.Add(cvParam);
                }

                if (includeCurrent && cvParam.GetTerm().GetID().Equals(id))
                {
                    children.Add(cvParam);
                }
            }

            return children;
        }
    }
}
