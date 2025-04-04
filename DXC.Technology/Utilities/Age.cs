using System;

namespace DXC.Technology.Utilities
{
    /// <summary>
    /// Represents an age calculation utility.
    /// </summary>
    public class Age
    {
        #region Instance Fields

        /// <summary>
        /// Represents the number of years.
        /// </summary>
        public int Years { get; private set; }

        /// <summary>
        /// Represents the number of months.
        /// </summary>
        public int Months { get; private set; }

        /// <summary>
        /// Represents the number of days.
        /// </summary>
        public int Days { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Age"/> class and calculates the age based on the provided birthday.
        /// </summary>
        /// <param name="birthday">The birthday date.</param>
        public Age(DateTime birthday)
        {
            Count(birthday);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Age"/> class and calculates the age based on the provided birthday and comparison date.
        /// </summary>
        /// <param name="birthday">The birthday date.</param>
        /// <param name="comparisonDate">The comparison date.</param>
        public Age(DateTime birthday, DateTime comparisonDate)
        {
            Count(birthday, comparisonDate);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Calculates the age based on the provided birthday and the current date.
        /// </summary>
        /// <param name="birthday">The birthday date.</param>
        /// <returns>The calculated <see cref="Age"/>.</returns>
        public Age Count(DateTime birthday)
        {
            return Count(birthday, DateTime.Today);
        }

        /// <summary>
        /// Calculates the age based on the provided birthday and comparison date.
        /// </summary>
        /// <param name="birthday">The birthday date.</param>
        /// <param name="comparisonDate">The comparison date.</param>
        /// <returns>The calculated <see cref="Age"/>.</returns>
        public Age Count(DateTime birthday, DateTime comparisonDate)
        {
            if ((comparisonDate.Year - birthday.Year) > 0 ||
                (((comparisonDate.Year - birthday.Year) == 0) && ((birthday.Month < comparisonDate.Month) ||
                  ((birthday.Month == comparisonDate.Month) && (birthday.Day <= comparisonDate.Day)))))
            {
                int daysInBirthdayMonth = DateTime.DaysInMonth(birthday.Year, birthday.Month);
                int daysRemain = comparisonDate.Day + (daysInBirthdayMonth - birthday.Day);

                if (comparisonDate.Month > birthday.Month)
                {
                    Years = comparisonDate.Year - birthday.Year;
                    Months = comparisonDate.Month - (birthday.Month + 1) + Math.Abs(daysRemain / daysInBirthdayMonth);
                    Days = (daysRemain % daysInBirthdayMonth + daysInBirthdayMonth) % daysInBirthdayMonth;
                }
                else if (comparisonDate.Month == birthday.Month)
                {
                    if (comparisonDate.Day >= birthday.Day)
                    {
                        Years = comparisonDate.Year - birthday.Year;
                        Months = 0;
                        Days = comparisonDate.Day - birthday.Day;
                    }
                    else
                    {
                        Years = (comparisonDate.Year - 1) - birthday.Year;
                        Months = 11;
                        Days = DateTime.DaysInMonth(birthday.Year, birthday.Month) - (birthday.Day - comparisonDate.Day);
                    }
                }
                else
                {
                    Years = (comparisonDate.Year - 1) - birthday.Year;
                    Months = comparisonDate.Month + (11 - birthday.Month) + Math.Abs(daysRemain / daysInBirthdayMonth);
                    Days = (daysRemain % daysInBirthdayMonth + daysInBirthdayMonth) % daysInBirthdayMonth;
                }
            }
            else
            {
                throw new ArgumentException("Birthday date must be earlier than current date");
            }
            return this;
        }

        #endregion
    }
}