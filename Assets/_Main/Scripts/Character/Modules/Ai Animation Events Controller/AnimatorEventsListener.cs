using System.Runtime.CompilerServices;
using System;
using UnityEngine;

public class AnimatorEventsListener : MonoBehaviour
{
    
    public event Action<int> OnBeginHitEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnStopIndicatorEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnBeginTrailEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnStopTrailEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnBeginMoveEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnStopMoveEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnBeginTrackingEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnStopTrackingEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action OnBeginAttackEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action OnEndAttackEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action OnCanMoveNextSkillEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action OnEnterLocomotionStateEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action OnAnimatorMoveEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnPrepareShootEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnChargeFullEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnShootEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnCustomEventEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnFeedbackEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public event Action<int> OnFootEvent
    {
        [CompilerGenerated]
        add
        {
        }
        [CompilerGenerated]
        remove
        {
        }
    }

    public event Action OnJumpLandingEvent
    {
        add
        {
        }
        remove
        {
        }
    }

    public void OnBeginHit(int index)
    {
    }

    public void OnStopIndicator(int index)
    {
    }

    public void OnBeginMove(int index)
    {
    }

    public void OnStopMove(int index)
    {
    }

    public void OnBeginTracking(int index)
    {
    }

    public void OnStopTracking(int index)
    {
    }

    public void OnBeginTrail(int index)
    {
    }

    public void OnStopTrail(int index)
    {
    }

    public void OnBeginAttack()
    {
    }

    public void OnEndAttack()
    {
    }

    public void OnCanMoveNextSkill()
    {
    }

    public void OnPrepareShoot(int index)
    {
    }

    public void OnChargeFull(int index)
    {
    }

    public void OnShoot(int index)
    {
    }

    public void OnCustomEvent(int index)
    {
    }

    public void OnFeedback(int index)
    {
    }

    public void OnFoot(int index)
    {
    }

    public void OnJumpLanding()
    {
    }

    public void OnEnterLocomotionState()
    {
    }

    protected void TriggerAnimatorMoveEvent()
    {
    }
}
