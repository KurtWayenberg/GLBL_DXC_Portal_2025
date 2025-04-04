using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text;

namespace DXC.Technology.Data
{
    [DataContract(IsReference = true)]
    public class SearchCriteriaBase
    {
        #region Static Fields
        /// <summary>
        /// Represents the sort direction for search criteria.
        /// </summary>
        private enum SearchCriteriaSortDirection
        {
            ASC,
            DESC
        }
        #endregion

        #region Instance Fields
        /// <summary>
        /// The number of rows per page.
        /// </summary>
        private int numberOfRowsPerPage = int.MaxValue;

        /// <summary>
        /// The current page number.
        /// </summary>
        private int pageNumber = 0;

        /// <summary>
        /// The maximum number of rows.
        /// </summary>
        private int maxRows = 0;

        /// <summary>
        /// The list of sort criteria.
        /// </summary>
        private List<SearchCriterion> sortCriteria = new List<SearchCriterion>();
        #endregion

        #region Nested Classes
        /// <summary>
        /// Represents a single search criterion.
        /// </summary>
        private class SearchCriterion
        {
            /// <summary>
            /// The field to sort by.
            /// </summary>
            public string Field { get; set; }

            /// <summary>
            /// The sort direction for the field.
            /// </summary>
            public SearchCriteriaSortDirection SortDirection { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="SearchCriterion"/> class with ascending sort direction.
            /// </summary>
            /// <param name="field">The field to sort by.</param>
            public SearchCriterion(string field)
            {
                Field = field;
                SortDirection = SearchCriteriaSortDirection.ASC;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="SearchCriterion"/> class with specified sort direction.
            /// </summary>
            /// <param name="field">The field to sort by.</param>
            /// <param name="sortDirection">The sort direction.</param>
            public SearchCriterion(string field, SearchCriteriaSortDirection sortDirection)
            {
                Field = field;
                SortDirection = sortDirection;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCriteriaBase"/> class with default maximum rows.
        /// </summary>
        public SearchCriteriaBase()
        {
            maxRows = 1000;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCriteriaBase"/> class with specified maximum rows.
        /// </summary>
        /// <param name="maxRows">The maximum number of rows.</param>
        public SearchCriteriaBase(int maxRows)
        {
            this.maxRows = maxRows;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCriteriaBase"/> class with specified page number and rows per page.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="numberOfRowsPerPage">The number of rows per page.</param>
        public SearchCriteriaBase(int pageNumber, int numberOfRowsPerPage) : this()
        {
            this.numberOfRowsPerPage = numberOfRowsPerPage;
            this.pageNumber = pageNumber;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchCriteriaBase"/> class with specified maximum rows, page number, and rows per page.
        /// </summary>
        /// <param name="maxRows">The maximum number of rows.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="numberOfRowsPerPage">The number of rows per page.</param>
        public SearchCriteriaBase(int maxRows, int pageNumber, int numberOfRowsPerPage) : this(maxRows)
        {
            this.numberOfRowsPerPage = numberOfRowsPerPage;
            this.pageNumber = pageNumber;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the number of rows per page.
        /// </summary>
        [DataMember]
        public int NumberOfRowsPerPage
        {
            get => numberOfRowsPerPage;
            set => numberOfRowsPerPage = value;
        }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [DataMember]
        public int PageNumber
        {
            get => pageNumber;
            set => pageNumber = value;
        }

        /// <summary>
        /// Gets or sets the maximum number of rows.
        /// </summary>
        public int MaxRows
        {
            get => maxRows;
            set => maxRows = value;
        }

        /// <summary>
        /// Gets or sets the search criteria as a serialized string.
        /// </summary>
        [DataMember]
        public string SearchCriteria
        {
            get
            {
                using var sw = new System.IO.StringWriter();
                foreach (var sc in sortCriteria)
                {
                    sw.Write(sc.Field);
                    sw.Write("|");
                    sw.Write(sc.SortDirection);
                    sw.Write(";");
                }
                return sw.ToString();
            }
            set
            {
                sortCriteria = new List<SearchCriterion>();
                if (!string.IsNullOrEmpty(value))
                {
                    foreach (var sc in value.Split(';'))
                    {
                        var sandc = sc.Split('|').ToList();
                        if (!Enum.TryParse(sandc.Last(), out SearchCriteriaSortDirection sortDirection))
                            sortDirection = SearchCriteriaSortDirection.ASC;
                        sortCriteria.Add(new SearchCriterion(sandc.First(), sortDirection));
                    }
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the default maximum rows for a timeslot.
        /// </summary>
        /// <param name="defaultMaxRows">The default maximum rows.</param>
        public void SetDefaultMaxRowsForTimeslot(int defaultMaxRows)
        {
            maxRows = defaultMaxRows;
        }

        /// <summary>
        /// Adds a sort criterion with ascending order.
        /// </summary>
        /// <param name="propertyName">The property name to sort by.</param>
        public void AddSortCriteria(string propertyName)
        {
            sortCriteria.Add(new SearchCriterion(propertyName));
        }

        /// <summary>
        /// Adds a sort criterion with descending order.
        /// </summary>
        /// <param name="propertyName">The property name to sort by.</param>
        public void AddSortCriteriaDescending(string propertyName)
        {
            sortCriteria.Add(new SearchCriterion(propertyName, SearchCriteriaSortDirection.DESC));
        }

        /// <summary>
        /// Defines multiple sort criteria with ascending order.
        /// </summary>
        /// <param name="propertyNames">The property names to sort by.</param>
        public void DefineSortCriteria(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                AddSortCriteria(propertyName);
            }
        }

        /// <summary>
        /// Defines multiple sort criteria with descending order.
        /// </summary>
        /// <param name="propertyNames">The property names to sort by.</param>
        public void DefineSortCriteriaDescending(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                AddSortCriteriaDescending(propertyName);
            }
        }

        /// <summary>
        /// Checks the maximum number of results in a queryable.
        /// </summary>
        /// <typeparam name="T">The type of the queryable.</typeparam>
        /// <param name="queryable">The queryable to check.</param>
        public void CheckMaxNumberOfResults<T>(IQueryable<T> queryable)
        {
            if (maxRows < 0)
                maxRows = DXC.Technology.Timeslot.TimeslotSettingManager.SettingForTimeslot(DXC.Technology.Timeslot.TimeslotSettingTypeEnum.Search, "");
            if (MaxRows < NumberOfRowsPerPage) return;
            var take = Convert.ToInt32(MaxRows) + 1;
            if (queryable.Take(take).Count() == take)
                throw new DXC.Technology.Exceptions.NamedExceptions.CountLimitExceededException(take - 1, "Refine your criteria");
        }

        /// <summary>
        /// Adds paging and dynamic order-by logic to a queryable.
        /// </summary>
        /// <typeparam name="T">The type of the queryable.</typeparam>
        /// <param name="queryable">The queryable to modify.</param>
        /// <returns>The modified queryable.</returns>
        public IQueryable<T> AddPagingAndDynamicOrderByLogic<T>(IQueryable<T> queryable)
        {
            var result = AddOrderByCriteria(queryable);
            var skip = 0;
            if (PageNumber > 0)
            {
                skip = NumberOfRowsPerPage * PageNumber;
            }
            result = result.Skip(skip).Take(NumberOfRowsPerPage);
            return result;
        }

        /// <summary>
        /// Converts the search criteria to a query string.
        /// </summary>
        /// <param name="includePagingFields">Whether to include paging fields in the query string.</param>
        /// <returns>The query string representation of the search criteria.</returns>
        public string AsQueryString(bool includePagingFields = false)
        {
            var pagingFieldNames = new[] { "NumberOfRowsPerPage", "PageNumber", "MaxRows", "SearchCriteriaSortDirection" };
            var type = GetType();
            var flags = System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            using var sw = new System.IO.StringWriter();
            var first = true;
            foreach (var propertyInfo in type.GetProperties(flags)
                .Where(p => Attribute.IsDefined(p, typeof(DataMemberAttribute)))
                .OrderBy(p => p.Name))
            {
                if (!includePagingFields && pagingFieldNames.Contains(propertyInfo.Name)) continue;
                var fieldIdentifier = propertyInfo.Name;
                var value = Convert.ToString(type.InvokeMember(propertyInfo.Name, flags, null, this, null, DXC.Technology.Utilities.CultureInfoProvider.Default));
                if (!string.IsNullOrEmpty(value))
                {
                    if (first)
                        sw.Write("?");
                    else
                        sw.Write("&");
                    sw.Write(propertyInfo.Name);
                    sw.Write("=");
                    sw.Write(value);
                    first = false;
                }
            }
            return sw.ToString();
        }

        /// <summary>
        /// Loads search criteria from a query string.
        /// </summary>
        /// <param name="queryString">The query string to load.</param>
        public void LoadQueryString(string queryString)
        {
            if (string.IsNullOrEmpty(queryString)) return;
            var partsAndValues = queryString.Trim().Replace("?", "").Split('&');
            var partsAndValuesDictionary = new Dictionary<string, string>();
            foreach (var partAndValue in partsAndValues)
            {
                var partAndValueParts = partAndValue.Split('=');
                partsAndValuesDictionary.Add(partAndValueParts[0], partAndValueParts[1]);
            }

            var type = GetType();
            var flags = System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public;
            foreach (var propertyInfo in type.GetProperties(flags).OrderBy(p => p.Name))
            {
                if (partsAndValuesDictionary.ContainsKey(propertyInfo.Name))
                {
                    if (propertyInfo.PropertyType.UnderlyingSystemType.FullName.Contains("Int32"))
                        propertyInfo.SetValue(this, Convert.ToInt32(partsAndValuesDictionary[propertyInfo.Name]));
                    else if (propertyInfo.PropertyType.UnderlyingSystemType.FullName.Contains("Int64"))
                        propertyInfo.SetValue(this, Convert.ToInt64(partsAndValuesDictionary[propertyInfo.Name]));
                    else if (propertyInfo.PropertyType.UnderlyingSystemType.FullName.Contains("Int16"))
                        propertyInfo.SetValue(this, Convert.ToInt16(partsAndValuesDictionary[propertyInfo.Name]));
                    else if (propertyInfo.PropertyType.UnderlyingSystemType.FullName.Contains("DateTime"))
                        propertyInfo.SetValue(this, Convert.ToDateTime(partsAndValuesDictionary[propertyInfo.Name]));
                    else if (propertyInfo.PropertyType.UnderlyingSystemType.FullName.Contains("Bool"))
                        propertyInfo.SetValue(this, Convert.ToBoolean(partsAndValuesDictionary[propertyInfo.Name]));
                    else
                        propertyInfo.SetValue(this, partsAndValuesDictionary[propertyInfo.Name]);
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Adds order-by criteria to a queryable.
        /// </summary>
        /// <typeparam name="T">The type of the queryable.</typeparam>
        /// <param name="queryable">The queryable to modify.</param>
        /// <returns>The modified queryable.</returns>
        private IQueryable<T> AddOrderByCriteria<T>(IQueryable<T> queryable)
        {
            var param = Expression.Parameter(typeof(T), "p");
            Expression parent = param;

            if (sortCriteria.Count == 0) return queryable;

            if (sortCriteria.Count == 1)
            {
                var parts = sortCriteria[0].Field.Split('.');
                foreach (var part in parts)
                {
                    parent = Expression.Property(parent, part);
                }

                return sortCriteria[0].SortDirection == SearchCriteriaSortDirection.ASC
                    ? queryable.OrderBy(Expression.Lambda<Func<T, object>>(parent, param))
                    : queryable.OrderByDescending(Expression.Lambda<Func<T, object>>(parent, param));
            }
            else
            {
                var result = queryable;
                foreach (var sc in sortCriteria)
                {
                    Expression expression = param;
                    var parts = sc.Field.Split('.');
                    foreach (var part in parts)
                    {
                        expression = Expression.Property(parent, part);
                    }

                    result = sc.SortDirection == SearchCriteriaSortDirection.ASC
                        ? result.OrderBy(Expression.Lambda<Func<T, object>>(expression, param))
                        : result.OrderByDescending(Expression.Lambda<Func<T, object>>(expression, param));
                }
                return result;
            }
        }

        /// <summary>
        /// Initializes the criteria for the next page.
        /// </summary>
        public void InitializeForNextPage()
        {
            if (PageNumber < 0) PageNumber = 0;
            if (NumberOfRowsPerPage < int.MaxValue) PageNumber++;
        }

        /// <summary>
        /// Initializes the criteria for the previous page.
        /// </summary>
        public void InitializeForPreviousPage()
        {
            if (PageNumber > 0)
                PageNumber--;
            else
                PageNumber = 0;
        }
        #endregion
    }
}