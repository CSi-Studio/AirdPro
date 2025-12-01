using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AirdPro.csimzMLParser.mzml
{
    public abstract class MzMLIDContentList<T> : MzMLContentList<T>, IReferenceList<T> where T : IReferenceableTag, IMzMLTag
    {        
        Dictionary<string, T> dictionary = [];
        
        public MzMLIDContentList(int count) : base(count)
        {

        }

        public MzMLIDContentList(MzMLIDContentList<T> contentList) : base(contentList)
        {
            
        }
        
        public virtual T Get(string id)
        {
            if (dictionary.ContainsKey(id))
            {
                return dictionary[id];
            }
            return default;
        }

        private static readonly Regex LAST_INTEGER_PATTERN = new(@"[^0-9]+([0-9]+)$");

        public override void Add(T item)
        {
            if (ContainsID(item.GetID()))
            {
                Match matcher = LAST_INTEGER_PATTERN.Match(item.GetID());
                if (matcher.Success)
                {
                    string someNumberStr = matcher.Groups[1].Value;
                    int lastNumberInt = int.Parse(someNumberStr);

                    item.SetID(item.GetID().Replace(matcher.Groups[1].Value, "" + (lastNumberInt + 1)));
                }
                else
                {
                    item.SetID(item.GetID() + "0");
                }
            }
        
            dictionary ??= [];

            dictionary[item.GetID()] = item;
            base.Add(item);
        }

        public override T Remove(int index)
        {
            T removed = base.Remove(index);

            if (removed != null)
                dictionary.Remove(removed.GetID());

            return removed;
        }

        public override bool Remove(T item)
        {
            dictionary.Remove(item.GetID());
            return base.Remove(item);
        }

        public bool ContainsID(string id)
        {
            if (dictionary == null)
                return false;

            return dictionary.ContainsKey(id);
        }

        public virtual T GetValidReference(T processing)
        {
            foreach (T curProcessing in this)
            {
                if (processing.Equals(curProcessing) || processing.GetID().Equals(curProcessing.GetID()))
                {
                    return curProcessing;
                }
            }            
            this.Add(processing);
            return processing;
        }
        
    }
}
