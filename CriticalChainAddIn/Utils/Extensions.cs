using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSProject = Microsoft.Office.Interop.MSProject;

namespace CriticalChainAddIn.Utils
{
    static class Extensions
    {

        public static bool IsEmpty(this string value) => (String.IsNullOrEmpty(value));

        public static string CompleteMessage(this Exception exp)
        {
            string message = string.Empty;
            Exception innerException = exp;

            do
            {
                message = message + " " + (string.IsNullOrEmpty(innerException.Message) ? string.Empty : innerException.Message);
                innerException = innerException.InnerException;
            }
            while (innerException != null);

            return message;
        }

        public static bool IsNumeric(this string s)
        {
            float output;
            return float.TryParse(s, out output);
        }

        public static bool IsEmpty<T>(this List<T> list)
        {
            if (list == null) return true;
            return list.Count == 0;
        }

        public static MSProject.Task Find(this MSProject.Tasks tasks, int id  )
        {
            foreach (MSProject.Task task in tasks)
            {
                if (task.ID == id) return task;
            }
            throw new Exception($"Task with ID: {id} does note exist");
        }

        public static List<MSProject.Task> ToList(this MSProject.Tasks tasks)
        {
            var list = new List<MSProject.Task>();
            foreach (MSProject.Task task in tasks)
            {
                list.Add(task);
            }
            return list;
        }
    }
}
