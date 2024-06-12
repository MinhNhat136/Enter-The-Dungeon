using System.Collections.Generic;
using UnityEngine;

namespace Atomic.AbilitySystem
{
    /// <summary>
    /// Manages the attributes for a game character
    /// </summary>
    public class AttributeSystemComponent : MonoBehaviour
    {
        //  Events ----------------------------------------
        public delegate void NotifyAttributeChanged(AttributeScriptableObject attributeScriptableObject);
        public NotifyAttributeChanged onAttributeChanged;

        //  Properties ------------------------------------
       

        //  Fields ----------------------------------------
        [SerializeField] private AbstractAttributeEventHandler[] attributeSystemEvents;
        [SerializeField] private List<AttributeScriptableObject> attributeScriptableObjects;
        [SerializeField] private List<AttributeValue> attributeValues;
        
        private bool _isAttributeDictStale;
        public Dictionary<AttributeScriptableObject, int> MAttributeIndexCache { get; private set; } = new ();
        private readonly List<AttributeValue> _prevAttributeValues = new();
        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------
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

        private void OnDestroy()
        {
            foreach (var attributeSystemEvent in attributeSystemEvents)
            {
                attributeSystemEvent.Reset();
            }
        }
        
        //  Other Methods ---------------------------------
        public void MarkAttributesDirty()
        {
            _isAttributeDictStale = true;
        }
        
        public bool GetAttributeValue(AttributeScriptableObject attribute, out AttributeValue value)
        {
            var attributeCache = GetAttributeCache();

            if (attributeCache.TryGetValue(attribute, out var index))
            {
                value = attributeValues[index];
                return true;
            }
            
            value = new AttributeValue();
            return false;
        }
        
        public void SetAttributeBaseValue(AttributeScriptableObject attribute, float value)
        {
            var attributeCache = GetAttributeCache();
            if (!attributeCache.TryGetValue(attribute, out var index)) return;
            var attributeValue = attributeValues[index];
            attributeValue.baseValue = value;
            attributeValues[index] = attributeValue;
        }
        
        public bool UpdateAttributeModifiers(AttributeScriptableObject attribute, AttributeModifier modifier, out AttributeValue value)
        {
            var attributeCache = GetAttributeCache();

            if (attributeCache.TryGetValue(attribute, out var index))
            {
                value = attributeValues[index];
                value.modifier = value.modifier.Combine(modifier);

                attributeValues[index] = value;
                return true;
            }

            // No matching attribute found
            value = new AttributeValue();
            return false;
        }

        public void AddAttributes(params AttributeScriptableObject[] attributes)
        {
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
        
        //  Event Handlers --------------------------------

    }
    
}