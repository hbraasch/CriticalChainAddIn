using CriticalChainAddIn.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MSProject = Microsoft.Office.Interop.MSProject;

namespace CriticalChainAddIn.Models
{
    public class CcmData
    {
        /// <summary>
        /// Saves aditional data per task e.g. Safe duration
        /// </summary>

        MSProject.Application application;
        MSProject.Project project;
        static Repository repository;

        public CcmData()
        {
            application = CmmAddIn.thisApp;
            project = CmmAddIn.thisApp.ActiveProject;
        }

        public class Repository
        {
            public Dictionary<int, TaskData> TaskDatas { get; set; } = new Dictionary<int, TaskData>();
            public Dictionary<string, BufferPerformanceData> BufferPerformanceDatas { get; set; } = new Dictionary<string, BufferPerformanceData>();
            public bool IsBuffersHidden { get; set; } = false;
        }

        public static Repository GetRepository()
        {
            if (repository == null) Load();
            return repository;
        }

        public class TaskData
        {
            public int TaskId { get; set; }
            public int SnapShotDurationInMinutes { get; set; }
            public int AuxDurationInMinutes { get; set; }

        }

        public class BufferPerformanceData
        {
            public List<PerformanceData> PerformanceDatas = new List<PerformanceData>();

            public class PerformanceData
            {
                public float PercentBufferUsed { get; set; }
                public float PercentProjectCompleted { get; set; }
                public DateTime SampleDate { get; set; }
            }
        }


        public static void Load()
        {
            var cmmDataJson = Properties.Settings.Default.CcmDataJson;
            if (cmmDataJson.IsEmpty())
            {
                repository = new Repository();
            }
            else
            {
                repository = Serialize.DeserializeObjectFromStringJson<Repository>(cmmDataJson);
            }
        }

        internal static void ClearTaskData()
        {
            if (repository == null) Load();
            repository.TaskDatas.Clear();
            Save();
        }

        public static void Save()
        {
            var cmmDataJson = Serialize.SerializeObjectToStringJson<Repository>(repository);
            Properties.Settings.Default.CcmDataJson = cmmDataJson;
            Properties.Settings.Default.Save();
        }

        public static TaskData GetTaskData (int taskId)
        {
            if (repository == null) Load(); 
            if (repository.TaskDatas.Keys.Contains(taskId))
            {
                return repository.TaskDatas[taskId];
            }
            return null;
        }

        public static void UpdateTaskData(int taskId, int aggressiveDurationInMinutes, int safeDurationInMinutes)
        {
            if (repository == null) Load();
            if (repository.TaskDatas.Keys.Contains(taskId))
            {
                repository.TaskDatas[taskId].SnapShotDurationInMinutes = aggressiveDurationInMinutes;
                repository.TaskDatas[taskId].AuxDurationInMinutes = safeDurationInMinutes;
            }
            else
            {
                repository.TaskDatas.Add(taskId, new TaskData { TaskId = taskId,
                    SnapShotDurationInMinutes = aggressiveDurationInMinutes,
                    AuxDurationInMinutes = safeDurationInMinutes});
            }
            Save();
        }

        public static void UpdateBufferPerformanceData(string bufferId, BufferPerformanceData.PerformanceData progressData)
        {
            if (repository == null) Load();
            if (repository.BufferPerformanceDatas.Keys.Contains(bufferId))
            {
                // Buffer already exists
                if (repository.BufferPerformanceDatas[bufferId].PerformanceDatas.FirstOrDefault(o=>o.SampleDate == progressData.SampleDate) != null)
                {
                    // Overwrite data
                    var item = repository.BufferPerformanceDatas[bufferId].PerformanceDatas.FirstOrDefault(o => o.SampleDate == progressData.SampleDate);
                    var index = repository.BufferPerformanceDatas[bufferId].PerformanceDatas.IndexOf(item);
                    repository.BufferPerformanceDatas[bufferId].PerformanceDatas[index] = progressData;

                } else
                {
                    // Add item
                    repository.BufferPerformanceDatas[bufferId].PerformanceDatas.Add(progressData);
                }
            }
            else
            {
                // Create new buffer with first data item
                repository.BufferPerformanceDatas.Add(bufferId, new BufferPerformanceData ());
                repository.BufferPerformanceDatas[bufferId].PerformanceDatas.Add(progressData);
            }
            Save();
        }

        public static List<BufferPerformanceData.PerformanceData> GetBufferProgressDatas(string bufferId)
        {
            if (repository.BufferPerformanceDatas.Keys.Contains(bufferId))
            {
                return repository.BufferPerformanceDatas[bufferId].PerformanceDatas;
            } else
            {
                return new List<BufferPerformanceData.PerformanceData>();
            }
        }

        internal static bool GetIsBuffersHidden() => GetRepository().IsBuffersHidden;

        internal static void  SetIsBuffersHidden(bool isHidden)
        {
            GetRepository().IsBuffersHidden = isHidden;
            Save();
        }

        internal static void SaveBufferDuration(int taskId, dynamic durationInMinutes)
        {
            
            if (GetRepository().TaskDatas.Keys.Contains(taskId))
            {
                var taskData = GetRepository().TaskDatas.FirstOrDefault(o => o.Key == taskId).Value;
                taskData.AuxDurationInMinutes = durationInMinutes;
            }
            else
            {
                var taskData = new TaskData {
                    TaskId = taskId,
                    AuxDurationInMinutes = durationInMinutes,
                };
                repository.TaskDatas.Add(taskId, taskData);
            }
            Save();
        }

        internal static int GetBufferDuration(int taskId)
        {
            if (GetRepository().TaskDatas.Keys.Contains(taskId))
            {
                return repository.TaskDatas.FirstOrDefault(o => o.Key == taskId).Value.AuxDurationInMinutes;
            }
            throw new BusinessException("Buffer does not exist");
        }

        /// <summary>
        /// Used to get safe duration, get if from saved value, and if not existing or duraion has changed, update it to task duration values
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="taskDuration"></param>
        /// <returns></returns>
        internal static int GetTaskSafeDurationInMinutes(string taskId, dynamic taskDuration)
        {
            var taskData = CcmData.GetTaskData(int.Parse(taskId)) ?? new CcmData.TaskData { SnapShotDurationInMinutes = taskDuration, AuxDurationInMinutes = taskDuration };
            if (taskDuration == taskData.SnapShotDurationInMinutes)
            {
                // Good, no changes
                return CcmData.GetTaskData(int.Parse(taskId))?.AuxDurationInMinutes ?? taskDuration;
            }
            else
            {
                // Something changed, generate new default values
                CcmData.UpdateTaskData(int.Parse(taskId), taskDuration, taskDuration);
                return taskDuration;
            }
        }
    }
}
