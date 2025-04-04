using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXC.Technology.PerformanceCounters
{
    public class EndToEndPerformanceRecord
    {
        #region Static Fields
        /// <summary>
        /// Stores the average browser time in seconds.
        /// </summary>
        private static double averageBrowserTimeInSeconds = 0;
        #endregion

        #region Instance Fields
        /// <summary>
        /// Stores the name of the active controller.
        /// </summary>
        private string activeController;

        /// <summary>
        /// Stores the name of the last action performed.
        /// </summary>
        private string lastAction;

        /// <summary>
        /// Stores the name of the current action being performed.
        /// </summary>
        private string currentAction;
        #endregion

        #region Constructors
        // No constructors in this class.
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the server render time.
        /// </summary>
        public string ServerRenderTime { get; private set; }

        /// <summary>
        /// Gets or sets the browser load time.
        /// </summary>
        public string BrowserLoadTime { get; private set; }

        /// <summary>
        /// Gets or sets the browser submit time.
        /// </summary>
        public string BrowserSubmitTime { get; private set; }

        /// <summary>
        /// Gets or sets the server receive time.
        /// </summary>
        public string ServerReceiveTime { get; private set; }

        /// <summary>
        /// Gets or sets the average browser time.
        /// </summary>
        public string AverageBrowserTime { get; set; }

        /// <summary>
        /// Gets or sets the browser time.
        /// </summary>
        public string BrowserTime { get; set; }

        /// <summary>
        /// Gets or sets the processing time.
        /// </summary>
        public string ProcessingTime { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initializes the end-to-end performance record.
        /// </summary>
        /// <param name="serverRenderTime">The server render time.</param>
        /// <param name="browserLoadTime">The browser load time.</param>
        /// <param name="currentBrowserSubmitTime">The current browser submit time.</param>
        public void InitializeEndToEndPerformanceRecord(string serverRenderTime, string browserLoadTime, string currentBrowserSubmitTime)
        {
            ServerRenderTime = serverRenderTime;
            BrowserLoadTime = browserLoadTime;
            BrowserSubmitTime = currentBrowserSubmitTime;
            ServerReceiveTime = DXC.Technology.Utilities.Time.FromNow();
        }

        /// <summary>
        /// Initializes for forward end-to-end performance record.
        /// </summary>
        public void InitializeForForwardEndToEndPerformanceRecord()
        {
            if (string.IsNullOrEmpty(ServerReceiveTime))
            {
                ServerReceiveTime = DXC.Technology.Utilities.Time.FromNow();
            }

            if (IsPostBackLikeAction)
            {
                return;
            }

            ServerRenderTime = DXC.Technology.Utilities.Time.FromNow();
            BrowserLoadTime = DXC.Technology.Utilities.Time.FromNow();
            BrowserSubmitTime = DXC.Technology.Utilities.Time.FromNow();
        }

        /// <summary>
        /// Initializes for controller.
        /// </summary>
        /// <param name="controller">The name of the controller.</param>
        /// <param name="action">The name of the action.</param>
        public void InitializeForController(string controller, string action)
        {
            if (string.IsNullOrEmpty(activeController) || (!activeController.Equals(controller)))
            {
                activeController = controller;
                lastAction = "";
                currentAction = "";
                ServerReceiveTime = DXC.Technology.Utilities.Time.FromNow();
                ServerRenderTime = DXC.Technology.Utilities.Time.FromNow();
                BrowserLoadTime = DXC.Technology.Utilities.Time.FromNow();
                BrowserSubmitTime = DXC.Technology.Utilities.Time.FromNow();
            }
            lastAction = currentAction;
            currentAction = action;
        }

        /// <summary>
        /// Registers the browser submit action.
        /// </summary>
        public void RegisterBrowserSubmitAction()
        {
            ServerReceiveTime = DXC.Technology.Utilities.Time.FromNow();
        }

        /// <summary>
        /// Calculates statistics and sets a new server render time.
        /// </summary>
        public void CalculateStatisticsAndSetNewServerRenderTime()
        {
            BrowserTime = "0.000";
            ProcessingTime = "0.000";

            string serverRenderTime = ServerRenderTime;
            if (string.IsNullOrEmpty(serverRenderTime))
                serverRenderTime = DXC.Technology.Utilities.Time.FromNow();

            ServerRenderTime = DXC.Technology.Utilities.Time.FromNow();

            if (string.IsNullOrEmpty(ServerReceiveTime))
                ServerReceiveTime = DXC.Technology.Utilities.Time.FromNow();
            if (string.IsNullOrEmpty(serverRenderTime)) return;
            if (string.IsNullOrEmpty(BrowserLoadTime))
                BrowserLoadTime = ServerReceiveTime;
            if (string.IsNullOrEmpty(BrowserSubmitTime))
                BrowserSubmitTime = serverRenderTime;

            DateTime serverRenderDateTime = DXC.Technology.Utilities.Time.ToDateTime(serverRenderTime);
            DateTime browserLoadDateTime = DXC.Technology.Utilities.Time.ToDateTime(BrowserLoadTime);

            DateTime currentBrowserSubmitDateTime = DXC.Technology.Utilities.Time.ToDateTime(BrowserSubmitTime);
            DateTime currentServerReceiveDateTime = DXC.Technology.Utilities.Time.ToDateTime(ServerReceiveTime);

            TimeSpan tsRendering = serverRenderDateTime.Subtract(browserLoadDateTime);
            TimeSpan tsSubmission = currentServerReceiveDateTime.Subtract(currentBrowserSubmitDateTime);

            // Issue with day crossover -- ignore
            if (Math.Abs(tsRendering.TotalHours) > 1) return;
            if (Math.Abs(tsSubmission.TotalHours) > 1) return;

            TimeSpan tsBrowser = tsSubmission.Add(tsRendering);
            double browserTimeInSeconds = Math.Abs(tsBrowser.TotalSeconds);
            averageBrowserTimeInSeconds = browserTimeInSeconds == 0 ? averageBrowserTimeInSeconds : 0.5 * averageBrowserTimeInSeconds / 2 + 0.5 * browserTimeInSeconds / 2;
            BrowserTime = browserTimeInSeconds.ToString("0.000");

            TimeSpan tsProcessing = DateTime.Now.Subtract(currentServerReceiveDateTime);
            ProcessingTime = tsProcessing.TotalSeconds.ToString("0.000");

            BrowserSubmitTime = "";
            BrowserLoadTime = "";
            ServerReceiveTime = "";
        }
        #endregion

        #region Private Properties
        /// <summary>
        /// Determines if the action is postback-like.
        /// </summary>
        private bool IsPostBackLikeAction
        {
            get
            {
                if (!string.IsNullOrEmpty(currentAction))
                {
                    // Same controller
                    if (currentAction.Equals(lastAction))
                    {
                        // Either a postback or a refresh
                        if (currentAction.Contains("Edit") || currentAction.Contains("Insert") || currentAction.Contains("Delete") || currentAction.Contains("Validate"))
                        {
                            // Safe assumption: it is a postback. Stop here!
                            return true;
                        }
                    }
                    else
                    {
                        // Perhaps a 'start editing or a save'
                        if (currentAction.Contains("Edit") || currentAction.Contains("Insert") || currentAction.Contains("Delete")
                            || lastAction.Contains("Edit") || lastAction.Contains("Insert") || lastAction.Contains("Delete"))
                        {
                            // Safe assumption: it is a transfer during editing
                            return true;
                        }
                        if (currentAction.Contains("View") && lastAction.Contains("Search"))
                        {
                            // Safe assumption: it is a transfer during editing
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        #endregion
    }

    public class BasicEndToEndPerformanceRecord
    {
        #region Public Properties
        /// <summary>
        /// Gets or sets the action start time.
        /// </summary>
        public DateTime? ActionStartTime { get; set; }

        /// <summary>
        /// Gets or sets the action end time.
        /// </summary>
        public DateTime? ActionEndTime { get; set; }

        /// <summary>
        /// Gets or sets the browser submit time.
        /// </summary>
        public DateTime? BrowserSubmitTime { get; set; }

        /// <summary>
        /// Gets or sets the browser receive time.
        /// </summary>
        public DateTime? BrowserReceiveTime { get; private set; }

        /// <summary>
        /// Gets or sets the browser time.
        /// </summary>
        public string BrowserTime { get; set; }

        /// <summary>
        /// Gets or sets the processing time.
        /// </summary>
        public string ProcessingTime { get; set; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Calculates statistics and sets a new server render time.
        /// </summary>
        public void CalculateStatisticsAndSetNewServerRenderTime()
        {
            if (!ActionStartTime.HasValue) ActionStartTime = DateTime.Now;
            if (!ActionEndTime.HasValue) ActionEndTime = DateTime.Now;
            BrowserReceiveTime = DateTime.Now;
            if (!BrowserSubmitTime.HasValue) BrowserSubmitTime = ActionStartTime;
            BrowserTime = "0.000";
            ProcessingTime = "0.000";
            if (!BrowserSubmitTime.HasValue) return;
            TimeSpan tsRendering = BrowserReceiveTime.Value.Subtract(BrowserSubmitTime.Value);
            TimeSpan tsSubmission = ActionEndTime.Value.Subtract(ActionStartTime.Value);

            // Issue with day crossover -- ignore
            if (Math.Abs(tsRendering.TotalHours) > 1) return;
            if (Math.Abs(tsSubmission.TotalHours) > 1) return;

            TimeSpan tsBrowser = tsSubmission.Subtract(tsRendering);
            double browserTimeInSeconds = Math.Abs(tsBrowser.TotalSeconds);
            BrowserTime = browserTimeInSeconds.ToString("0.000");

            ProcessingTime = tsSubmission.TotalSeconds.ToString("0.000");
        }
        #endregion
    }
}