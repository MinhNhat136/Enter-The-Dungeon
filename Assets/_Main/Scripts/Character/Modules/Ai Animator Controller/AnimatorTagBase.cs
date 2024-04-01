using System.Collections.Generic;
using UnityEngine;

namespace Atomic.Character.Module
{
    public abstract class AnimatorTagBase : StateMachineBehaviour
    {
        public delegate void OnStateTrigger(List<string> tags);
        public List<AnimatorStateInfos> stateInfos = new List<AnimatorStateInfos>();
        public event OnStateTrigger onStateEnter;
        public event OnStateTrigger onStateExit;
        public virtual void AddStateInfoListener(AnimatorStateInfos stateInfo)
        {
            if (!stateInfos.Contains(stateInfo))
            {
                stateInfos.Add(stateInfo);
            }
        }
        public virtual void RemoveStateInfoListener(AnimatorStateInfos stateInfo)
        {
            if (stateInfos.Contains(stateInfo))
            {
                stateInfos.Remove(stateInfo);
            }
        }
        protected virtual void OnStateEnterEvent(List<string> tags)
        {
            if (onStateEnter != null)
                onStateEnter(tags);
        }
        protected virtual void OnStateExitEvent(List<string> tags)
        {
            if (onStateEnter != null)
                onStateExit(tags);
        }


    }
}