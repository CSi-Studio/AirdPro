namespace AirdPro.Controls
{
    partial class ChartPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title2 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title3 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title4 = new System.Windows.Forms.DataVisualization.Charting.Title();
            this.ticChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.basePeakChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.spectraGridView = new System.Windows.Forms.DataGridView();
            this.num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precursor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalIons = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.basePeakMz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.basePeakIntensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filterString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spectrumChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.xicChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xicChart)).BeginInit();
            this.SuspendLayout();
            // 
            // ticChart
            // 
            chartArea1.Name = "ChartArea1";
            this.ticChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ticChart.Legends.Add(legend1);
            this.ticChart.Location = new System.Drawing.Point(0, 4);
            this.ticChart.Margin = new System.Windows.Forms.Padding(4);
            this.ticChart.Name = "ticChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ticChart.Series.Add(series1);
            this.ticChart.Size = new System.Drawing.Size(591, 406);
            this.ticChart.TabIndex = 4;
            this.ticChart.Text = "TIC Chart";
            title1.Name = "titleTic";
            title1.Text = "TIC";
            this.ticChart.Titles.Add(title1);
            // 
            // basePeakChart
            // 
            chartArea2.Name = "ChartArea1";
            this.basePeakChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.basePeakChart.Legends.Add(legend2);
            this.basePeakChart.Location = new System.Drawing.Point(0, 418);
            this.basePeakChart.Margin = new System.Windows.Forms.Padding(4);
            this.basePeakChart.Name = "basePeakChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.basePeakChart.Series.Add(series2);
            this.basePeakChart.Size = new System.Drawing.Size(591, 394);
            this.basePeakChart.TabIndex = 5;
            this.basePeakChart.Text = "BasePeak Chart";
            title2.Name = "titleBasePeak";
            title2.Text = "Base Peak";
            this.basePeakChart.Titles.Add(title2);
            // 
            // spectraGridView
            // 
            this.spectraGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spectraGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.spectraGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.num,
            this.level,
            this.rt,
            this.precursor,
            this.totalIons,
            this.basePeakMz,
            this.basePeakIntensity,
            this.filterString});
            this.spectraGridView.Location = new System.Drawing.Point(599, 4);
            this.spectraGridView.Margin = new System.Windows.Forms.Padding(4);
            this.spectraGridView.Name = "spectraGridView";
            this.spectraGridView.RowHeadersWidth = 62;
            this.spectraGridView.RowTemplate.Height = 23;
            this.spectraGridView.Size = new System.Drawing.Size(1190, 808);
            this.spectraGridView.TabIndex = 6;
            // 
            // num
            // 
            this.num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.num.HeaderText = "num";
            this.num.MinimumWidth = 8;
            this.num.Name = "num";
            // 
            // level
            // 
            this.level.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.level.HeaderText = "level";
            this.level.MinimumWidth = 8;
            this.level.Name = "level";
            // 
            // rt
            // 
            this.rt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rt.HeaderText = "rt";
            this.rt.MinimumWidth = 8;
            this.rt.Name = "rt";
            // 
            // precursor
            // 
            this.precursor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.precursor.HeaderText = "precursor";
            this.precursor.MinimumWidth = 8;
            this.precursor.Name = "precursor";
            // 
            // totalIons
            // 
            this.totalIons.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.totalIons.HeaderText = "totalIons";
            this.totalIons.MinimumWidth = 8;
            this.totalIons.Name = "totalIons";
            // 
            // basePeakMz
            // 
            this.basePeakMz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.basePeakMz.HeaderText = "Base Peak m/z";
            this.basePeakMz.MinimumWidth = 8;
            this.basePeakMz.Name = "basePeakMz";
            // 
            // basePeakIntensity
            // 
            this.basePeakIntensity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.basePeakIntensity.HeaderText = "Base Peak Intensity";
            this.basePeakIntensity.MinimumWidth = 8;
            this.basePeakIntensity.Name = "basePeakIntensity";
            // 
            // filterString
            // 
            this.filterString.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.filterString.HeaderText = "filterString";
            this.filterString.MinimumWidth = 8;
            this.filterString.Name = "filterString";
            // 
            // spectrumChart
            // 
            this.spectrumChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            chartArea3.Name = "ChartArea1";
            this.spectrumChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.spectrumChart.Legends.Add(legend3);
            this.spectrumChart.Location = new System.Drawing.Point(0, 819);
            this.spectrumChart.Margin = new System.Windows.Forms.Padding(4);
            this.spectrumChart.Name = "spectrumChart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.spectrumChart.Series.Add(series3);
            this.spectrumChart.Size = new System.Drawing.Size(887, 376);
            this.spectrumChart.TabIndex = 7;
            this.spectrumChart.Text = "Spectrum Chart";
            title3.Name = "titleSpectrum";
            title3.Text = "Spectrum";
            this.spectrumChart.Titles.Add(title3);
            // 
            // xicChart
            // 
            this.xicChart.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea4.Name = "ChartArea1";
            this.xicChart.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.xicChart.Legends.Add(legend4);
            this.xicChart.Location = new System.Drawing.Point(894, 818);
            this.xicChart.Margin = new System.Windows.Forms.Padding(4);
            this.xicChart.Name = "xicChart";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.xicChart.Series.Add(series4);
            this.xicChart.Size = new System.Drawing.Size(895, 376);
            this.xicChart.TabIndex = 8;
            this.xicChart.Text = "XIC Chart";
            title4.Name = "titleXic";
            title4.Text = "XIC";
            this.xicChart.Titles.Add(title4);
            // 
            // ChartPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ticChart);
            this.Controls.Add(this.basePeakChart);
            this.Controls.Add(this.spectraGridView);
            this.Controls.Add(this.spectrumChart);
            this.Controls.Add(this.xicChart);
            this.Name = "ChartPanel";
            this.Size = new System.Drawing.Size(1800, 1200);
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xicChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ticChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart basePeakChart;
        private System.Windows.Forms.DataGridView spectraGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn num;
        private System.Windows.Forms.DataGridViewTextBoxColumn level;
        private System.Windows.Forms.DataGridViewTextBoxColumn rt;
        private System.Windows.Forms.DataGridViewTextBoxColumn precursor;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalIons;
        private System.Windows.Forms.DataGridViewTextBoxColumn basePeakMz;
        private System.Windows.Forms.DataGridViewTextBoxColumn basePeakIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn filterString;
        private System.Windows.Forms.DataVisualization.Charting.Chart spectrumChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart xicChart;
    }
}
