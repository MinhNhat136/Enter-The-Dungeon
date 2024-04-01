using Atomic.Core;
using UnityEngine;

namespace Atomic.Character.Module
{
    public class AnimatorTag : AnimatorTagBase
    {
        public string[] tags = new string[] { "CustomAction" };

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex); 
            if (stateInfos != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    for (int a = 0; a < stateInfos.Count; a++)
                    {
                        stateInfos[a].AddStateInfo(tags[i], layerIndex);
                    }
                }
            }
            OnStateEnterEvent(tags.vToList());
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);
            if (stateInfos != null)
            {
                for (int a = 0; a < stateInfos.Count; a++)
                {
                    stateInfos[a].UpdateStateInfo(layerIndex, stateInfo.normalizedTime, stateInfo.shortNameHash);
                }
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfos != null)
            {
                for (int i = 0; i < tags.Length; i++)
                {
                    for (int a = 0; a < stateInfos.Count; a++)
                        stateInfos[a].RemoveStateInfo(tags[i], layerIndex);
                }
            }
            base.OnStateExit(animator, stateInfo, layerIndex);
            OnStateExitEvent(tags.vToList());           
        }
    }
}