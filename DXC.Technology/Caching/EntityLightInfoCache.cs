using System;
using System.Collections.Generic;

namespace DXC.Technology.Caching
{
    #region Interfaces

    /// <summary>
    /// Interface for providing entity light info.
    /// </summary>
    public interface IEntityLightInfoProvider
    {
        /// <summary>
        /// Retrieves entity light info by ID.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity light info.</returns>
        EntityLightInfo GetEntityLightInfoById(string entityType, long id);

        /// <summary>
        /// Retrieves entity light info by code.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="code">The code of the entity.</param>
        /// <returns>The entity light info.</returns>
        EntityLightInfo GetEntityLightInfoByCode(string entityType, string code);
    }

    #endregion

    #region Classes

    /// <summary>
    /// Default implementation of IEntityLightInfoProvider.
    /// </summary>
    public class DefaultEntityLightInfoProvider : IEntityLightInfoProvider
    {
        #region Public Methods

        /// <summary>
        /// Retrieves entity light info by ID.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity light info.</returns>
        public EntityLightInfo GetEntityLightInfoById(string entityType, long id)
        {
            return new EntityLightInfo
            {
                Id = id,
                Code = id.ToString(DXC.Technology.Utilities.StringFormatProvider.Default),
                EntityLightInfoType = ""
            };
        }

        /// <summary>
        /// Retrieves entity light info by code.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="code">The code of the entity.</param>
        /// <returns>The entity light info.</returns>
        public EntityLightInfo GetEntityLightInfoByCode(string entityType, string code)
        {
            if (long.TryParse(code, out long parsedId))
            {
                return new EntityLightInfo
                {
                    Id = parsedId,
                    Code = code,
                    EntityLightInfoType = ""
                };
            }
            else
            {
                return new EntityLightInfo
                {
                    Id = DateTime.Now.Millisecond,
                    Code = code,
                    EntityLightInfoType = ""
                };
            }
        }

        #endregion
    }

    /// <summary>
    /// Cache for entity light info.
    /// </summary>
    public class EntityLightInfoCache
    {
        #region Static Fields

        /// <summary>
        /// Static field for the entity light info provider.
        /// </summary>
        private static IEntityLightInfoProvider entityLightInfoProvider = new DefaultEntityLightInfoProvider();

        #endregion

        #region Instance Fields

        /// <summary>
        /// Cache for entity light info dictionaries.
        /// </summary>
        private Dictionary<string, EntityLightInfoDictionaries> entityLightInfoDictionaries { get; set; } = new();

        /// <summary>
        /// Counter for lookups performed.
        /// </summary>
        private int lookupsPerformed { get; set; } = 0;

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Property for accessing the entity light info provider.
        /// </summary>
        public static IEntityLightInfoProvider EntityLightInfoProvider
        {
            get => entityLightInfoProvider;
            set => entityLightInfoProvider = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves the ID for a given entity code.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="code">The code of the entity.</param>
        /// <returns>The ID of the entity.</returns>
        public long GetIdForEntityCode(string entityType, string code) => GetEntityLightInfoByCode(entityType, code).Id;

        /// <summary>
        /// Retrieves the entity type for a given entity code.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="code">The code of the entity.</param>
        /// <returns>The entity type.</returns>
        public string GetEntityTypeForEntityCode(string entityType, string code) => GetEntityLightInfoByCode(entityType, code).EntityLightInfoType;

        /// <summary>
        /// Retrieves the name for a given entity code.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="code">The code of the entity.</param>
        /// <returns>The name of the entity.</returns>
        public string GetNameForEntityCode(string entityType, string code) => GetEntityLightInfoByCode(entityType, code).Name;

        /// <summary>
        /// Retrieves a specific info element for a given entity code.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="code">The code of the entity.</param>
        /// <param name="infoElement">The info element to retrieve.</param>
        /// <returns>The info element value.</returns>
        public string GetInfoElementForEntityCode(string entityType, string code, string infoElement) => GetEntityLightInfoByCode(entityType, code).GetEntityInfoElement(infoElement);

        /// <summary>
        /// Retrieves the code for a given entity ID.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The code of the entity.</returns>
        public string GetCodeForEntityId(string entityType, long id) => GetEntityLightInfoById(entityType, id).Code;

        /// <summary>
        /// Retrieves the name for a given entity ID.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The name of the entity.</returns>
        public string GetNameForEntityId(string entityType, long id) => GetEntityLightInfoById(entityType, id).Name;

        /// <summary>
        /// Retrieves the entity type for a given entity ID.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity type.</returns>
        public string GetEntityTypeForEntityId(string entityType, long id) => GetEntityLightInfoById(entityType, id).EntityLightInfoType;

        /// <summary>
        /// Retrieves a specific info element for a given entity ID.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <param name="infoElement">The info element to retrieve.</param>
        /// <returns>The info element value.</returns>
        public string GetInfoElementForEntityId(string entityType, long id, string infoElement) => GetEntityLightInfoById(entityType, id).GetEntityInfoElement(infoElement);

        #endregion

        #region Private Methods

        /// <summary>
        /// Retrieves entity light info by ID.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="id">The ID of the entity.</param>
        /// <returns>The entity light info.</returns>
        private EntityLightInfo GetEntityLightInfoById(string entityType, long id)
        {
            CleanupCaches();
            if (!entityLightInfoDictionaries.ContainsKey(entityType))
            {
                entityLightInfoDictionaries.Add(entityType, new EntityLightInfoDictionaries());
            }

            if (!entityLightInfoDictionaries[entityType].EntityLightInfosById.ContainsKey(id))
            {
                entityLightInfoDictionaries[entityType].EntityLightInfosById.Add(id, entityLightInfoProvider.GetEntityLightInfoById(entityType, id));
                lookupsPerformed++;
            }

            return entityLightInfoDictionaries[entityType].EntityLightInfosById[id];
        }

        /// <summary>
        /// Retrieves entity light info by code.
        /// </summary>
        /// <param name="entityType">The type of the entity.</param>
        /// <param name="code">The code of the entity.</param>
        /// <returns>The entity light info.</returns>
        private EntityLightInfo GetEntityLightInfoByCode(string entityType, string code)
        {
            if (string.IsNullOrEmpty(code)) return new EntityLightInfo();

            CleanupCaches();
            if (!entityLightInfoDictionaries.ContainsKey(entityType))
            {
                entityLightInfoDictionaries.Add(entityType, new EntityLightInfoDictionaries());
            }

            if (!entityLightInfoDictionaries[entityType].EntityLightInfosByCode.ContainsKey(code))
            {
                entityLightInfoDictionaries[entityType].EntityLightInfosByCode.Add(code, entityLightInfoProvider.GetEntityLightInfoByCode(entityType, code));
                lookupsPerformed++;
            }

            return entityLightInfoDictionaries[entityType].EntityLightInfosByCode[code];
        }

        /// <summary>
        /// Cleans up the caches if the lookup count exceeds a threshold.
        /// </summary>
        private void CleanupCaches()
        {
            if (lookupsPerformed < 5000) return;

            entityLightInfoDictionaries = new Dictionary<string, EntityLightInfoDictionaries>();
            lookupsPerformed = 0;
        }

        #endregion
    }

    /// <summary>
    /// Represents a collection of entity light info dictionaries.
    /// </summary>
    internal class EntityLightInfoDictionaries
    {
        #region Public Properties

        /// <summary>
        /// Dictionary for entity light infos by ID.
        /// </summary>
        public Dictionary<long, EntityLightInfo> EntityLightInfosById { get; } = new();

        /// <summary>
        /// Dictionary for entity light infos by code.
        /// </summary>
        public Dictionary<string, EntityLightInfo> EntityLightInfosByCode { get; } = new();

        #endregion
    }

    /// <summary>
    /// Represents entity light info.
    /// </summary>
    public class EntityLightInfo
    {
        #region Public Properties

        /// <summary>
        /// ID of the entity.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Code of the entity.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Name of the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type of the entity light info.
        /// </summary>
        public string EntityLightInfoType { get; set; }

        /// <summary>
        /// Dictionary for entity info elements.
        /// </summary>
        public Dictionary<string, string> EntityInfoElements { get; } = new();

        #endregion

        #region Public Methods

        /// <summary>
        /// Retrieves a specific entity info element.
        /// </summary>
        /// <param name="infoElement">The info element to retrieve.</param>
        /// <returns>The info element value.</returns>
        public string GetEntityInfoElement(string infoElement)
        {
            if (EntityInfoElements == null || !EntityInfoElements.ContainsKey(infoElement)) return "";

            return EntityInfoElements[infoElement];
        }

        #endregion
    }

    #endregion
}