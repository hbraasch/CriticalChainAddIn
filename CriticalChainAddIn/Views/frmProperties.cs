using CriticalChainAddIn.Utils;
using System;
using System.Windows.Forms;

namespace CriticalChainAddIn.Views
{
    public partial class frmProperties : Form
    {

        public InputOutputData inputOutputData { get; set; } = new InputOutputData();
        public class InputOutputData
        {
            public int AggressiveDurationInMinutes { get; set; }
            public int SafeDurationInMinutes { get; set; }

            public ExitStates ExitState {get; set; }

            public enum ExitStates
            {
                EXIT, QUIT
            }
        }

        public bool IsDirty { get; set; }
        private bool IsEventsEnabled = false;

        public frmProperties()
        {
            InitializeComponent();
        }

        public frmProperties(InputOutputData inputOutputData): base()
        {
            InitializeComponent();
            this.inputOutputData = inputOutputData;
            IsEventsEnabled = false;
            boxAggressiveDurationInDays.Text = $"{inputOutputData.AggressiveDurationInMinutes * DurationTools.GetMinutes2Days()}";
            boxSafeDurationInDays.Text = $"{inputOutputData.SafeDurationInMinutes * DurationTools.GetMinutes2Days()}";
            IsEventsEnabled = true;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsDirty)
                {
                    inputOutputData.AggressiveDurationInMinutes = Validation.ValidateForNumericValue(boxAggressiveDurationInDays.Text);
                    inputOutputData.SafeDurationInMinutes = Validation.ValidateForNumericValue(boxSafeDurationInDays.Text);
                    inputOutputData.ExitState = InputOutputData.ExitStates.EXIT;
                } else
                {
                    inputOutputData.ExitState = InputOutputData.ExitStates.QUIT;
                }
                this.Close();
            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.CompleteMessage());
            }
            
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            inputOutputData.ExitState = InputOutputData.ExitStates.QUIT;
            this.Close();
        }

        private void boxAggressiveDurationInDays_TextChanged(object sender, EventArgs e)
        {
            if (IsEventsEnabled)
            {
                IsDirty = true; 
            }
        }

        private void boxSafeDurationInDays_TextChanged(object sender, EventArgs e)
        {
            if (IsEventsEnabled)
            {
                IsDirty = true;
            }
        }
    }
}
