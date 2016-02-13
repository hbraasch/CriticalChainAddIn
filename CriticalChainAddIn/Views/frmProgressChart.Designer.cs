namespace CriticalChainAddIn.Views
{
    partial class frmProgressChart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartFever = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartFever)).BeginInit();
            this.SuspendLayout();
            // 
            // chartFever
            // 
            chartArea1.Name = "Default";
            this.chartFever.ChartAreas.Add(chartArea1);
            this.chartFever.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chartFever.Legends.Add(legend1);
            this.chartFever.Location = new System.Drawing.Point(0, 0);
            this.chartFever.Name = "chartFever";
            series1.ChartArea = "Default";
            series1.Legend = "Legend1";
            series1.Name = "AlarmBorder";
            series2.ChartArea = "Default";
            series2.Legend = "Legend1";
            series2.Name = "WarningBorder";
            series3.ChartArea = "Default";
            series3.Legend = "Legend1";
            series3.Name = "SafeBorder";
            this.chartFever.Series.Add(series1);
            this.chartFever.Series.Add(series2);
            this.chartFever.Series.Add(series3);
            this.chartFever.Size = new System.Drawing.Size(727, 566);
            this.chartFever.TabIndex = 0;
            this.chartFever.Text = "chart1";
            // 
            // frmProgressChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(727, 566);
            this.Controls.Add(this.chartFever);
            this.Name = "frmProgressChart";
            this.Text = "Fever Chart";
            ((System.ComponentModel.ISupportInitialize)(this.chartFever)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartFever;
    }
}