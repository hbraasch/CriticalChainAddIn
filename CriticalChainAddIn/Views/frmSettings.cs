using CriticalChainAddIn.Utils;
using System;
using System.Windows.Forms;

namespace CriticalChainAddIn.Views
{
    public partial class frmSettings : Form
    {
        public InputOutputData inputOutputData { get; set; } = new InputOutputData();
        public class InputOutputData
        {
            public int WorkingHoursPerDay { get; set; }
            public ExitStates ExitState { get; set; }

            public enum ExitStates
            {
                EXIT, QUIT
            }
        }

        public bool IsDirty { get; set; } 
        private bool IsEventsEnabled = false;

        public frmSettings()
        {
            InitializeComponent();
        }

        public frmSettings(InputOutputData inputOutputData): base()
        {
            InitializeComponent();
            this.inputOutputData = inputOutputData;
            boxWorkingHoursPerDay.Text = $"{inputOutputData.WorkingHoursPerDay}";
            IsEventsEnabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (IsDirty)
            {
                inputOutputData.WorkingHoursPerDay = Validation.ValidateForNumericValue(boxWorkingHoursPerDay.Text);
                inputOutputData.ExitState = InputOutputData.ExitStates.EXIT;
            } else
            {
                inputOutputData.ExitState = InputOutputData.ExitStates.QUIT;
            }
            Close();
        }

        private void boxWorkingHoursPerDay_TextChanged(object sender, EventArgs e)
        {
            if (IsEventsEnabled)
            {
                IsDirty = true;
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputOutputData.ExitState = InputOutputData.ExitStates.QUIT;
            Close();
        }
    }


}
