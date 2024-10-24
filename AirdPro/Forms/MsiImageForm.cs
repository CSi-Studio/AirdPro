using AirdSDK.Bean.Msi;
using AirdSDK.Beans;
using AirdSDK.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;


namespace AirdPro.Forms
{
    public partial class MSIImageForm : Form
    {  
        private string fileName; //AirdFile
        OpenFileDialog openFileDialog;
        MSIParser msiParser;

        private List<string> messages = new();
        public List<string> Messages
        {
            get { return messages; }
        }
        public MSIImageForm()
        {
            InitializeComponent();
            openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "AIRD files (*.aird)|*.aird";
        }        

        private void BtnSelect_Click(object sender, EventArgs e)
        {            
            if (openFileDialog.ShowDialog() == DialogResult.OK) 
            {
                fileName = openFileDialog.FileName;
                TbAirdFile.Text = openFileDialog.FileName;
                ShowParamInfo();
            }
        }

        private void ShowParamInfo()
        {           
            msiParser = new MSIParser(Path.ChangeExtension(fileName, ".json"));
            AirdInfo airdInfo = msiParser.airdInfo;
            ImageInfo imageInfo = airdInfo.msiInfo.imageInfo;
            ScanInfo scanInfo = airdInfo.msiInfo.scanInfo;
            LbParams.Items.Clear();
            LbParams.Items.Add($"Range m/z: {imageInfo.minMZ}-{imageInfo.maxMZ}");           
            LbParams.Items.Add($"Image dimension [um]: X {imageInfo.maxPixelX} * Y {imageInfo.maxPixelY}");
            LbParams.Items.Add($"Total number of pixels: {msiParser.airdInfo.totalCount}");
            LbParams.Items.Add($"Spectra per pixel: {imageInfo.spectraPerPixel}");
            LbParams.Items.Add($"Scan direction: {scanInfo.scanDirection?.ToLower()}");
            LbParams.Items.Add($"Scan sequence: {scanInfo.scanSequence?.ToLower()}");
            LbParams.Items.Add($"Scan pattern: {scanInfo.scanPattern?.ToLower()}");
            LbParams.Items.Add($"Scan type: {scanInfo.scanType?.ToLower()}");
        }

        private void BtnShowImage_Click(object sender, EventArgs e)
        {
            if(fileName == null)
            {
                MessageBox.Show("please select an aird file first!", "message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if(TbMZ.Text.Trim().Equals(""))
            {
                MessageBox.Show("please input m/z first!", "message",MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }            
            double mz = double.Parse(TbMZ.Text.Trim());            
            double[,] intensityMatrix = msiParser.GetIntensityMatrix(mz);

           

            

        }

    }
}
