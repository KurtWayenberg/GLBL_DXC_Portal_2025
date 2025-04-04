using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DXC.Technology.Exceptions
{
    [DataContract]
    public class TechnologyFaultContractException
    {
        #region Instance Fields

        // No instance fields in this class.

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TechnologyFaultContractException"/> class.
        /// </summary>
        /// <param name="errorMessage">The error message associated with the exception.</param>
        /// <param name="exceptionDefinitionString">The definition string of the exception.</param>
        public TechnologyFaultContractException(string errorMessage, string exceptionDefinitionString)
        {
            ErrorMessage = errorMessage;
            ExceptionDefinitionString = exceptionDefinitionString;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The error message associated with the exception.
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The definition string of the exception.
        /// </summary>
        [DataMember]
        public string ExceptionDefinitionString { get; set; }

        #endregion

        #region Public Methods

        // No public methods in this class.

        #endregion

        #region Overridden Methods

        // No overridden methods in this class.

        #endregion

        #region Protected Properties

        // No protected properties in this class.

        #endregion

        #region Protected Methods

        // No protected methods in this class.

        #endregion

        #region Private Properties

        // No private properties in this class.

        #endregion

        #region Private Methods

        // No private methods in this class.

        #endregion
    }
}