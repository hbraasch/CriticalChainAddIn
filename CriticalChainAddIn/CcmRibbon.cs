using System;
using System.Collections.Generic;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;
using MSProject = Microsoft.Office.Interop.MSProject;
using CriticalChainAddIn.Views;
using CriticalChainAddIn.Utils;
using CriticalChainAddIn.Models;
using System.Linq;

namespace CriticalChainAddIn
{
    public partial class CcmRibbon
    {
        MSProject.Application application;
        MSProject.Project project;


        private void CcmRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            application = CmmAddIn.thisApp;
            CmmAddIn.OnProjectLoaded += (project) => { 
                this.project = project;

                // Update ribbon display
                var isBuffersHidden = CcmData.GetIsBuffersHidden();
                HideUnhideBuffers(isBuffersHidden);
            };
        }

        private void buttonCmmProperties_Click(object sender, RibbonControlEventArgs e)
        {

            try
            {
                
                // Conduct: Get durations of selected item
                var selection = application.ActiveSelection;
                var selectedTasks = selection.Tasks;
                MSProject.Task selectedTask;
                if (selectedTasks == null) throw new BusinessException("Please select a task");
                if (selectedTasks?.Count > 1) throw new BusinessException("Please select only a single task");
                var inputOutputData = new frmProperties.InputOutputData();
                selectedTask = selectedTasks[1];

                if (selectedTask.Name.Contains(Constants.BUFFER_NAME) || selectedTask.Name.Contains(Constants.MILESTONE_NAME)) throw new BusinessException("You can only set CCM properties of normal tasks");

                inputOutputData.AggressiveDurationInMinutes = Convert.ToInt32(selectedTask.Duration);
                inputOutputData.SafeDurationInMinutes = CcmData.GetTaskData(selectedTask.ID)?.AuxDurationInMinutes ?? inputOutputData.AggressiveDurationInMinutes;
                // End of: Get durations of selected item

                // Display form
                var form = new frmProperties(inputOutputData);
                form.ShowDialog();
                if (form.inputOutputData.ExitState == frmProperties.InputOutputData.ExitStates.EXIT)
                {
                    // Conduct: Save duration
                    selectedTask.Duration = form.inputOutputData.AggressiveDurationInMinutes * DurationTools.GetMinutes2Days();
                    CcmData.UpdateTaskData(selectedTask.ID, (int)(form.inputOutputData.SafeDurationInMinutes * DurationTools.GetMinutes2Days()));
                    // End of: Save duration
                }

            }
            catch (BusinessException ex)
            {
                MessageBox.Show(ex.Message);
            } catch (Exception ex)
            {
                Console.Write(ex.CompleteMessage());
            }

        }

        private void buttonCcmPerformance_Click(object sender, RibbonControlEventArgs e)
        {

            if (project.StatusDate is string)
            {
                if (project.StatusDate == "NA")
                {
                    MessageBox.Show("Set the status date");
                    return;
                } 
            }

            Dictionary<string, MSProject.Task> indexedTasks = new Dictionary<string, Microsoft.Office.Interop.MSProject.Task>();

            // Calculate performance for each buffer
            var criticalChains = new List<List<Activity>>();
            // Find all buffer tasks
            var buffers = GetAllBuffers();

            // For each buffer, get critical path
            foreach (var buffer in buffers)
            {
                criticalChains.Add(GetCriticalPath(buffer));
            }

            // Index all activityTasks
            foreach (var criticalChain in criticalChains)
            {
                foreach (var item in criticalChain)
                {
                    if (!indexedTasks.ContainsKey(item.Id)) {
                        indexedTasks.Add(item.Id, project.Tasks.Find(int.Parse(item.Id)));
                    }
                }               
            }
            // End of: Calculate performance for each buffer

            // Calculate performance for each buffer
            foreach (var criticalChain in criticalChains)
            {
                // Determine total duration of chain
                int totalChainDurationInMinutes = 0;
                foreach (var item in criticalChain)
                {
                    totalChainDurationInMinutes += item.Duration;
                }

                // Determine how much time from start of chain till status date
                int timeTillStatusDateInMinutes = 0;
                foreach (var item in criticalChain)
                {
                    var itemTask = indexedTasks[item.Id];
                    if (itemTask.Finish <= project.StatusDate)
                    {
                        timeTillStatusDateInMinutes += itemTask.Duration;
                    }
                    else if (itemTask.Start > project.StatusDate)
                    {
                        // Do not take these into account
                    }
                    else
                    {
                        timeTillStatusDateInMinutes += GetWorkingTimeBetweenDatesInMinutes(itemTask.Start, project.StatusDate);
                    }
                }

                // Determine how much is progress (in time)
                // Get tasks with some progress in chain
                List<MSProject.Task> progressedTasks = new List<MSProject.Task>();
                foreach (var item in criticalChain)
                {
                    var itemTask = indexedTasks[item.Id];
                    if (itemTask.PercentComplete > 0)
                    {
                        progressedTasks.Add(itemTask);
                    }
                }

                // Add up their progress
                int progressedTimeInMinutes = 0;
                foreach (var progressedTask in progressedTasks)
                {
                    progressedTimeInMinutes += (progressedTask.PercentComplete * progressedTask.Duration)/100;
                }
                // End of: Determine how much is progress (in time)

                // Determine percentage of buffer used
                var bufferIndex = criticalChains.IndexOf(criticalChain);
                var timeBehind = (float)(Math.Max(0,timeTillStatusDateInMinutes - progressedTimeInMinutes));
                float percentBufferUsed = Math.Min(1f, (float) (timeBehind / buffers[bufferIndex].Duration));

                // Determine percent project complete
                float percentProjectComplete = (float) timeTillStatusDateInMinutes / totalChainDurationInMinutes;

                // Save this datapoint
                CcmData.UpdateBufferProgressData(buffers[bufferIndex].ID.ToString(), new CcmData.BufferProgressData.ProgressData {
                    PercentBufferUsed = percentBufferUsed,
                    PercentProjectCompleted = percentProjectComplete,
                    SampleDate = project.StatusDate,
                });

            }

            // Display performance
            var form = new frmProgressChart();
            form.ShowDialog();
            // End of: Display performance

        }

        private int GetWorkingTimeBetweenDatesInMinutes(DateTime startDate, DateTime endDate)
        {
            var days = Math.Ceiling((endDate - startDate).TotalDays);
            var timeInHours = days * project.HoursPerDay;
            return (int)(timeInHours * 60);
        }

        private void buttonCcmSettings_Click(object sender, RibbonControlEventArgs e)
        {
            var inputOutputData = new frmSettings.InputOutputData {WorkingHoursPerDay = Properties.Settings.Default.WorkingHoursPerDay };
            var page = new frmSettings(inputOutputData);
            page.ShowDialog();
            if (page.inputOutputData.ExitState == frmSettings.InputOutputData.ExitStates.EXIT)
            {
                Properties.Settings.Default.WorkingHoursPerDay = page.inputOutputData.WorkingHoursPerDay;
                Properties.Settings.Default.Save();
            }
        }

        private void buttonCcmUpdateBuffers_Click(object sender, RibbonControlEventArgs e)
        {
            UpdateBuffers();
        }

        private void UpdateBuffers()
        {
            // Find all buffer tasks
            var buffers = GetAllBuffers();

            // Conduct: For each buffer, update its duration
            foreach (var buffer in buffers)
            {
                // Get critical path for buffer
                var bufferCriticalPathActivities = GetCriticalPath(buffer);

                // Calc buffer length using particular formula
                var duration = CalcBufferDuration(bufferCriticalPathActivities, (activities) =>
                {
                    // Collate all delta durations
                    int result = 0;
                    foreach (var activity in activities)
                    {
                        var taskSafeDurationInMinutes = CcmData.GetTaskData(int.Parse(activity.Id))?.AuxDurationInMinutes ?? activity.Duration;
                        if (taskSafeDurationInMinutes < activity.Duration)
                        {
                            taskSafeDurationInMinutes = activity.Duration;
                            CcmData.UpdateTaskData(int.Parse(activity.Id), taskSafeDurationInMinutes);
                        }
                        result += (taskSafeDurationInMinutes - activity.Duration) / 2;
                    }
                    return result;
                });

                // Set buffer length
                buffer.Duration = duration;

            }
            // End of: For each buffer, update its duration
        }

        private int CalcBufferDuration(List<Activity> bufferCriticalPathActivities, Func<List<Activity>, int> DurationCalculator) => DurationCalculator(bufferCriticalPathActivities);

        private List<Activity> GetCriticalPath(MSProject.Task task)
        {
            var predecessorActivities = new Dictionary<string, Activity>();
            // Get all task predecessors, up to where START another BUFFER is reached
            foreach (MSProject.Task predecessorTask in task.PredecessorTasks)
            {
                GetPredecessorActivitiesRecursively(predecessorTask, ref predecessorActivities);
            }
            // Fill predecessors for all activities
            foreach (Activity activity in predecessorActivities.Values)
            {
                MSProject.Task activityTask = project.Tasks.Find(int.Parse(activity.Id));
                foreach (MSProject.Task taskPredecessor in activityTask.PredecessorTasks)
                {   
                    if (predecessorActivities.ContainsKey(taskPredecessor.ID.ToString())) { 
                        predecessorActivities[activity.Id].Predecessors.Add(predecessorActivities[taskPredecessor.ID.ToString()]);
                    }
                }                
            }
            // End of: Get all task predecessors
            // Calc critical path
            var criticalPathActivities = predecessorActivities.Values.CriticalPath(p => p.Predecessors, l => (long)l.Duration);

            // Ensure return is sorted first to last

            // Return
            return criticalPathActivities.ToList();
        }

        private void GetPredecessorActivitiesRecursively(MSProject.Task task, ref Dictionary<string, Activity> predecessorActivities)
        {          
            if (task.Name.Contains(Constants.START_NAME) || task.Name.Contains(Constants.BUFFER_NAME)) return;
            if (predecessorActivities.ContainsKey(task.ID.ToString())) return;
            predecessorActivities.Add(task.ID.ToString(), new Activity {
                Id = task.ID.ToString(),
                Description = task.Name,
                Duration = task.Duration,
            });
            foreach (MSProject.Task predecessorTask in task.PredecessorTasks)
            {
                GetPredecessorActivitiesRecursively(predecessorTask, ref predecessorActivities);
            }           
        }

        private List<MSProject.Task> GetAllBuffers()
        {
            var list = new List<MSProject.Task>();
            foreach (MSProject.Task task in project.Tasks)
            {
                if (task.Name.Contains(Constants.BUFFER_NAME)) {
                    list.Add(task);
                }
            }
            return list;
        }

        private void buttonInsertStart_Click(object sender, RibbonControlEventArgs e)
        {
            var newTask = project.Tasks.Add(Constants.START_NAME);
            newTask.Start = project.Start;
            newTask.Duration = 0;
        }

        private void buttonInsertBuffer_Click(object sender, RibbonControlEventArgs e)
        {
            // Create buffer task
            var newTask = project.Tasks.Add(Constants.BUFFER_NAME);
            newTask.Start = project.Start;
            newTask.Duration = 1;
#if false
            var selection = application.ActiveSelection;
            var selectedTasks = selection.Tasks;
            if (selectedTasks == null)
            {
                // Create buffer task
                var newTask = project.Tasks.Add(Constants.BUFFER_NAME);
                newTask.Start = project.Start;
                newTask.Duration = 1;
            }
            else
            {
                if (selectedTasks.Count != 2) throw new BusinessException("Please select TWO tasks");
                MSProject.Task startTask = selectedTasks[1];
                long startTaskIndex = project.Tasks.ToList().FindIndex(o => o.ID == startTask.ID);
                MSProject.Task endTask = selectedTasks[2];
                if (startTask.Name.Contains(Constants.BUFFER_NAME)) throw new BusinessException("Start task cannot be a BUFFER");
                if (endTask.Name.Contains(Constants.BUFFER_NAME)) throw new BusinessException("End task cannot be a BUFFER");
                // Remove end task link to start task if exists
                var predecessorTasks = endTask.PredecessorTasks.ToList();
                predecessorTasks.Remove(predecessorTasks.FirstOrDefault(o => o.ID == startTask.ID));
                endTask.Predecessors = GeneratePredecessorsString(predecessorTasks);

                // Add dependencies
                // Create buffer task
                var newTask = project.Tasks.Add(Constants.BUFFER_NAME);
                newTask.Start = startTask.Finish;
                newTask.Duration = "";
                newTask.Predecessors = startTask.ID.ToString();
                // Add buffer to end task dependency
                endTask.Predecessors = newTask.ID.ToString();
                // End of: Add dependencies 
            } 
#endif
        }

        private string GeneratePredecessorsString(List<MSProject.Task> predecessorTasks)
        {
            string result = "";
            foreach (var predecessorTask in predecessorTasks)
            {
                if (result.IsEmpty())
                {
                    result += $"{predecessorTask.ID.ToString()}";
                } else
                {
                    result += $", {predecessorTask.ID.ToString()}";
                }
            }
            return result;
        }

        private void buttonClearPerformance_Click(object sender, RibbonControlEventArgs e)
        {
            if (MessageBox.Show("Are you sure?", "Clear", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
            CcmData.GetRepository().BufferProgressDatas.Clear();
            CcmData.Save();
        }

        private void buttonHideUnhideBuffers_Click(object sender, RibbonControlEventArgs e)
        {
            var isBuffersHidden = CcmData.GetIsBuffersHidden();
            HideUnhideBuffers(!isBuffersHidden);
            CcmData.SetIsBuffersHidden(!isBuffersHidden);
        }

        public void HideUnhideBuffers(bool isHidden)
        {
            if (isHidden)
            {
                // Hide
                foreach (MSProject.Task task in project.Tasks)
                {
                    if (task.Name.Contains(Constants.BUFFER_NAME))
                    {
                        CcmData.SaveBufferDuration(task.ID, task.Duration);
                        task.Duration = 0;
                        task.Name = Constants.MILESTONE_NAME;
                    }
                }
                buttonHideUnhideBuffers.Label = "Unhide Buffers";
            }
            else
            {
                // Unhide
                foreach (MSProject.Task task in project.Tasks)
                {
                    if (task.Name.Contains(Constants.MILESTONE_NAME))
                    {
                        task.Duration = CcmData.GetBufferDuration(task.ID);
                        task.Name = Constants.BUFFER_NAME;
                    }
                }
                buttonHideUnhideBuffers.Label = "Hide Buffers";
            }

            
        }


    }
}
