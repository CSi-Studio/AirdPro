namespace AirdPro.csimzMLParser.mzml
{
    public class TargetList : MzMLContentList<Target>
    {
        private static readonly long serialVersionUID = 1L;

        public TargetList(int count) : base(count)
        {
            
        }

        public TargetList(TargetList targetList, ReferenceableParamGroupList rpgList) : this(targetList.Size())
        {
            foreach (Target target in targetList)
            {
                Add(new Target(target, rpgList));
            }
        }

        public void AddTarget(Target target)
        {
            Add(target);
        }

        public override string GetTagName()
        {
            return "targetList";
        }

    }
}
