using System;

namespace DXC.Technology.Utilities
{
    public class SearchRangeHelper
    {
        #region Private Properties

        /// <summary>
        /// The starting date/time of the search range.
        /// </summary>
        private DateTime from { get; set; }

        /// <summary>
        /// The ending date/time of the search range.
        /// </summary>
        private DateTime to { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRangeHelper"/> class with a specified range.
        /// </summary>
        /// <param name="from">The starting date/time.</param>
        /// <param name="to">The ending date/time.</param>
        public SearchRangeHelper(DateTime from, DateTime to)
        {
            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRangeHelper"/> class with a specified range and maximum duration.
        /// </summary>
        /// <param name="from">The starting date/time.</param>
        /// <param name="to">The ending date/time.</param>
        /// <param name="totalNumberOfMinutesMax">The maximum duration in minutes.</param>
        public SearchRangeHelper(DateTime from, DateTime to, int totalNumberOfMinutesMax)
        {
            this.from = from;
            this.to = from.AddMinutes(Convert.ToDouble(totalNumberOfMinutesMax));
            if (this.to > to)
                this.to = to;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the next search range by splitting the current range in half.
        /// </summary>
        /// <returns>A new <see cref="SearchRange"/> instance representing the next range.</returns>
        public SearchRange GetNextSearchRange()
        {
            SearchRange result = new SearchRange(from, to);
            to = from.AddMilliseconds(to.Subtract(from).TotalMilliseconds / 2);
            return result;
        }

        #endregion
    }

    public class SearchRange
    {
        #region Public Properties

        /// <summary>
        /// Gets the starting date/time of the range.
        /// </summary>
        public DateTime From { get; private set; }

        /// <summary>
        /// Gets the ending date/time of the range.
        /// </summary>
        public DateTime To { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchRange"/> class.
        /// </summary>
        /// <param name="from">The starting date/time.</param>
        /// <param name="to">The ending date/time.</param>
        /// <exception cref="Exception">Thrown when the "from" or "to" parameters are null or invalid.</exception>
        public SearchRange(DateTime from, DateTime to)
        {
            if (from == null || to == null) throw new Exception("From/To cannot be null");
            if (from >= to) throw new Exception("From < To  - required");
            From = from;
            To = to;
        }

        #endregion
    }
}