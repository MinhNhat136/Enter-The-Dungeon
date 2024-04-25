using UnityEngine;

namespace Atomic.Equipment
{
    //  Namespace Properties ------------------------------

    //  Class Attributes ----------------------------------

    /// <summary>
    /// TODO: Replace with comments...
    /// </summary>
    public class StraightIndicator : MonoBehaviour, ITrajectoryIndicator
    {
        
        //  Events ----------------------------------------


        //  Properties ------------------------------------
        public Vector3 Position { get; set; }
        public Vector3 LaunchPosition { get; set; }
        public Vector3 TargetPosition { get; set; }
        public Vector3 ForwardDirection { get; set; }
        public float MaxDistance { get; set; }
        public float SpreadAngle { get; set; }
        public float MaxRadius { get; set; }
        
        //  Fields ----------------------------------------
        [Header("MAIN")] 
        [SerializeField] private float targetIndicatorLength;
        [SerializeField] private SpriteRenderer directionIndicator;

        [Header("EDGE-LINE")]
        [SerializeField] private Transform left;
        [SerializeField] private Transform right;
        [SerializeField] private Vector3 targetEdgePosition; 
        [SerializeField] private Vector3 targetEdgeScale; 

        [Header("PARAMETER")]
        [SerializeField] private float speedIndicate;

        
        //  Initialization  -------------------------------

        
        //  Unity Methods   -------------------------------


        //  Other Methods ---------------------------------
        public void Indicate()
        {
            SpreadEdgeLine(left, false);
            SpreadEdgeLine(right, true);
            
            ScaleEdgeLineLength(left);
            ScaleEdgeLineLength(right);

            ScaleSprite();
        }

        private void SpreadEdgeLine(Transform transform, bool reverse = false)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, reverse ? targetEdgePosition : - targetEdgePosition, speedIndicate * Time.deltaTime);
        }

        private void ScaleEdgeLineLength(Transform transform)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, targetEdgeScale, speedIndicate * Time.deltaTime);
        }

        private void ScaleSprite()
        {
            Vector2 targetSize = new Vector2(targetIndicatorLength, directionIndicator.size.y);
            directionIndicator.size = Vector2.Lerp(directionIndicator.size, targetSize, speedIndicate * Time.deltaTime);
        }
        
        public void TurnOn()
        {
            transform.gameObject.SetActive(true);
        }

        public void TurnOff()
        {
            transform.gameObject.SetActive(false);
            left.transform.localPosition = Vector3.zero;
            right.transform.localPosition = Vector3.zero;

            left.transform.localScale = new Vector3(1, 1, 3.5f);
            right.transform.localScale = new Vector3(1, 1, 3.5f);
            
            directionIndicator.size = new Vector2(16, directionIndicator.size.y);
        }
        
        //  Event Handlers --------------------------------
        
    }
    
}
