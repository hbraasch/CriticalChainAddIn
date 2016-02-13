using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using MSProject = Microsoft.Office.Interop.MSProject;
using Office = Microsoft.Office.Core;

namespace CriticalChainAddIn
{
    public partial class CmmAddIn
    {
        public static MSProject.Application thisApp;
        public static Action<MSProject.Project> OnProjectLoaded;
        public static Action<MSProject.Task, int> OnTaskDurationChanged;

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            thisApp = Application;
            this.Application.NewProject += Application_NewProject;
            this.Application.ProjectBeforeTaskChange += Application_ProjectBeforeTaskChange;
        }

        private void Application_ProjectBeforeTaskChange(MSProject.Task task, MSProject.PjField field, object newVal, ref bool cancel)
        {
            if (field == Microsoft.Office.Interop.MSProject.PjField.pjTaskDurationText)
            {
                OnTaskDurationChanged(task, (int) ParseDurationText((string) newVal));
            }
        }

        private int ParseDurationText(string newVal)
        {
            var lastChar = newVal.Substring(newVal.Length-1);
            var dataString = newVal.Substring(0, newVal.Length - 1);
            switch (lastChar)
            {
                case "d":   
                    return (int)(int.Parse(dataString) * thisApp.ActiveProject.HoursPerDay * 60);
                case "w":
                    return (int)(int.Parse(dataString) * thisApp.ActiveProject.HoursPerWeek * 60);
                default:
                    return (int)(int.Parse(newVal) * thisApp.ActiveProject.HoursPerDay * 60);
            }
        }   


        private void Application_NewProject(MSProject.Project pj)
        {
            OnProjectLoaded(pj);
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
