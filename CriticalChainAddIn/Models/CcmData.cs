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
            public Dictionary<string, BufferProgressData> BufferProgressDatas { get; set; } = new Dictionary<string, BufferProgressData>();
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
            public int AuxDurationInMinutes { get; set; }

        }

        public class BufferProgressData
        {
            public List<ProgressData> ProgressDatas = new List<ProgressData>();

            public class ProgressData
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

        public static void UpdateTaskData(int taskId, int safeDurationInMinutes)
        {
            if (repository == null) Load();
            if (repository.TaskDatas.Keys.Contains(taskId))
            {
                repository.TaskDatas[taskId].AuxDurationInMinutes = safeDurationInMinutes;
            } else
            {
                repository.TaskDatas.Add(taskId, new TaskData { TaskId = taskId, AuxDurationInMinutes = safeDurationInMinutes});
            }
            Save();
        }

        public static void UpdateBufferProgressData(string bufferId, BufferProgressData.ProgressData progressData)
        {
            if (repository == null) Load();
            if (repository.BufferProgressDatas.Keys.Contains(bufferId))
            {
                // Buffer already exists
                if (repository.BufferProgressDatas[bufferId].ProgressDatas.FirstOrDefault(o=>o.SampleDate == progressData.SampleDate) != null)
                {
                    // Overwrite data
                    var item = repository.BufferProgressDatas[bufferId].ProgressDatas.FirstOrDefault(o => o.SampleDate == progressData.SampleDate);
                    var index = repository.BufferProgressDatas[bufferId].ProgressDatas.IndexOf(item);
                    repository.BufferProgressDatas[bufferId].ProgressDatas[index] = progressData;

                } else
                {
                    // Add item
                    repository.BufferProgressDatas[bufferId].ProgressDatas.Add(progressData);
                }
            }
            else
            {
                // Create new buffer with first data item
                repository.BufferProgressDatas.Add(bufferId, new BufferProgressData ());
                repository.BufferProgressDatas[bufferId].ProgressDatas.Add(progressData);
            }
            Save();
        }

        public static List<BufferProgressData.ProgressData> GetBufferProgressDatas(string bufferId)
        {
            if (repository.BufferProgressDatas.Keys.Contains(bufferId))
            {
                return repository.BufferProgressDatas[bufferId].ProgressDatas;
            } else
            {
                return new List<BufferProgressData.ProgressData>();
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
    }
}
