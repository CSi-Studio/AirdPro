using AirdPro.Utils;
using AirdSDK.Bean.Msi;
using AirdSDK.Beans;
using AirdSDK.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AirdPro.Forms
{
    public partial class MSIImageForm : Form
    {
        readonly OpenFileDialog openFileDialog;
        MSIMaldiParser msiMaldiParser;
        List<ImageData> imageDataList;
        double mz, tolerance;

        public MSIImageForm()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog
            {
                Filter = "AIRD files (*.aird)|*.aird"
            };

            //使用webview2加载html
            string htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MsiImage/html", "msBarChart.html");
            webViewMS.Source = new Uri(htmlPath);
            htmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MsiImage/html", "msiHeatMap.html");
            webViewMSI.Source = new Uri(htmlPath);

        }

        private void ClearInfo()
        {
            TbScanNumber.Text = "0";
            TbMz.Text = "";
            TbPPM.Text = "";
            TbDAL.Text = "";
            BtnShowMS.Enabled = false;
            BtnShowImage.Enabled = false;
            LbAirdInfo.Items.Clear();
            webViewMS.Reload();
            webViewMSI.Reload();
        }

        private void BtnShowImage_Click(object sender, EventArgs e)
        {
            if (TbAirdFile.Text.Trim().Equals(""))
            {
                MessageBox.Show("please select an aird file first!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (TbMz.Text.Trim().Equals(""))
            {
                MessageBox.Show("please input m/z first!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (TbPPM.Text.Trim().Equals("") && TbDAL.Text.Trim().Equals(""))
            {
                MessageBox.Show("please input m/z tolerance first!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

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
            try
            {
                tolerance = double.Parse(TbDAL.Text.Trim());
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
                MessageBox.Show("m/z tolerance value is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ImageInfo imageInfo = msiMaldiParser.airdInfo.msiInfo.imageInfo;
            if (mz < imageInfo.minMZ || mz > imageInfo.maxMZ)
            {
                MessageBox.Show("m/z value is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            imageDataList = msiMaldiParser.GetImageDataList(mz, tolerance);
            var heatmapData = imageDataList.Select(data => new object[] { data.X, data.Y, data.Intensity }).ToList();
            double maxIntensity = GetMaxIntensity(imageDataList);
            double maxPixelX = imageInfo.maxPixelX;
            double maxPixelY = imageInfo.maxPixelY;

            string heatmapDataJson = JsonSerializer.Serialize(heatmapData);
            string maxIntensityJson = JsonSerializer.Serialize(maxIntensity);
            string maxPixelXJson = JsonSerializer.Serialize(maxPixelX);
            string maxPixelYJson = JsonSerializer.Serialize(maxPixelY);
            string script = $"drawHeatmap({heatmapDataJson}, {maxIntensityJson}, {maxPixelXJson}, {maxPixelYJson});";
            webViewMSI.ExecuteScriptAsync(script);
        }

        private double GetMaxIntensity(List<ImageData> imageDataList)
        {
            double maxIntensity = 0;
            foreach (ImageData imageData in imageDataList)
            {
                if (imageData.Intensity > maxIntensity)
                {
                    maxIntensity = imageData.Intensity;
                }
            }
            return maxIntensity;
        }

        private void BtnShowMS_Click(object sender, EventArgs e)
        {
            if (TbAirdFile.Text.Trim().Equals(""))
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
            }
            catch (FormatException fe)
            {
                Console.WriteLine(fe.Message);
                MessageBox.Show("scan number is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (scanNumber < 0 || scanNumber >= msiMaldiParser.airdInfo.totalCount)
            {
                MessageBox.Show("scan number is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            double[] mzArray = msiMaldiParser.msList[scanNumber].spectrum.mzs;
            double[] intensityArray = msiMaldiParser.msList[scanNumber].spectrum.ints;

            // 执行 JavaScript 代码
            string mzArrayJson = JsonSerializer.Serialize(mzArray);
            string intensityArrayJson = JsonSerializer.Serialize(intensityArray);
            string script = $"drawBarChart({mzArrayJson}, {intensityArrayJson});";
            webViewMS.ExecuteScriptAsync(script);
        }

        private async void BtnAirdImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string airdFile = openFileDialog.FileName;
                TbAirdFile.Text = airdFile;
                ClearInfo();

                LbAirdInfo.Items.Add("  Importing and parsing file, please wait...");
                DateTime startTime = DateTime.Now;
                await Task.Run(() => ImportAirdFile(airdFile));
                DateTime endTime = DateTime.Now;
                LbAirdInfo.Items.Add($"  The file was successfully imported and took {(endTime - startTime).TotalSeconds} s!");

                ShowAirdInfo(airdFile);
                BtnShowMS.Enabled = true;
                BtnShowImage.Enabled = true;
            }
        }

        private void ImportAirdFile(string airdFile)
        {
            msiMaldiParser = new MSIMaldiParser(Path.ChangeExtension(airdFile, ".json"));
        }



        private void ShowAirdInfo(string airdFile)
        {
            AirdInfo airdInfo = msiMaldiParser.airdInfo;
            ImageInfo imageInfo = airdInfo.msiInfo.imageInfo;
            ScanInfo scanInfo = airdInfo.msiInfo.scanInfo;

            // LbAirdInfo
            LbAirdInfo.Items.Add("");
            LbAirdInfo.Items.Add("  Basic Info");
            LbAirdInfo.Items.Add($"  Aird File Name: {airdFile}");
            LbAirdInfo.Items.Add($"  Aird File Size: {DataUtil.FormatFileSize(msiMaldiParser.airdFile.Length)}");
            LbAirdInfo.Items.Add($"  Aird Type: {airdInfo.type}");
            LbAirdInfo.Items.Add($"  Source File Size: {DataUtil.FormatFileSize(airdInfo.fileSize)}");

            LbAirdInfo.Items.Add("");
            LbAirdInfo.Items.Add("  Image Params");
            LbAirdInfo.Items.Add($"  File Organisation : {airdInfo.msiInfo.fileOrganisation?.ToLower()}");
            LbAirdInfo.Items.Add($"  Number of scans: {airdInfo.totalCount}");
            LbAirdInfo.Items.Add($"  Range m/z: {imageInfo.minMZ}-{imageInfo.maxMZ}");
            LbAirdInfo.Items.Add($"  Max count of pixels x: {imageInfo.maxPixelX}");
            LbAirdInfo.Items.Add($"  Max count of pixels y: {imageInfo.maxPixelY}");
            LbAirdInfo.Items.Add($"  Max count of pixels z: {imageInfo.maxPixelZ}");

            LbAirdInfo.Items.Add($"  Pixel size: Xaxis {imageInfo.pixelSizeX}, Yaxis {imageInfo.pixelSizeY}");
            LbAirdInfo.Items.Add($"  Image dimension [um]: X {imageInfo.pixelSizeX * imageInfo.maxPixelX} * Y {imageInfo.pixelSizeY * imageInfo.maxPixelY}");
            LbAirdInfo.Items.Add($"  Total number of pixels: {airdInfo.msiInfo.spectraPosition.x.Length}");
            LbAirdInfo.Items.Add($"  Spectra per pixel: {imageInfo.spectraPerPixel}");
            LbAirdInfo.Items.Add($"  Scan direction: {scanInfo.scanDirection?.ToLower()}");
            LbAirdInfo.Items.Add($"  Scan sequence: {scanInfo.scanSequence?.ToLower()}");
            LbAirdInfo.Items.Add($"  Scan pattern: {scanInfo.scanPattern?.ToLower()}");
            LbAirdInfo.Items.Add($"  Scan type: {scanInfo.scanType?.ToLower()}");
        }        

        private void TbPPM_Leave(object sender, EventArgs e)
        {
            if (TbPPM.Text.Trim().Equals(""))
            {
                return;
            }
            if (!TbMz.Text.Trim().Equals(""))
            {
                try
                {
                    double mz = Convert.ToDouble(TbMz.Text.Trim());
                    double ppm = Convert.ToDouble(TbPPM.Text.Trim());
                    double dal = mz * ppm * Math.Pow(10, -6);
                    TbDAL.Text = dal.ToString();
                }
                catch (FormatException fe)
                {
                    Console.WriteLine(fe.Message);
                    MessageBox.Show("ppm value is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }

        private void TbDAL_Leave(object sender, EventArgs e)
        {
            if (TbDAL.Text.Trim().Equals(""))
            {
                return;
            }
            if (!TbMz.Text.Trim().Equals(""))
            {
                try
                {
                    double mz = Convert.ToDouble(TbMz.Text.Trim());
                    double dal = Convert.ToDouble(TbDAL.Text.Trim());
                    double ppm = Math.Round(dal * Math.Pow(10, 6) / mz);
                    TbPPM.Text = ppm.ToString();
                }
                catch (FormatException fe)
                {
                    Console.WriteLine(fe.Message);
                    MessageBox.Show("dal value is invalid!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }
        }
     
    }
}
