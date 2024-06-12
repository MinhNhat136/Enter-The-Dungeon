using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace  Atomic.Damage
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class EffectPopupAnimation : MonoBehaviour
    {
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public EffectPopupAnimation SetTimeDelay(float value)
        {
            _timeDelay = value; 
            return this;
        }
        
        public EffectPopupAnimation SetPosition(Vector3 position)
        {
            transform.position = position;
            return this;
        }
        
        public EffectPopupAnimation SetColor(Color color)
        {
            _originColor = color;
            return this; 
        }

        public EffectPopupAnimation SetHorizontalDirection(float value)
        {
            horizontalDirection = Mathf.Clamp(value, -1, 1);
            return this;
        }

        public EffectPopupAnimation SetVerticalDirection(float value)
        {
            verticalDirection = Mathf.Clamp(value, -1, 1);
            return this;
        }

        public EffectPopupAnimation SetShowValue(float value)
        {
            if (value != 0) 
                _value = value;
            return this;
        }

        public EffectPopupAnimation SetDescription(string decription)
        {
            _description = decription;
            return this;
        }

        //  Fields --------p--------------------------------
        [SerializeField] 
        private float speedChange;
        [SerializeField] 
        private float lifeTime; 
        [SerializeField] 
        private AnimationCurve opacityCurve;
        [SerializeField] 
        private AnimationCurve scaleCurve;
        [SerializeField] 
        private AnimationCurve verticalCurve;
        [SerializeField] 
        private AnimationCurve horizontalCurve;
        [SerializeField] 
        private float verticalOffset;
        [SerializeField] 
        private float horizontalOffset;
        
        private TextMeshProUGUI _textRenderer;
        private float _time;
        private Vector3 _originPosition;
        private Vector3 _originScale;
        private Color _originColor;
        private float _timeDelay; 

        public float horizontalDirection = 1;
        public float verticalDirection = 1;
        
        private float _value;
        private string _description; 
        private Transform _transform;
        private Camera _camera;
        public ObjectPool<EffectPopupAnimation> MyPools { get; set; }

        private float _verticalRandomValue;
        private float _horizontalRandomValue;

        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------
        private void Awake()
        {
            _camera = Camera.main;
            gameObject.SetActive(false);
            _transform = transform;
            _textRenderer = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }

        public void Update()
        {
            UpdateAnimation();
        }

        public void Play()
        {
            Invoke(nameof(StartAnimation), _timeDelay);
        }
        
        private void StartAnimation()
        {
            _time = 0; 
            _originPosition = _transform.position ;
            _originScale = _transform.localScale;

            string textValue = _value != 0 ? $"{_value}" : ""; 
            
            _textRenderer.text = $"{textValue} {_description}";
            _transform.forward = _camera.transform.forward;

            _verticalRandomValue = Random.Range(-horizontalOffset, horizontalOffset);
            _horizontalRandomValue = Random.Range(-verticalOffset, verticalOffset);
            gameObject.SetActive(true);
            Invoke(nameof(EndAnimation), lifeTime);
        }
        
        private void UpdateAnimation()
        {
            _textRenderer.color = _originColor + new Color(0, 0, 0, opacityCurve.Evaluate(_time));
            _transform.localScale = Vector3.one * scaleCurve.Evaluate(_time);
            _transform.position = _originPosition + new Vector3(
                horizontalCurve.Evaluate(_time) * horizontalDirection + _horizontalRandomValue, 
                0, 
                verticalDirection * verticalCurve.Evaluate(_time)+  _verticalRandomValue);
            _time += speedChange * Time.deltaTime;
        }
        
        private void EndAnimation()
        {
            _time = 0; 
            _transform.position = _originPosition;
            _transform.localScale = _originScale;
            _textRenderer.color = _originColor;
            MyPools.Release(this);
        }
        
        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        
    }
    
}
