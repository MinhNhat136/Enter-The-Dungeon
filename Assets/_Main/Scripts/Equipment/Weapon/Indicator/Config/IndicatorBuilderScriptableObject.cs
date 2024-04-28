using System;
using Atomic.Character;
using UnityEngine;
using UnityEngine.UIElements;

namespace Atomic.Equipment
{
    [CreateAssetMenu(fileName = "Indicator", menuName = "Weapons/Indicator/Default", order = 0)]
    public class IndicatorBuilderScriptableObject : ScriptableObject, ICloneable
    {
        public GameObject indicatorPrefab;
        public Vector3 indicatorPosition;
        public float delayActivateTime;
        public float minDistance;
        public float maxDistance;
        
       
        private GameObject _indicator;
        public  ITrajectoryIndicator trajectoryIndicator;
        
        public virtual void Initialize(BaseAgent owner)
        {
            
            _indicator = Instantiate(indicatorPrefab, owner.transform, true);

            trajectoryIndicator = _indicator.GetComponent<ITrajectoryIndicator>();
            trajectoryIndicator.DelayActivateTime = delayActivateTime;
            trajectoryIndicator.DeActivate();
        }

        public void Destroy()
        {
            _indicator = null;
            Debug.Log("hello");
            trajectoryIndicator = null; 
        }
        
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
