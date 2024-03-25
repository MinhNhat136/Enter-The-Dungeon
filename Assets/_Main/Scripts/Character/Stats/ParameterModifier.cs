namespace Atomic.Character.Stats
{

    //  Namespace Properties ------------------------------
    public enum ParameterModifierType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }
    //  Class Attributes ----------------------------------

    /// <summary>
    /// Represents a modifier that can be applied to a character stat, affecting its value.
    /// </summary>
    public class ParameterModifier
	{
		private readonly float _value;
		private readonly ParameterModifierType _type;
		private readonly int _order;
		private readonly object _source;

		public ParameterModifier(float value, ParameterModifierType type, int order, object source)
		{
			_value = value;
			_type = type;
			_order = order;
			_source = source;
		}

		public ParameterModifier(float value, ParameterModifierType type) : this(value, type, (int)type, null) { }

		public ParameterModifier(float value, ParameterModifierType type, int order) : this(value, type, order, null) { }

		public ParameterModifier(float value, ParameterModifierType type, object source) : this(value, type, (int)type, source) { }

		public float Value
		{
			get { return _value; }
		}

		public ParameterModifierType Type
		{
			get { return _type; }
		}

		public int Order
		{
			get { return _order; }
		}

		public object Source
		{
			get { return _source; }
		}
	}
}
