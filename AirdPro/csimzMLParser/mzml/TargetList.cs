namespace AirdPro.csimzMLParser.mzml
{
    public class TargetList(int count) : MzMLContentList<Target>(count)
    {
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
