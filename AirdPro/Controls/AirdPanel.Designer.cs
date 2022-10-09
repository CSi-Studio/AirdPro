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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea5 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend5 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title5 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea6 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend6 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title6 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea7 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend7 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series7 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title7 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea8 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend8 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series8 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title8 = new System.Windows.Forms.DataVisualization.Charting.Title();
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
            this.mainLayout = new System.Windows.Forms.SplitContainer();
            this.topLayout = new System.Windows.Forms.SplitContainer();
            this.ticAndBasePeakLayout = new System.Windows.Forms.SplitContainer();
            this.bottomLayout = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xicChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).BeginInit();
            this.mainLayout.Panel1.SuspendLayout();
            this.mainLayout.Panel2.SuspendLayout();
            this.mainLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topLayout)).BeginInit();
            this.topLayout.Panel1.SuspendLayout();
            this.topLayout.Panel2.SuspendLayout();
            this.topLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ticAndBasePeakLayout)).BeginInit();
            this.ticAndBasePeakLayout.Panel1.SuspendLayout();
            this.ticAndBasePeakLayout.Panel2.SuspendLayout();
            this.ticAndBasePeakLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bottomLayout)).BeginInit();
            this.bottomLayout.Panel1.SuspendLayout();
            this.bottomLayout.Panel2.SuspendLayout();
            this.bottomLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // ticChart
            // 
            chartArea5.Name = "ChartArea1";
            this.ticChart.ChartAreas.Add(chartArea5);
            legend5.Name = "Legend1";
            this.ticChart.Legends.Add(legend5);
            this.ticChart.Location = new System.Drawing.Point(4, 4);
            this.ticChart.Margin = new System.Windows.Forms.Padding(4);
            this.ticChart.Name = "ticChart";
            series5.ChartArea = "ChartArea1";
            series5.Legend = "Legend1";
            series5.Name = "Series1";
            this.ticChart.Series.Add(series5);
            this.ticChart.Size = new System.Drawing.Size(591, 406);
            this.ticChart.TabIndex = 0;
            this.ticChart.Text = "TIC Chart";
            title5.Name = "titleTic";
            title5.Text = "TIC";
            this.ticChart.Titles.Add(title5);
            // 
            // basePeakChart
            // 
            chartArea6.Name = "ChartArea1";
            this.basePeakChart.ChartAreas.Add(chartArea6);
            legend6.Name = "Legend1";
            this.basePeakChart.Legends.Add(legend6);
            this.basePeakChart.Location = new System.Drawing.Point(4, 4);
            this.basePeakChart.Margin = new System.Windows.Forms.Padding(4);
            this.basePeakChart.Name = "basePeakChart";
            series6.ChartArea = "ChartArea1";
            series6.Legend = "Legend1";
            series6.Name = "Series1";
            this.basePeakChart.Series.Add(series6);
            this.basePeakChart.Size = new System.Drawing.Size(591, 394);
            this.basePeakChart.TabIndex = 0;
            this.basePeakChart.Text = "BasePeak Chart";
            title6.Name = "titleBasePeak";
            title6.Text = "Base Peak";
            this.basePeakChart.Titles.Add(title6);
            // 
            // spectraGridView
            // 
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
            this.spectraGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spectraGridView.Location = new System.Drawing.Point(0, 0);
            this.spectraGridView.Margin = new System.Windows.Forms.Padding(4);
            this.spectraGridView.Name = "spectraGridView";
            this.spectraGridView.RowHeadersWidth = 62;
            this.spectraGridView.RowTemplate.Height = 23;
            this.spectraGridView.Size = new System.Drawing.Size(1204, 822);
            this.spectraGridView.TabIndex = 0;
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
            this.spectrumChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            chartArea7.Name = "ChartArea1";
            this.spectrumChart.ChartAreas.Add(chartArea7);
            legend7.Name = "Legend1";
            this.spectrumChart.Legends.Add(legend7);
            this.spectrumChart.Location = new System.Drawing.Point(4, 6);
            this.spectrumChart.Margin = new System.Windows.Forms.Padding(4);
            this.spectrumChart.Name = "spectrumChart";
            series7.ChartArea = "ChartArea1";
            series7.Legend = "Legend1";
            series7.Name = "Series1";
            this.spectrumChart.Series.Add(series7);
            this.spectrumChart.Size = new System.Drawing.Size(887, 362);
            this.spectrumChart.TabIndex = 2;
            this.spectrumChart.Text = "Spectrum Chart";
            title7.Name = "titleSpectrum";
            title7.Text = "Spectrum";
            this.spectrumChart.Titles.Add(title7);
            // 
            // xicChart
            // 
            this.xicChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            chartArea8.Name = "ChartArea1";
            this.xicChart.ChartAreas.Add(chartArea8);
            legend8.Name = "Legend1";
            this.xicChart.Legends.Add(legend8);
            this.xicChart.Location = new System.Drawing.Point(-1, 6);
            this.xicChart.Margin = new System.Windows.Forms.Padding(4);
            this.xicChart.Name = "xicChart";
            series8.ChartArea = "ChartArea1";
            series8.Legend = "Legend1";
            series8.Name = "Series1";
            this.xicChart.Series.Add(series8);
            this.xicChart.Size = new System.Drawing.Size(890, 362);
            this.xicChart.TabIndex = 3;
            this.xicChart.Text = "XIC Chart";
            title8.Name = "titleXic";
            title8.Text = "XIC";
            this.xicChart.Titles.Add(title8);
            // 
            // mainLayout
            // 
            this.mainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayout.Location = new System.Drawing.Point(0, 0);
            this.mainLayout.Margin = new System.Windows.Forms.Padding(4);
            this.mainLayout.Name = "mainLayout";
            this.mainLayout.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // mainLayout.Panel1
            // 
            this.mainLayout.Panel1.Controls.Add(this.topLayout);
            // 
            // mainLayout.Panel2
            // 
            this.mainLayout.Panel2.Controls.Add(this.bottomLayout);
            this.mainLayout.Size = new System.Drawing.Size(1800, 1200);
            this.mainLayout.SplitterDistance = 822;
            this.mainLayout.SplitterWidth = 6;
            this.mainLayout.TabIndex = 4;
            // 
            // topLayout
            // 
            this.topLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLayout.Location = new System.Drawing.Point(0, 0);
            this.topLayout.Margin = new System.Windows.Forms.Padding(4);
            this.topLayout.Name = "topLayout";
            // 
            // topLayout.Panel1
            // 
            this.topLayout.Panel1.Controls.Add(this.ticAndBasePeakLayout);
            // 
            // topLayout.Panel2
            // 
            this.topLayout.Panel2.Controls.Add(this.spectraGridView);
            this.topLayout.Size = new System.Drawing.Size(1800, 822);
            this.topLayout.SplitterDistance = 590;
            this.topLayout.SplitterWidth = 6;
            this.topLayout.TabIndex = 0;
            // 
            // ticAndBasePeakLayout
            // 
            this.ticAndBasePeakLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ticAndBasePeakLayout.Location = new System.Drawing.Point(0, 0);
            this.ticAndBasePeakLayout.Margin = new System.Windows.Forms.Padding(4);
            this.ticAndBasePeakLayout.Name = "ticAndBasePeakLayout";
            this.ticAndBasePeakLayout.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // ticAndBasePeakLayout.Panel1
            // 
            this.ticAndBasePeakLayout.Panel1.Controls.Add(this.ticChart);
            // 
            // ticAndBasePeakLayout.Panel2
            // 
            this.ticAndBasePeakLayout.Panel2.Controls.Add(this.basePeakChart);
            this.ticAndBasePeakLayout.Size = new System.Drawing.Size(590, 822);
            this.ticAndBasePeakLayout.SplitterDistance = 414;
            this.ticAndBasePeakLayout.SplitterWidth = 6;
            this.ticAndBasePeakLayout.TabIndex = 0;
            // 
            // bottomLayout
            // 
            this.bottomLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLayout.Location = new System.Drawing.Point(0, 0);
            this.bottomLayout.Margin = new System.Windows.Forms.Padding(4);
            this.bottomLayout.Name = "bottomLayout";
            // 
            // bottomLayout.Panel1
            // 
            this.bottomLayout.Panel1.Controls.Add(this.spectrumChart);
            // 
            // bottomLayout.Panel2
            // 
            this.bottomLayout.Panel2.Controls.Add(this.xicChart);
            this.bottomLayout.Size = new System.Drawing.Size(1800, 372);
            this.bottomLayout.SplitterDistance = 895;
            this.bottomLayout.SplitterWidth = 6;
            this.bottomLayout.TabIndex = 0;
            // 
            // AirdPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainLayout);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AirdPanel";
            this.Size = new System.Drawing.Size(1800, 1200);
            ((System.ComponentModel.ISupportInitialize)(this.ticChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.basePeakChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectraGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spectrumChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xicChart)).EndInit();
            this.mainLayout.Panel1.ResumeLayout(false);
            this.mainLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainLayout)).EndInit();
            this.mainLayout.ResumeLayout(false);
            this.topLayout.Panel1.ResumeLayout(false);
            this.topLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.topLayout)).EndInit();
            this.topLayout.ResumeLayout(false);
            this.ticAndBasePeakLayout.Panel1.ResumeLayout(false);
            this.ticAndBasePeakLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ticAndBasePeakLayout)).EndInit();
            this.ticAndBasePeakLayout.ResumeLayout(false);
            this.bottomLayout.Panel1.ResumeLayout(false);
            this.bottomLayout.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bottomLayout)).EndInit();
            this.bottomLayout.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
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
        private System.Windows.Forms.SplitContainer mainLayout;
        private System.Windows.Forms.SplitContainer topLayout;
        private System.Windows.Forms.SplitContainer ticAndBasePeakLayout;
        private System.Windows.Forms.SplitContainer bottomLayout;
    }
}
