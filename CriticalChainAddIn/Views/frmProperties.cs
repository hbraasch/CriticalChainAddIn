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

            this.FormClosing += FrmProperties_FormClosing;
        }

        private void FrmProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.Cancel)
            {

            }
            else
            {
                switch (e.CloseReason)
                {
                    case CloseReason.UserClosing:
                        e.Cancel = true;
                        ExitQuit();
                        break;
                }
            }
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
            ExitOk();

        }

        private void ExitOk()
        {
            try
            {
                if (IsDirty)
                {
                    inputOutputData.AggressiveDurationInMinutes = Validation.ValidateForNumericValue(boxAggressiveDurationInDays.Text) * DurationTools.GetDays2Minutes();
                    inputOutputData.SafeDurationInMinutes = Validation.ValidateForNumericValue(boxSafeDurationInDays.Text) * DurationTools.GetDays2Minutes();
                    inputOutputData.ExitState = InputOutputData.ExitStates.EXIT;
                }
                else
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
            ExitQuit();
        }

        private void ExitQuit()
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
