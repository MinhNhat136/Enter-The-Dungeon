using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;

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
        
        private TextMeshProUGUI _textRenderer;
        private float _time;
        private Vector3 _originPosition;
        private Vector3 _originScale;
        private Color _originColor;

        public float horizontalDirection = 1;
        public float verticalDirection = 1;
        public float value;
        public string description; 
        
        private Transform _transform;

        private Camera _camera;
        [HideInInspector] public ObjectPool<EffectPopupAnimation> myPool;

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
        
        public void StartAnimation()
        {
            _time = 0; 
            _originPosition = _transform.position;
            _originScale = _transform.localScale;
            _originColor = _textRenderer.color; 
            
            _textRenderer.text = $"{value} {description}";
            horizontalDirection = Mathf.Clamp(horizontalDirection, -1, 1);
            verticalDirection = Mathf.Clamp(verticalDirection, -1, 1);
            _transform.forward = _camera.transform.forward;
            gameObject.SetActive(true);
            Invoke(nameof(EndAnimation), lifeTime);
        }
        
        private void UpdateAnimation()
        {
            _textRenderer.color = _originColor + new Color(0, 0, 0, opacityCurve.Evaluate(_time));
            _transform.localScale = Vector3.one * scaleCurve.Evaluate(_time);
            _transform.position = _originPosition + new Vector3((1 +   horizontalCurve.Evaluate(_time)) * horizontalDirection, verticalDirection * verticalCurve.Evaluate(_time), 0);
            _time += speedChange * Time.deltaTime;
        }
        
        public void EndAnimation()
        {
            gameObject.SetActive(false);
            _time = 0; 
            _transform.position = _originPosition;
            _transform.localScale = _originScale;
            _textRenderer.color = _originColor;
            myPool.Release(this);
        }

        //  Other Methods ---------------------------------


        //  Event Handlers --------------------------------
        
    }
    
}
