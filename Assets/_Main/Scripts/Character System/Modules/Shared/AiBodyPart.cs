using System;
using UnityEngine;

namespace Atomic.Character
{
public class AiBodyPart : MonoBehaviour
{
    [field: SerializeField]
    public BodyPartType BodyPartType { get; private set; }

    private Collider _collider;
    

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    public void SetLayer(int hitBoxLayer)
    {
       
            gameObject.layer = hitBoxLayer;
    }

    public void TurnOffHitBox()
    {
        if (_collider != null)
        {
            _collider.enabled = false;
        }
    }

    public void TurnOnHitBox()
    {
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }


}    
}
