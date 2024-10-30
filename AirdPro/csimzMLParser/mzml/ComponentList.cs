using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace AirdPro.csimzMLParser.mzml
{
    public class ComponentList : MzMLContentList<Component>
    {
        private readonly List<Source> sources;
        private readonly List<Analyser> analysers;
        private readonly List<Detector> detectors;

        public ComponentList()
        {
            sources = [];
            analysers = [];
            detectors = [];
        }

        public ComponentList(ComponentList componentList, ReferenceableParamGroupList rpgList)
        {
            sources = new List<Source>(componentList.sources.Count);

            foreach (Source source in componentList.sources)
            {
                sources.Add(new Source(source, rpgList));
            }

            analysers = new List<Analyser>(componentList.analysers.Count);

            foreach (Analyser analyser in componentList.analysers)
            {
                analysers.Add(new Analyser(analyser, rpgList));
            }

            detectors = new List<Detector>(componentList.detectors.Count);

            foreach (Detector detector in componentList.detectors)
            {
                detectors.Add(new Detector(detector, rpgList));
            }
        }

        public override void Add(Component component)
        {
            if (component is Source source)
                AddSource(source);
            else if (component is Analyser analyser)
                AddAnalyser(analyser);
            else if (component is Detector detector)
                AddDetector(detector);
        }

        public void AddSource(Source source)
        {
            source.SetParent(this);
            sources.Add(source);
        }

        public void AddAnalyser(Analyser analyser)
        {
            analyser.SetParent(this);
            analysers.Add(analyser);
        }

        public void AddDetector(Detector detector)
        {
            detector.SetParent(this);
            detectors.Add(detector);
        }

        public int SourceCount
        {
            get { return sources.Count; }
        }

        public int AnalyserCount
        {
            get { return analysers.Count; }
        }

        public int DetectorCount
        {
            get { return detectors.Count; }
        }

        public Source GetSource(int index)
        {
            return sources[index];
        }

        public Analyser GetAnalyser(int index)
        {
            return analysers[index];
        }

        public Detector GetDetector(int index)
        {
            return detectors[index];
        }

        public override int Size()
        {
            return sources.Count + analysers.Count + detectors.Count;
        }

        protected virtual void AddTagSpecificElementsAtXPathToCollection(List<IMzMLTag> elements, string fullXPath, string currentXPath)
        {
            if (currentXPath.StartsWith("/source"))
            {
                foreach (Source source in sources)
                {
                    source.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
                }
            }
            else if (currentXPath.StartsWith("/analyzer"))
            {
                foreach (Analyser analyser in analysers)
                {
                    analyser.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
                }
            }
            else if (currentXPath.StartsWith("/detector"))
            {
                foreach (Detector detector in detectors)
                {
                    detector.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
                }
            }
        }

        public override string GetTagName()
        {
            return "componentList";
        }

        public void AddChildrenToCollection(List<IMzMLTag> children)
        {
            children.AddRange(sources);
            children.AddRange(analysers);           
            children.AddRange(detectors);
        }
    }
}
