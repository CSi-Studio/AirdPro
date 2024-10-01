using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirdPro.ImzMLParser.mzml
{
    public class SourceFileList : MzMLIDContentList<SourceFile>
    {
        private static readonly long serialVersionUID = 1L;

        public SourceFileList(int count) : base(count)
        {
        }

        public SourceFileList(SourceFileList sourceFileList, ReferenceableParamGroupList rpgList) : this(sourceFileList.Size())
        { 
            foreach (SourceFile sourceFile in sourceFileList)
            {
                Add(new SourceFile(sourceFile, rpgList));
            }
        }

        public void AddSourceFile(SourceFile sourceFile)
        {
            Add(sourceFile);
        }

        public SourceFile GetSourceFile(string id)
        {
            return Get(id);
        }

        public SourceFile GetSourceFile(int index)
        {            
            return Get(index);
        }

        public override string GetTagName()
        {
            return "ourceFileList";
        }
    }
}
