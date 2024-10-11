namespace AirdPro.csimzMLParser.mzml
{
    public class ReferenceableParamGroupList : MzMLIDContentList<ReferenceableParamGroup>
    {
        private static long serialVersionUID = 1L;

        public ReferenceableParamGroupList(int count) : base(count)
        {

        }

        public ReferenceableParamGroupList(ReferenceableParamGroupList rpgList) : this(rpgList.Size())
        {
            foreach(ReferenceableParamGroup rgp in rpgList)
            {
                Add(new ReferenceableParamGroup(rgp));
            }
        }

        public void AddReferenceableParamGroup(ReferenceableParamGroup rpg)
        {
            Add(rpg);
        }

        public ReferenceableParamGroup GetReferenceableParamGroup(int index)
        {
            return Get(index);
        }

        public ReferenceableParamGroup GetReferenceableParamGroup(string id)
        {
            return Get(id);
        }

        public override string GetTagName()
        {
            return "referenceableParamGroupList";
        }
    }
}
