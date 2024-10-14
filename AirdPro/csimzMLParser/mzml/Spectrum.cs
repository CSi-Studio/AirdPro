using AirdPro.csimzMLParser.exceptions;
using AirdPro.csimzMLParser.imzml;
using AirdPro.csimzMLParser.obo;
using AirdPro.csimzMLParser.util;
using HZH_Controls;
using System;
using System.Collections.Generic;
using static AirdPro.csimzMLParser.mzml.BinaryDataArray;

namespace AirdPro.csimzMLParser.mzml
{
    public class Spectrum : MzMLDataContainer
    {
        private static readonly long serialVersionUID = 1L;

        public static readonly string SCAN_POLARITY_ID = "MS:1000465";
        public static readonly string SPECTRUM_TYPE_ID = "MS:1000559";
        public static readonly string SPECTRUM_REPRESENTATION_ID = "MS:1000525";
        public static readonly string SPECTRUM_ATTRIBUTE_ID = "MS:1000499";
        public static readonly string TOTAL_ION_CURRENT_ID = "MS:1000285";
        public static readonly string BASE_PEAK_MZ_ID = "MS:1000504";
        public static readonly string BASE_PEAK_INTENSITY_ID = "MS:1000505";
        public static readonly string LOWEST_OBSERVED_MZ_ID = "MS:1000528";
        public static readonly string HIGHEST_OBSERVED_MZ_ID = "MS:1000527";
        public static readonly string MS1_SPECTRUM_ID = "MS:1000579";
        public static readonly string POSITIVE_SCAN_ID = "MS:1000130";
        public static readonly string NEGATIVE_SCAN_ID = "MS:1000129";
        public static readonly string PROFILE_SPECTRUM_ID = "MS:1000128";
        public static readonly string CENTROID_SPECTRUM_ID = "MS:1000127";
        public static readonly string MS_lEVEL_ID = "MS:1000511";

        protected static int spectrumNumber = 0;

        public SourceFile sourceFileRef;
        public string spotID;
        public ScanList scanList;
        public PrecursorList precursorList;
        public ProductList productList;
        public PixelLocation pixelLocation;

        public Spectrum(string id, int defaultArrayLength) : base(id, defaultArrayLength)
        {

        }

        public Spectrum(Spectrum spectrum, MzML mzML) : this(spectrum, mzML.referenceableParamGroupList, mzML.dataProcessingList, mzML.fileDescription.sourceFileList, mzML.instrumentConfigurationList)
        {

        }

        public Spectrum(Spectrum spectrum, ReferenceableParamGroupList rpgList, DataProcessingList dpList, SourceFileList sourceFileList, InstrumentConfigurationList icList) : base(spectrum, rpgList, dpList)
        {
            this.id = spectrum.id;
            this.spotID = spectrum.spotID;

            if (spectrum.sourceFileRef != null && sourceFileList != null)
            {
                foreach(SourceFile sourceFile in sourceFileList)
                {
                    if (spectrum.sourceFileRef.GetID().Equals(sourceFile.GetID()))
                    {
                        sourceFileRef = sourceFile;
                        break;
                    }
                }
            }

            if (spectrum.scanList != null)
            {
                scanList = new ScanList(spectrum.scanList, rpgList, sourceFileList, icList);
            }
            if (spectrum.precursorList != null)
            {
                this.precursorList = new PrecursorList(spectrum.precursorList, rpgList, sourceFileList);
            }
            if (spectrum.productList != null)
            {
                this.productList = new ProductList(spectrum.productList, rpgList);
            }
        }

        public bool IsCentroid()
        {
            var centroidSpectrumCVParam = GetCVParam(CENTROID_SPECTRUM_ID);
            return centroidSpectrumCVParam != null;
        }

        public bool IsProfile()
        {
            var profileSpectrumCVParam = GetCVParam(PROFILE_SPECTRUM_ID);
            return profileSpectrumCVParam != null;
        }

        public void SetSourceFileRef(SourceFile sourceFileRef)
        {
            this.sourceFileRef = sourceFileRef;
        }

        public void SetSpotID(string spotID)
        {
            this.spotID = spotID;
        }

        public ScanList GetScanList()
        {
            return scanList;
        }

        public void SetScanList(ScanList scanList)
        {
            scanList.SetParent(this);
            this.scanList = scanList;
        }

        public void SetPrecursorList(PrecursorList precursorList)
        {
            precursorList.SetParent(this);
            this.precursorList = precursorList;
        }

        public PrecursorList GetPrecursorList()
        {
            return precursorList;
        }

        public void SetProductList(ProductList productList)
        {
            productList.SetParent(this);
            this.productList = productList;
        }

        public ProductList GetProductList()
        {
            return productList;
        }

        public PixelLocation GetPixelLocation()
        {
            if (pixelLocation == null)
            {
                foreach (Scan scan in scanList)
                {
                    CVParam xValue = scan.GetCVParam(Scan.POSITION_X_ID);
                    CVParam yValue = scan.GetCVParam(Scan.POSITION_Y_ID);
                    CVParam zValue = scan.GetCVParam(Scan.POSITION_Z_ID);

                    if (xValue != null && yValue != null)
                    {
                        int x = xValue.GetValueAsInteger();
                        int y = yValue.GetValueAsInteger();
                        int z = zValue?.GetValueAsInteger() ?? 1;

                        pixelLocation = new PixelLocation(x, y, z);
                        break;
                    }
                }
            }

            return pixelLocation;
        }

        public void SetPixelLocation(PixelLocation location)
        {
            if (scanList == null)
            {
                scanList = ScanList.Create();
            }

            Scan scan = scanList.Get(0);

            scan.RemoveCVParam(Scan.POSITION_X_ID);
            scan.RemoveCVParam(Scan.POSITION_Y_ID);
            scan.RemoveCVParam(Scan.POSITION_Z_ID);

            scan.AddCVParam(new IntegerCVParam(OBO.GetOBO().GetTerm(Scan.POSITION_X_ID), location.x));
            scan.AddCVParam(new IntegerCVParam(OBO.GetOBO().GetTerm(Scan.POSITION_Y_ID), location.y));

            if (location.z >= 1)
            {
                scan.AddCVParam(new IntegerCVParam(OBO.GetOBO().GetTerm(Scan.POSITION_Z_ID), location.z));
            }

            pixelLocation = location;
        }

        public void SetPixelLocation(int x, int y)
        {
            SetPixelLocation(new PixelLocation(x, y, -1));
        }

        public void SetPixelLocation(int x, int y, int z)
        {
            SetPixelLocation(new PixelLocation(x, y, z));
        }

        public override void AddTagSpecificElementsAtXPathToCollection(ICollection<IMzMLTag> elements, String fullXPath, String currentXPath) 
        {
            if (currentXPath.StartsWith("/scanList")) 
            {
                if (scanList == null) {
                    throw new UnfollowableXPathException("No scanList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                scanList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            } 
            else if (currentXPath.StartsWith("/precursorList"))
            {
                if (precursorList == null)
                {
                throw new UnfollowableXPathException("No precursorList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
            }
            precursorList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
        }
        else if (currentXPath.StartsWith("/productList"))
        {
            if (productList == null)
            {
                throw new UnfollowableXPathException("No productList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
            }

            productList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
            else if (currentXPath.StartsWith("/binaryDataArrayList"))
            {
                if (binaryDataArrayList == null)
                {
                    throw new UnfollowableXPathException("No binaryDataArrayList exists, so cannot go to " + fullXPath, fullXPath, currentXPath);
                }

                binaryDataArrayList.AddElementsAtXPathToCollection(elements, fullXPath, currentXPath);
            }
        }

        public override string GetXMLAttributeText()
        {
            string attributeText = base.GetXMLAttributeText();

            if (sourceFileRef != null)
            {
                attributeText += $" sourceFileRef=\"{XMLHelper.EnsureSafeXML(sourceFileRef.id)}\"";
            }
            if (spotID != null)
            {
                attributeText += $" spotID=\"{XMLHelper.EnsureSafeXML(spotID)}\"";
            }
            return attributeText;
        }

        public override string ToString()
        {
            return $"spectrum: " +
                $"id=\"{id}\" " +
                $"{(dataProcessingRef != null ? $"dataProcessingRef=\"{dataProcessingRef.id}\" " : "")} " +
                $"defaultArrayLength=\"{defaultArrayLength}\"" +
                $" {(sourceFileRef != null ? $"sourceFileRef=\"{sourceFileRef.id}\" " : "")} " +
                $"{(spotID != null && !spotID.IsEmpty() ? $"spotID=\"{spotID}\"" : "")}";
        }

        public override string GetTagName()
        {
            return "spectrum";
        }

        public override void AddChildrenToCollection(ICollection<IMzMLTag> children)
        {
            base.AddChildrenToCollection(children);

            if (scanList != null)
            {
                children.Add(scanList);
            }
            if (precursorList != null)
            {
                children.Add(precursorList);
            }
            if (productList != null)
            {
                children.Add(productList);
            }
            if (binaryDataArrayList != null)
            {
                children.Add(binaryDataArrayList);
            }
        }

        public double[] GetMzArray()
        {
            return GetMzArray(false);
        }

        public double[] GetMzArray(bool keepInMemory)
        {
            if (binaryDataArrayList == null)
            {
                return [];
            }
            EnsureLoadableData();
            return binaryDataArrayList.GetMzArray().GetDataAsDouble(keepInMemory);
        }

        protected void SetMzArray(double[] mzs)
        {
            binaryDataArrayList.GetMzArray().SetData(mzs);
        }

        protected void SetIntensityArray(double[] intensities)
        {
            binaryDataArrayList.GetIntensityArray().SetData(intensities);
        }
        protected void SetSpectralData(double[] mzs, double[] intensities)
        {
            SetMzArray(mzs);
            SetIntensityArray(intensities);
        }

        protected void UpdateDataProcessing(DataProcessing processing)
        {
            DataProcessing previousProcessing = this.dataProcessingRef;
            string newID = "";

            if (previousProcessing != null)
                newID = previousProcessing.id + "-";

            DataProcessing newProcessing = new (newID + processing.id);

            foreach (ProcessingMethod method in previousProcessing)
            {
                newProcessing.Add(method);
            }
            foreach (ProcessingMethod method in processing)
            {
                newProcessing.Add(method);
            }
            this.SetDataProcessingRef(newProcessing);
        }

        public void UpdatemzArray(double[] mzs, DataProcessing processing)
        {
            UpdateDataProcessing(processing);
            SetMzArray(mzs);
        }

        public void UpdateIntensityArray(double[] intensities, DataProcessing processing)
        {
            UpdateDataProcessing(processing);
            SetIntensityArray(intensities);
        }

        public void UpdateSpectralData(double[] mzs, double[] intensities, DataProcessing processing)
        {
            UpdateDataProcessing(processing);
            SetMzArray(mzs);
            SetIntensityArray(intensities);
        }

        private static Spectrum CreateSpectrum(double[] mzs, double[] intensities)
        {
            return CreateSpectrum(mzs, intensities, DataProcessing.Create());
        }

        private static Spectrum CreateSpectrum(double[] mzs, double[] intensities, DataProcessing processing)
        {
            string id = "spectrum=" + spectrumNumber++;

            Spectrum spectrum = new Spectrum(id, intensities.Length);
            spectrum.SetDataProcessingRef(processing);

            spectrum.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(MS1_SPECTRUM_ID)));
            spectrum.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(PROFILE_SPECTRUM_ID)));

            BinaryDataArray mzsDataArray = new BinaryDataArray(mzs.Length);
            mzsDataArray.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(MZ_ARRAY_ID), 
                OBO.GetOBO().GetTerm(MZ_ARRAY_UNITS_ID)));
            mzsDataArray.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(NO_COMPRESSION_ID)));
            mzsDataArray.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(DOUBLE_PRECISION_ID)));

            BinaryDataArray intensitiesDataArray = new BinaryDataArray(intensities.Length);
            intensitiesDataArray.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(INTENSITY_ARRAY_ID)));
            intensitiesDataArray.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(NO_COMPRESSION_ID)));
            intensitiesDataArray.AddCVParam(new EmptyCVParam(OBO.GetOBO().GetTerm(DOUBLE_PRECISION_ID)));

            spectrum.binaryDataArrayList.AddBinaryDataArray(mzsDataArray);
            spectrum.binaryDataArrayList.AddBinaryDataArray(intensitiesDataArray);

            spectrum.SetSpectralData(mzs, intensities);

            return spectrum;
        }

        public static Spectrum CreateSpectrum(double[] mzs, double[] intensities, int x, int y)
        {
            Spectrum spectrum = CreateSpectrum(mzs, intensities, DataProcessing.Create());
            spectrum.SetPixelLocation(x, y);
            return spectrum;
        }

        public static Spectrum CreateSpectrum(double[] mzs, double[] intensities, DataProcessing processing, int x, int y)
        {
            Spectrum spectrum = CreateSpectrum(mzs, intensities, processing);
            spectrum.SetPixelLocation(x, y);
            return spectrum;
        }
    }
}
