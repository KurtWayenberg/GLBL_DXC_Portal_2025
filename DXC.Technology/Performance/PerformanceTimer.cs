using System;

namespace DXC.Technology.Performance
{
    /// <summary>
    /// Simple Timer to help measure call-durations
    /// </summary>
    public class PerformanceTimer
    {
        #region Instance Fields

        /// <summary>
        /// The start time of the timer
        /// </summary>
        private DateTime startTime;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceTimer"/> class.
        /// </summary>
        public PerformanceTimer()
        {
            Reset();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceTimer"/> class with a specific start time.
        /// </summary>
        /// <param name="startDateTime">The start time to initialize the timer with.</param>
        public PerformanceTimer(DateTime startDateTime)
        {
            Reset(startDateTime);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the start time of the timer.
        /// </summary>
        public DateTime StartTime => startTime;

        #endregion

        #region Public Methods

        /// <summary>
        /// Resets the timer to the current time.
        /// </summary>
        public void Reset()
        {
            startTime = DateTime.Now;
        }

        /// <summary>
        /// Resets the timer to a specific start time.
        /// </summary>
        /// <param name="startDateTime">The start time to reset to.</param>
        public void Reset(DateTime startDateTime)
        {
            startTime = startDateTime;
        }

        /// <summary>
        /// Calculates the elapsed time in milliseconds since the timer started.
        /// </summary>
        /// <returns>Elapsed time in milliseconds.</returns>
        public double ElapsedTimeInMilliseconds()
        {
            TimeSpan elapsed = DateTime.Now.Subtract(startTime);
            return elapsed.TotalMilliseconds;
        }

        /// <summary>
        /// Calculates the elapsed time in milliseconds until a specific time.
        /// </summary>
        /// <param name="untilDateTime">The time to calculate elapsed time until.</param>
        /// <returns>Elapsed time in milliseconds.</returns>
        public double ElapsedTimeInMilliseconds(DateTime untilDateTime)
        {
            TimeSpan elapsed = untilDateTime.Subtract(startTime);
            return elapsed.TotalMilliseconds;
        }

        #endregion
    }
}