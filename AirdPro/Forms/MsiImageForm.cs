using AirdSDK.Bean.Msi;
using AirdSDK.Bean.Msi.HeatMap;
using AirdSDK.Beans;
using AirdSDK.Parser;
using HZH_Controls.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
            imageInfo = airdInfo.msiInfo.imageInfo;
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
            
            /*double[,] intensityMatrix = msiParser.GetIntensityMatrix(mz);
            for(int i = 0; i < intensityMatrix.GetLength(0); i++)
            {
                for(int j = 0; j < intensityMatrix.GetLength(1); j++)
                {
                    if(intensityMatrix[i,j] > 0)
                    {
                        intensityMatrix[i,j] = Math.Log10(intensityMatrix[i,j]);
                    }
                }
            }*/

            imageDataList = msiParser.GetImageDatas(mz);

            /*var sw = new Stopwatch();
            sw.Start();
            int h = (int)Math.Round(imageInfo.pixelSizeX);
            int w = (int)Math.Round(imageInfo.pixelSizeY);
            PbMsiImage.Image = GetImage(w, h, imageDataList.Count);
            sw.Stop();*/

            DgvImage.ColumnCount = imageInfo.maxPixelY;
            DgvImage.RowCount = imageInfo.maxPixelX;

            // 填充 DataGridView
            foreach (var data in imageDataList)
            {
                DgvImage.Rows[data.Y - 1].Cells[data.X - 1].Value = data.Intensity;
                DgvImage.Rows[data.Y - 1].Cells[data.X - 1].Style.BackColor = GetHeatmapColor(data.Intensity);
            }

            // 订阅单元格绘制事件
            DgvImage.CellPainting += DataGridView_CellPainting;
        }

        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            // 绘制单元格背景色
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var intensity = DgvImage.Rows[e.RowIndex].Cells[e.ColumnIndex].Value as double?;
                if (intensity.HasValue)
                {
                    e.Graphics.FillRectangle(new SolidBrush(GetHeatmapColor(intensity.Value)), e.CellBounds);
                    e.Handled = true; // 阻止默认的单元格绘制
                }
            }
        }

        private Color GetHeatmapColor(double intensity)
        {
            //double range = msiParser.maxIntensity / 255;
            // 根据强度值返回不同的颜色
            if (intensity <= 1.0)
                return Color.Green;
            else if (intensity <= 2.0)
                return Color.Yellow;
            else if (intensity <= 3.0)
                return Color.Orange;
            else if (intensity <= 4.0)
                return Color.Red;
            else
                return Color.DarkRed;
        }

    }
}
