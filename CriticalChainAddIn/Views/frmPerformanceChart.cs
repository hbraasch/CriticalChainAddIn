using CriticalChainAddIn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CriticalChainAddIn.Views
{
    public partial class frmPerformanceChart : Form
    {


        public class InputData
        {
            public Dictionary<string, CcmData.BufferPerformanceData> BufferProgressDatas { get; set; }
        }



        public frmPerformanceChart()
        {
            InitializeComponent();
            UpdateChart();
        }

        


        public void UpdateChart()
        {
            Random random = new Random();


            chartFever.Series["WarningBorder"].Points.AddXY(0, 20);
            chartFever.Series["WarningBorder"].Points.AddXY(100, 90);

            chartFever.Series["SafeBorder"].Points.AddXY(0, 0);
            chartFever.Series["SafeBorder"].Points.AddXY(100, 50);

            chartFever.Series["AlarmBorder"].Points.AddXY(0, 100);
            chartFever.Series["AlarmBorder"].Points.AddXY(100, 100);

            // Set series chart type
            chartFever.Series["SafeBorder"].ChartType = SeriesChartType.Area;
            chartFever.Series["SafeBorder"].Color = Color.FromArgb(220, 0, 255, 0); ;
            chartFever.Series["WarningBorder"].ChartType = SeriesChartType.Area;
            chartFever.Series["WarningBorder"].Color = Color.FromArgb(220, 255, 160, 0); ;
            chartFever.Series["AlarmBorder"].ChartType = SeriesChartType.Area;
            chartFever.Series["AlarmBorder"].Color = Color.FromArgb(220, 255, 0, 0); ;

            // Set point labels
            chartFever.Series["SafeBorder"].IsValueShownAsLabel = false;
            chartFever.Series["WarningBorder"].IsValueShownAsLabel = false;
            chartFever.Series["AlarmBorder"].IsValueShownAsLabel = false;

            // Enable X axis margin
            chartFever.ChartAreas["Default"].AxisX.IsMarginVisible = false;
            chartFever.ChartAreas["Default"].BackColor = Color.Red;
            chartFever.ChartAreas["Default"].AxisX.Maximum = 100;
            chartFever.ChartAreas["Default"].AxisY.Maximum = 100;

            // Show as 3D
            chartFever.ChartAreas["Default"].Area3DStyle.Enable3D = false;

            // Fill data
            var bufferProgressDatas = CcmData.GetRepository().BufferPerformanceDatas;
            foreach (var bufferProgressData in bufferProgressDatas)
            {
                var newSeries = new Series { Name = $"Buffer: {bufferProgressData.Key}" };
                newSeries.ChartType = SeriesChartType.Line;
                newSeries.Color = Color.Black;
                newSeries.IsValueShownAsLabel = true;
                newSeries.Label = bufferProgressData.Key;
                newSeries.LabelFormat = "Top";

                chartFever.Series.Add(newSeries);
                foreach (var progressData in bufferProgressData.Value.PerformanceDatas)
                {
                    newSeries.Points.AddXY(progressData.PercentProjectCompleted * 100, progressData.PercentBufferUsed * 100);
                }
            }
        }

    }
}
