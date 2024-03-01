using Atomic.Command;
using Atomic.Core.Interface;
using RMC.Core.Architectures.Mini.Context;
using System.Collections;
using UnityEngine;

namespace Atomic.Chain
{
    public abstract class BaseChain : IChain
    {
        protected IChain _nextChain;
        protected IContext _context;

        public virtual IChain SetContext(IContext context)
        {
            _context = context;
            return this;
        }

        public virtual IChain SetNextHandler(IChain chain)
        {
            _nextChain = chain;
            return this;
        }

        public virtual void Handle()
        {

        }

        public virtual void OnDestroyChain()
        {

        }
    }
}