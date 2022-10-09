namespace AirdPro.Controls
{
    partial class AirdPanel
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
            this.mainLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ticAndBasePeakContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.ticChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.basePeakChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.spectraContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.spectrumAndXicContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.spectrumChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.xicChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.spectraGridView = new System.Windows.Forms.DataGridView();
            this.num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.precursor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.totalIons = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.basePeakMz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.basePeakIntensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.filterString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainLayoutPanel.SuspendLayout();
            this.ticAndBasePeakContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).BeginInit();
            this.spectraContainer.SuspendLayout();
            this.spectrumAndXicContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xicChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.Controls.Add(this.ticAndBasePeakContainer);
            this.mainLayoutPanel.Controls.Add(this.spectraContainer);
            this.mainLayoutPanel.Controls.Add(this.spectrumAndXicContainer);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.Size = new System.Drawing.Size(1200, 800);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // ticAndBasePeakContainer
            // 
            this.ticAndBasePeakContainer.Controls.Add(this.ticChart);
            this.ticAndBasePeakContainer.Controls.Add(this.basePeakChart);
            this.ticAndBasePeakContainer.Location = new System.Drawing.Point(3, 3);
            this.ticAndBasePeakContainer.Name = "ticAndBasePeakContainer";
            this.ticAndBasePeakContainer.Size = new System.Drawing.Size(417, 481);
            this.ticAndBasePeakContainer.TabIndex = 0;
            // 
            // ticChart
            // 
            chartArea1.Name = "ChartArea1";
            this.ticChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.ticChart.Legends.Add(legend1);
            this.ticChart.Location = new System.Drawing.Point(3, 3);
            this.ticChart.Name = "ticChart";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.ticChart.Series.Add(series1);
            this.ticChart.Size = new System.Drawing.Size(414, 245);
            this.ticChart.TabIndex = 0;
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
            this.basePeakChart.Location = new System.Drawing.Point(3, 254);
            this.basePeakChart.Name = "basePeakChart";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.basePeakChart.Series.Add(series2);
            this.basePeakChart.Size = new System.Drawing.Size(414, 236);
            this.basePeakChart.TabIndex = 1;
            this.basePeakChart.Text = "chart2";
            title2.Name = "titleBasePeak";
            title2.Text = "Base Peak";
            this.basePeakChart.Titles.Add(title2);
            // 
            // spectraContainer
            // 
            this.spectraContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.spectraContainer.Controls.Add(this.spectraGridView);
            this.spectraContainer.Location = new System.Drawing.Point(426, 3);
            this.spectraContainer.Name = "spectraContainer";
            this.spectraContainer.Size = new System.Drawing.Size(771, 481);
            this.spectraContainer.TabIndex = 1;
            // 
            // spectrumAndXicContainer
            // 
            this.spectrumAndXicContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spectrumAndXicContainer.Controls.Add(this.spectrumChart);
            this.spectrumAndXicContainer.Controls.Add(this.xicChart);
            this.spectrumAndXicContainer.Location = new System.Drawing.Point(3, 490);
            this.spectrumAndXicContainer.Name = "spectrumAndXicContainer";
            this.spectrumAndXicContainer.Size = new System.Drawing.Size(1194, 307);
            this.spectrumAndXicContainer.TabIndex = 0;
            // 
            // spectrumChart
            // 
            chartArea3.Name = "ChartArea1";
            this.spectrumChart.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.spectrumChart.Legends.Add(legend3);
            this.spectrumChart.Location = new System.Drawing.Point(3, 3);
            this.spectrumChart.Name = "spectrumChart";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.spectrumChart.Series.Add(series3);
            this.spectrumChart.Size = new System.Drawing.Size(559, 304);
            this.spectrumChart.TabIndex = 2;
            this.spectrumChart.Text = "Spectrum Chart";
            title3.Name = "titleSpectrum";
            title3.Text = "Spectrum";
            this.spectrumChart.Titles.Add(title3);
            // 
            // xicChart
            // 
            chartArea4.Name = "ChartArea1";
            this.xicChart.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.xicChart.Legends.Add(legend4);
            this.xicChart.Location = new System.Drawing.Point(568, 3);
            this.xicChart.Name = "xicChart";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.xicChart.Series.Add(series4);
            this.xicChart.Size = new System.Drawing.Size(623, 304);
            this.xicChart.TabIndex = 3;
            this.xicChart.Text = "XIC Chart";
            title4.Name = "titleXic";
            title4.Text = "XIC";
            this.xicChart.Titles.Add(title4);
            // 
            // spectraGridView
            // 
            this.spectraGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
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
            this.spectraGridView.Location = new System.Drawing.Point(3, 3);
            this.spectraGridView.Name = "spectraGridView";
            this.spectraGridView.RowTemplate.Height = 23;
            this.spectraGridView.Size = new System.Drawing.Size(765, 478);
            this.spectraGridView.TabIndex = 0;
            // 
            // num
            // 
            this.num.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.num.HeaderText = "num";
            this.num.Name = "num";
            // 
            // level
            // 
            this.level.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.level.HeaderText = "level";
            this.level.Name = "level";
            // 
            // rt
            // 
            this.rt.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.rt.HeaderText = "rt";
            this.rt.Name = "rt";
            // 
            // precursor
            // 
            this.precursor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.precursor.HeaderText = "precursor";
            this.precursor.Name = "precursor";
            // 
            // totalIons
            // 
            this.totalIons.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.totalIons.HeaderText = "totalIons";
            this.totalIons.Name = "totalIons";
            // 
            // basePeakMz
            // 
            this.basePeakMz.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.basePeakMz.HeaderText = "Base Peak m/z";
            this.basePeakMz.Name = "basePeakMz";
            // 
            // basePeakIntensity
            // 
            this.basePeakIntensity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.basePeakIntensity.HeaderText = "Base Peak Intensity";
            this.basePeakIntensity.Name = "basePeakIntensity";
            // 
            // filterString
            // 
            this.filterString.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.filterString.HeaderText = "filterString";
            this.filterString.Name = "filterString";
            // 
            // AirdPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayoutPanel);
            this.Name = "AirdPanel";
            this.Size = new System.Drawing.Size(1200, 800);
            this.mainLayoutPanel.ResumeLayout(false);
            this.ticAndBasePeakContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).EndInit();
            this.spectraContainer.ResumeLayout(false);
            this.spectrumAndXicContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xicChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.FlowLayoutPanel ticAndBasePeakContainer;
        private System.Windows.Forms.FlowLayoutPanel spectraContainer;
        private System.Windows.Forms.FlowLayoutPanel spectrumAndXicContainer;
        private System.Windows.Forms.DataVisualization.Charting.Chart ticChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart basePeakChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart spectrumChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart xicChart;
        private System.Windows.Forms.DataGridView spectraGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn num;
        private System.Windows.Forms.DataGridViewTextBoxColumn level;
        private System.Windows.Forms.DataGridViewTextBoxColumn rt;
        private System.Windows.Forms.DataGridViewTextBoxColumn precursor;
        private System.Windows.Forms.DataGridViewTextBoxColumn totalIons;
        private System.Windows.Forms.DataGridViewTextBoxColumn basePeakMz;
        private System.Windows.Forms.DataGridViewTextBoxColumn basePeakIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn filterString;
    }
}
