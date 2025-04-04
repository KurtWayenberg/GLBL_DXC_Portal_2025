using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace DXC.Technology.Entity
{
    #region Static Fields

    /// <summary>
    /// Represents constants used in the EntityHelper.
    /// </summary>
    public static class EntityHelperConstants
    {
        /// <summary>
        /// Represents an empty integer value.
        /// </summary>
        public const int EmptyInt = Int32.MinValue;

        /// <summary>
        /// Represents an empty long value.
        /// </summary>
        public const long EmptyLong = Int64.MinValue;

        /// <summary>
        /// Represents an empty string value.
        /// </summary>
        public const string EmptyString = "*** EMPTY ***";
    }

    #endregion

    #region Attribute Class

    /// <summary>
    /// Specifies that a property or method should never be copied.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property | System.AttributeTargets.Method)]
    public class NeverCopyAttribute : System.Attribute
    {
    }

    #endregion

    public class EntityHelper
    {
        #region Private Methods

        /// <summary>
        /// Retrieves the default value for a given property type.
        /// </summary>
        /// <param name="type">The type of the property as a string.</param>
        /// <returns>The default value for the property type.</returns>
        private static object GetDefaultValueForPropertyType(string type)
        {
            return type switch
            {
                "Int64" => 0,
                _ => null,
            };
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Copies properties from one entity to another, excluding specified properties.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="copyFrom">The source object to copy from.</param>
        /// <param name="copyInto">The target object to copy into.</param>
        /// <param name="excludedProperties">Properties to exclude from copying.</param>
        public static void CopyEntity<T>(object copyFrom, object copyInto, params string[] excludedProperties)
            where T : class
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                try
                {
                    var valueToCopy = propertyInfo.GetValue(copyFrom);
                    if ((valueToCopy != null) && ((Convert.ToString(valueToCopy)) != "0") && propertyInfo.CanWrite
                        && (!propertyInfo.Name.EndsWith("_StringWrapper"))
                        && (!propertyInfo.PropertyType.BaseType?.Name.Contains("Array") ?? true)
                        && (!propertyInfo.PropertyType.BaseType?.Name.Contains("Collection") ?? true)
                        && (!propertyInfo.PropertyType.BaseType?.Name.Contains("List") ?? true)
                        && (!propertyInfo.PropertyType.Name.Contains("Array"))
                        && (!propertyInfo.PropertyType.Name.Contains("Collection"))
                        && (!propertyInfo.PropertyType.Name.Contains("List"))
                        && !Attribute.IsDefined(propertyInfo, typeof(NeverCopyAttribute))
                        && ((excludedProperties == null) || (!excludedProperties.Contains(propertyInfo.Name))))
                    {
                        propertyInfo.SetValue(copyInto, valueToCopy, null);
                    }
                }
                catch (Exception ex)
                {
                    DXC.Technology.Exceptions.ExceptionHelper.Publish(ex);
                    //throw;
                }
            }
        }

        /// <summary>
        /// Creates a deep copy of an entity using serialization.
        /// </summary>
        /// <typeparam name="T">The type of the entity.</typeparam>
        /// <param name="objectCopy">The object to copy.</param>
        /// <returns>A deep copy of the object.</returns>
        public static T CopyEntity<T>(object objectCopy)
            where T : class
        {
            string deserializedString = DXC.Technology.Objects.SerializationHelper.DataContractSerialize(objectCopy);
            return DXC.Technology.Objects.SerializationHelper.DataContractDeserialize<T>(deserializedString);
        }

        /// <summary>
        /// Performs a best-effort copy of properties between two different types.
        /// </summary>
        /// <typeparam name="T">The source type.</typeparam>
        /// <typeparam name="U">The target type.</typeparam>
        /// <param name="copyFrom">The source object to copy from.</param>
        /// <param name="copyInto">The target object to copy into.</param>
        public static void BestEffortCopy<T, U>(object copyFrom, object copyInto)
            where T : class
            where U : class
        {
            var sourceProperties = typeof(T).GetProperties().ToDictionary(p => p.Name);
            var targetProperties = typeof(U).GetProperties().ToDictionary(p => p.Name);

            foreach (var name in sourceProperties.Keys)
            {
                if (targetProperties.ContainsKey(name))
                {
                    var sourcePropertyInfo = sourceProperties[name];
                    var targetPropertyInfo = targetProperties[name];
                    var valueToCopy = sourcePropertyInfo.GetValue(copyFrom, null);
                    if ((valueToCopy != null) && targetPropertyInfo.CanWrite && (!targetPropertyInfo.PropertyType.Name.StartsWith("TrackableCollection", StringComparison.OrdinalIgnoreCase)))
                    {
                        targetPropertyInfo.SetValue(copyInto, valueToCopy, null);
                    }
                }
            }
        }

        /// <summary>
        /// Merges two objects into a dynamic object, excluding specified properties.
        /// </summary>
        /// <param name="item1">The first object to merge.</param>
        /// <param name="item2">The second object to merge.</param>
        /// <param name="excludedProperties">Properties to exclude from the merge.</param>
        /// <returns>A dynamic object containing merged properties.</returns>
        public static dynamic Merge(object item1, object item2, params string[] excludedProperties)
        {
            if (item1 == null || item2 == null)
                return item1 ?? item2 ?? new ExpandoObject();

            dynamic expando = new ExpandoObject();
            var result = expando as IDictionary<string, object>;
            foreach (var propertyInfo in item1.GetType().GetProperties())
            {
                if ((excludedProperties == null) || (!excludedProperties.Contains(propertyInfo.Name)))
                    result[propertyInfo.Name] = propertyInfo.GetValue(item1, null);
            }
            foreach (var propertyInfo in item2.GetType().GetProperties())
            {
                if ((excludedProperties == null) || (!excludedProperties.Contains(propertyInfo.Name)))
                    result[propertyInfo.Name] = propertyInfo.GetValue(item2, null);
            }
            return result;
        }

        #endregion
    }
}