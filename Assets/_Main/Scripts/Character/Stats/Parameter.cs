using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atomic.Character.Stats
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// Represents a value that can be modified by various modifiers.
    /// </summary>
    [Serializable]
	public class Parameter
	{
        //  Events ----------------------------------------
        private event Action<float, float> onChangedEvent;

        //  Properties ------------------------------------
        public event Action<float, float> OnChangedEvent
        {
            add 
            { 
                onChangedEvent += value; 
            } 
            remove
            {
                onChangedEvent -= value;
            }
        }

        public virtual float BaseValue 
        {
            get
            {
                return _baseValue;
            }
            set
            {
                _baseValue = value;
                onChangedEvent?.Invoke(_lastBaseValue, _baseValue);
            }
        }

        public virtual float Value
        {
            get
            {
                if (_isDirty || _lastBaseValue != _baseValue)
                {
                    _lastBaseValue = _baseValue;
                    _value = CalculateFinalValue();
                    _isDirty = false;
                }
                return _value;
            }
        }

        //  Collections -----------------------------------
        protected readonly List<ParameterModifier> _parameterModifiers;
        public readonly ReadOnlyCollection<ParameterModifier> ParameterModifiers;

        //  Fields ----------------------------------------
        private float _baseValue;

        protected bool _isDirty = true;
        protected float _lastBaseValue;

        protected float _value;

        //  Initialization  -------------------------------
        public Parameter()
        {
            _parameterModifiers = new List<ParameterModifier>();
            ParameterModifiers = _parameterModifiers.AsReadOnly();
        }

        public Parameter(float baseValue) : this()
        {
            _baseValue = baseValue;
        }

        //  Other Methods ---------------------------------
        public virtual void AddModifier(ParameterModifier mod)
        {
            _isDirty = true;
            _parameterModifiers.Add(mod);
        }

        public virtual bool RemoveModifier(ParameterModifier mod)
        {
            if (_parameterModifiers.Remove(mod))
            {
                _isDirty = true;
                return true;
            }
            return false;
        }

        public virtual void UpdateModifier(int index, ParameterModifier newModifier)
        {
            if (index < 0 || index >= _parameterModifiers.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index of StatModifier is out of range.");

            _parameterModifiers[index] = newModifier;
            _isDirty = true;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            int numRemovals = _parameterModifiers.RemoveAll(mod => mod.Source == source);

            if (numRemovals > 0)
            {
                _isDirty = true;
                return true;
            }
            return false;
        }

        protected virtual int CompareModifierOrder(ParameterModifier a, ParameterModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = _baseValue;
            float sumPercentAdd = 0;

            _parameterModifiers.Sort(CompareModifierOrder);

            for (int i = 0; i < _parameterModifiers.Count; i++)
            {
                ParameterModifier mod = _parameterModifiers[i];

                if (mod.Type == ParameterModifierType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == ParameterModifierType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;

                    if (i + 1 >= _parameterModifiers.Count || _parameterModifiers[i + 1].Type != ParameterModifierType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == ParameterModifierType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }

    }
}
