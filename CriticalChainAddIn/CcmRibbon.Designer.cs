namespace CriticalChainAddIn
{
    partial class CcmRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public CcmRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.CcmGroup = this.Factory.CreateRibbonGroup();
            this.buttonCmmProperties = this.Factory.CreateRibbonButton();
            this.buttonInsertBuffer = this.Factory.CreateRibbonButton();
            this.buttonCcmUpdateBuffers = this.Factory.CreateRibbonButton();
            this.buttonHideUnhideBuffers = this.Factory.CreateRibbonButton();
            this.buttonCcmSettings = this.Factory.CreateRibbonButton();
            this.buttonCcmViewPerformance = this.Factory.CreateRibbonButton();
            this.buttonClearPerformance = this.Factory.CreateRibbonButton();
            this.buttonInsertStart = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.CcmGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.CcmGroup);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // CcmGroup
            // 
            this.CcmGroup.Items.Add(this.buttonCmmProperties);
            this.CcmGroup.Items.Add(this.buttonInsertStart);
            this.CcmGroup.Items.Add(this.buttonInsertBuffer);
            this.CcmGroup.Items.Add(this.buttonCcmUpdateBuffers);
            this.CcmGroup.Items.Add(this.buttonHideUnhideBuffers);
            this.CcmGroup.Items.Add(this.buttonCcmSettings);
            this.CcmGroup.Items.Add(this.buttonCcmViewPerformance);
            this.CcmGroup.Items.Add(this.buttonClearPerformance);
            this.CcmGroup.Label = "Critical Chain";
            this.CcmGroup.Name = "CcmGroup";
            // 
            // buttonCmmProperties
            // 
            this.buttonCmmProperties.Label = "Properties";
            this.buttonCmmProperties.Name = "buttonCmmProperties";
            this.buttonCmmProperties.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonCmmProperties_Click);
            // 
            // buttonInsertBuffer
            // 
            this.buttonInsertBuffer.Label = "Insert Buffer";
            this.buttonInsertBuffer.Name = "buttonInsertBuffer";
            this.buttonInsertBuffer.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonInsertBuffer_Click);
            // 
            // buttonCcmUpdateBuffers
            // 
            this.buttonCcmUpdateBuffers.Label = "Update Buffers";
            this.buttonCcmUpdateBuffers.Name = "buttonCcmUpdateBuffers";
            this.buttonCcmUpdateBuffers.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonCcmUpdateBuffers_Click);
            // 
            // buttonHideUnhideBuffers
            // 
            this.buttonHideUnhideBuffers.Label = "HideUnhideBuffers";
            this.buttonHideUnhideBuffers.Name = "buttonHideUnhideBuffers";
            this.buttonHideUnhideBuffers.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonHideUnhideBuffers_Click);
            // 
            // buttonCcmSettings
            // 
            this.buttonCcmSettings.Label = "Settings";
            this.buttonCcmSettings.Name = "buttonCcmSettings";
            this.buttonCcmSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonCcmSettings_Click);
            // 
            // buttonCcmViewPerformance
            // 
            this.buttonCcmViewPerformance.Label = "View Performance";
            this.buttonCcmViewPerformance.Name = "buttonCcmViewPerformance";
            this.buttonCcmViewPerformance.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonCcmPerformance_Click);
            // 
            // buttonClearPerformance
            // 
            this.buttonClearPerformance.Label = "Clear Performance";
            this.buttonClearPerformance.Name = "buttonClearPerformance";
            this.buttonClearPerformance.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonClearPerformance_Click);
            // 
            // buttonInsertStart
            // 
            this.buttonInsertStart.Label = "Insert Start";
            this.buttonInsertStart.Name = "buttonInsertStart";
            this.buttonInsertStart.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.buttonInsertStart_Click);
            // 
            // CcmRibbon
            // 
            this.Name = "CcmRibbon";
            this.RibbonType = "Microsoft.Project.Project";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.CcmRibbon_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.CcmGroup.ResumeLayout(false);
            this.CcmGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup CcmGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonCmmProperties;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonCcmUpdateBuffers;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonCcmViewPerformance;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonCcmSettings;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonInsertBuffer;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonClearPerformance;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonHideUnhideBuffers;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton buttonInsertStart;
    }

    partial class ThisRibbonCollection
    {
        internal CcmRibbon CcmRibbon
        {
            get { return this.GetRibbon<CcmRibbon>(); }
        }
    }
}
