using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    /// <summary>
    /// Manages the attributes for a game character
    /// </summary>
    public class AttributeSystemComponent : MonoBehaviour
    {
        [SerializeField] private AbstractAttributeEventHandler[] attributeSystemEvents;

        /// <summary>
        /// Attribute sets assigned to the game character
        /// </summary>
        [SerializeField] private List<AttributeScriptableObject> attributeScriptableObjects;

        [SerializeField] private List<AttributeValue> attributeValues;

        private bool _isAttributeDictStale;

        public Dictionary<AttributeScriptableObject, int> MAttributeIndexCache { get; private set; } =
            new Dictionary<AttributeScriptableObject, int>();

        /// <summary>
        /// Marks attribute cache dirty, so it can be recreated next time it is required
        /// </summary>
        public void MarkAttributesDirty()
        {
            _isAttributeDictStale = true;
        }

        /// <summary>
        /// Gets the value of an attribute.  Note that the returned value is a copy of the struct, so modifying it
        /// does not modify the original attribute
        /// </summary>
        /// <param name="attribute">Attribute to get value for</param>
        /// <param name="value">Returned attribute</param>
        /// <returns>True if attribute was found, false otherwise.</returns>
        public bool GetAttributeValue(AttributeScriptableObject attribute, out AttributeValue value)
        {
            // If dictionary is stale, rebuild it
            var attributeCache = GetAttributeCache();

            // We use a cache to store the index of the attribute in the list, so we don't
            // have to iterate through it every time
            if (attributeCache.TryGetValue(attribute, out var index))
            {
                value = attributeValues[index];
                return true;
            }
            
            // No matching attribute found
            value = new AttributeValue();
            return false;
        }

        public void SetAttributeBaseValue(AttributeScriptableObject attribute, float value)
        {
            // If dictionary is stale, rebuild it
            var attributeCache = GetAttributeCache();
            if (!attributeCache.TryGetValue(attribute, out var index)) return;
            var attributeValue = attributeValues[index];
            attributeValue.baseValue = value;
            attributeValues[index] = attributeValue;
        }

        /// <summary>
        /// Sets value of an attribute.  Note that the out value is a copy of the struct, so modifying it
        /// does not modify the original attribute
        /// </summary>
        /// <param name="attribute">Attribute to set</param>
        /// <param name="modifier">How to modify the attribute</param>
        /// <param name="value">Copy of newly modified attribute</param>
        /// <returns>True, if attribute was found.</returns>
        public bool UpdateAttributeModifiers(AttributeScriptableObject attribute, AttributeModifier modifier,
            out AttributeValue value)
        {
            // If dictionary is stale, rebuild it
            var attributeCache = GetAttributeCache();

            // We use a cache to store the index of the attribute in the list, so we don't
            // have to iterate through it every time
            if (attributeCache.TryGetValue(attribute, out var index))
            {
                // Get a copy of the attribute value struct
                value = attributeValues[index];
                value.modifier = value.modifier.Combine(modifier);

                // Structs are copied by value, so the modified attribute needs to be reassigned to the array
                attributeValues[index] = value;
                return true;
            }

            // No matching attribute found
            value = new AttributeValue();
            return false;
        }

        /// <summary>
        /// Add attributes to this attribute system.  Duplicates are ignored.
        /// </summary>
        /// <param name="attributes">Attributes to add</param>
        public void AddAttributes(params AttributeScriptableObject[] attributes)
        {
            // If this attribute already exists, we don't need to add it.  For that, we need to make sure the cache is up to date.
            var attributeCache = GetAttributeCache();

            foreach (var attribute in attributes)
            {
                if (attributeCache.ContainsKey(attribute))
                {
                    continue;
                }

                attributeScriptableObjects.Add(attribute);
                attributeCache.Add(attribute, this.attributeScriptableObjects.Count - 1);
            }
        }

        /// <summary>
        /// Remove attributes from this attribute system.
        /// </summary>
        /// <param name="attributes">Attributes to remove</param>
        public void RemoveAttributes(params AttributeScriptableObject[] attributes)
        {
            foreach (var attribute in attributes)
            {
                attributeScriptableObjects.Remove(attribute);
            }

            // Update attribute cache
            GetAttributeCache();
        }

        public void ResetAll()
        {
            for (var i = 0; i < attributeValues.Count; i++)
            {
                var defaultAttribute = new AttributeValue
                {
                    attributeScriptableObject = attributeValues[i].attributeScriptableObject
                };
                attributeValues[i] = defaultAttribute;
            }
        }

        public void ResetAttributeModifiers()
        {
            for (var i = 0; i < attributeValues.Count; i++)
            {
                var attributeValue = attributeValues[i];
                attributeValue.modifier = default;
                attributeValues[i] = attributeValue;
            }
        }

        private void InitialiseAttributeValues()
        {
            attributeValues = new List<AttributeValue>();
            foreach (var attribute in attributeScriptableObjects)
            {
                attributeValues.Add(new AttributeValue()
                    {
                        attributeScriptableObject = attribute,
                        modifier = new AttributeModifier()
                        {
                            addValue = 0f,
                            multiplyValue = 0f,
                            overrideValue = 0f
                        }
                    }
                );
            }
        }

        private readonly List<AttributeValue> _prevAttributeValues = new();

        public void UpdateAttributeCurrentValues()
        {
            _prevAttributeValues.Clear();
            for (var i = 0; i < attributeValues.Count; i++)
            {
                var attributeValue = attributeValues[i];
                _prevAttributeValues.Add(attributeValue);
                attributeValues[i] =
                    attributeValue.attributeScriptableObject.CalculateCurrentAttributeValue(attributeValue, attributeValues);
            }

            foreach (var attributeSystemEvent in attributeSystemEvents)
            {
                attributeSystemEvent.PreAttributeChange(this, _prevAttributeValues, ref attributeValues);
            }
        }

        private Dictionary<AttributeScriptableObject, int> GetAttributeCache()
        {
            if (_isAttributeDictStale)
            {
                MAttributeIndexCache.Clear();
                for (var i = 0; i < attributeValues.Count; i++)
                {
                    MAttributeIndexCache.Add(attributeValues[i].attributeScriptableObject, i);
                }

                _isAttributeDictStale = false;
            }

            return MAttributeIndexCache;
        }

        private void Awake()
        {
            InitialiseAttributeValues();
            MarkAttributesDirty();
            GetAttributeCache();
        }

        private void LateUpdate()
        {
            UpdateAttributeCurrentValues();
        }
    }
}