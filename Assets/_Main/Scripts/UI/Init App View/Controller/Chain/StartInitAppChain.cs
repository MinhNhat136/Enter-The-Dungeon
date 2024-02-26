using Atomic.Core;
using Atomic.Core.Interface;
using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using UnityEngine;

namespace Atomic.Chain
{
    public class StartInitAppChain : BaseChain
    {
        public override void Handle()
        {
            Coroutines.StartCoroutine(Coroutine_DelayLoading());
        }

        public IEnumerator Coroutine_DelayLoading()
        {
            Debug.Log("delay time");
            yield return new WaitForSeconds(1);
            _nextChain?.Handle();
        }

        public override IChain SetContext(IContext context)
        {
            return this;
        }
    }

}
