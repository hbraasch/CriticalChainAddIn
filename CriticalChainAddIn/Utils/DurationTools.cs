using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CriticalChainAddIn.Utils
{
    class DurationTools
    {

        public static int WORKING_HOURS_PER_DAY_DEFAULT = 8;

        private static float GetWorkinghHoursPerDay()
        {
            if (Properties.Settings.Default.WorkingHoursPerDay == 0)
            {
                return WORKING_HOURS_PER_DAY_DEFAULT;
            }
            else
            {
                return Properties.Settings.Default.WorkingHoursPerDay;
            }
        }

        public static int GetDays2Minutes() => (int)(60 * GetWorkinghHoursPerDay());

        public static float GetMinutes2Days() => 1f / (60 * GetWorkinghHoursPerDay());
    }
}
