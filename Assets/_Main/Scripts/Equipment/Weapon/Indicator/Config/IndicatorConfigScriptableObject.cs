using System;
using System.Collections;
using Atomic.Character;
using UnityEngine;

namespace Atomic.Equipment
{
    [CreateAssetMenu(fileName = "Indicator", menuName = "Weapons/Indicator/Default", order = 0)]
    public class IndicatorConfigScriptableObject : ScriptableObject, ICloneable
    {
        public float delayTurnOnTime;
        
        public GameObject indicatorPrefab;
        public Vector3 indicatorPosition;
        
        private GameObject _indicator;
        public  ITrajectoryIndicator trajectoryIndicator;

        
        public virtual void Initialize(BaseAgent owner)
        {
            _indicator = Instantiate(indicatorPrefab, owner.transform, true);

            trajectoryIndicator = _indicator.GetComponent<ITrajectoryIndicator>();
            trajectoryIndicator.DeActivate();
            
        }

        public void Destroy()
        {
            _indicator = null;
            trajectoryIndicator = null; 
        }

        public void TurnOn()
        {
            trajectoryIndicator.Activate(delayTurnOnTime);
        }

        public void Indicate()
        {
            trajectoryIndicator.Indicate();
        }

        public void TurnOff()
        {
            trajectoryIndicator.DeActivate();
        }
        
        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
