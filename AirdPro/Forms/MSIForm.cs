using AirdSDK.Bean.Msi;
using AirdSDK.Bean.Msi.HeatMap;
using AirdSDK.Beans;
using AirdSDK.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace AirdPro.Forms
{
    public partial class MSIImageForm : Form
    {  
        private string fileName; //AirdFile
        OpenFileDialog openFileDialog;
        MSIParser msiParser;
        List<ImageData> imageDataList;
        ImageInfo imageInfo;
        
        public MSIImageForm()
        {
            InitializeComponent();            
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AIRD files (*.aird)|*.aird";

            //使用webview2加载html
            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MsiImage/html", "msBarChart.html");
            webViewMS.Source = new Uri(htmlPath);
            htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MsiImage/html", "msiHeatMap.html");
            webViewMSI.Source = new Uri(htmlPath);
        }  

        private void ClearInfo()
        {
            LbRawData.Items.Clear();
            LbImageParams.Items.Clear();
            TbScanNumber.Text = "";
            TbMz.Text = "";
            webViewMS.Reload();
            webViewMSI.Reload();
        }

        private void ShowLbInfo()
        {                      
            msiParser = new MSIParser(Path.ChangeExtension(fileName, ".json"));
            AirdInfo airdInfo = msiParser.airdInfo;
            imageInfo = airdInfo.msiInfo.imageInfo;
            ScanInfo scanInfo = airdInfo.msiInfo.scanInfo;

            //LbRawData            
            LbRawData.Items.Add("  Aird Info");
            LbRawData.Items.Add($"  File Name: {fileName}");
            LbRawData.Items.Add($"  Number of spectra: {airdInfo.totalCount}");
            LbRawData.Items.Add($"  Range m/z: {imageInfo.minMZ}-{imageInfo.maxMZ}");

            //LbImageParams   
            LbImageParams.Items.Add("  Image Params");
            LbImageParams.Items.Add($"  Image dimension [um]: X {imageInfo.maxDimensionX} * Y {imageInfo.maxDimensionY}");
            LbImageParams.Items.Add($"  Total number of pixels: {msiParser.airdInfo.totalCount}");
            LbImageParams.Items.Add($"  Spectra per pixel: {imageInfo.spectraPerPixel}");
            LbImageParams.Items.Add($"  Scan direction: {scanInfo.scanDirection?.ToLower()}");
            LbImageParams.Items.Add($"  Scan sequence: {scanInfo.scanSequence?.ToLower()}");
            LbImageParams.Items.Add($"  Scan pattern: {scanInfo.scanPattern?.ToLower()}");
            LbImageParams.Items.Add($"  Scan type: {scanInfo.scanType?.ToLower()}");           
        }

        private void BtnShowImage_Click(object sender, EventArgs e)
        {
            if(fileName == null)
            {
                MessageBox.Show("please select an aird file first!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(TbMz.Text.Trim().Equals(""))
            {
                MessageBox.Show("please input m/z first!", "message",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            double mz;
            try
            {
                mz = double.Parse(TbMz.Text.Trim());
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
                MessageBox.Show("m/z value is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (mz < imageInfo.minMZ || mz > imageInfo.maxMZ)
            {
                MessageBox.Show("m/z value is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            
            imageDataList = msiParser.GetImageDataList(mz);
            double maxPixelX = imageInfo.maxPixelX;
            double maxPixelY = imageInfo.maxPixelY;
            double maxIntensity = msiParser.maxIntensity;
            
            var heatmapData = imageDataList.Select(data => new object[] { data.X, data.Y, data.Intensity }).ToList();


            string heatmapDataJson = JsonSerializer.Serialize(heatmapData);
            string maxPixelXJson = JsonSerializer.Serialize(maxPixelX);
            string maxPixelYJson = JsonSerializer.Serialize(maxPixelY);
            string maxIntensityJson = JsonSerializer.Serialize(maxIntensity);
            string script = $"drawHeatmap({heatmapDataJson}, {maxPixelXJson}, {maxPixelYJson}, {maxIntensityJson});";
            webViewMSI.ExecuteScriptAsync(script);            
        }

        private void BtnShowMS_Click(object sender, EventArgs e)
        {
            if (fileName == null)
            {
                MessageBox.Show("please select an aird file first!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (TbScanNumber.Text.Trim().Equals(""))
            {
                MessageBox.Show("please input scan number first!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int scanNumber;
            try
            {
                scanNumber = int.Parse(TbScanNumber.Text.Trim());
            } catch (FormatException fe) 
            {
                Console.WriteLine(fe.Message);
                MessageBox.Show("scan number is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }          

            if (scanNumber < 0 || scanNumber > msiParser.airdInfo.totalCount)
            {
                MessageBox.Show("scan number is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            double[] mzArray = msiParser.msList[scanNumber].spectrum.mzs;
            double[] intensityArray = msiParser.msList[scanNumber].spectrum.ints;

            // 执行 JavaScript 代码
            string mzArrayJson = JsonSerializer.Serialize(mzArray);
            string intensityArrayJson = JsonSerializer.Serialize(intensityArray);
            string script = $"drawBarChart({mzArrayJson}, {intensityArrayJson});";
            webViewMS.ExecuteScriptAsync(script);
            

        }

        private void BtnAirdImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.FileName;
                TbAirdFile.Text = openFileDialog.FileName;
                ClearInfo();
                ShowLbInfo();
            }
        }
    }
}
