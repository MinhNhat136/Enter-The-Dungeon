using RMC.Core.Architectures.Mini.Context;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;


namespace Atomic.Core.Interface
{
    public interface IChain 
    {
        IChain SetNextHandler(IChain chain);
        IChain SetContext(IContext context);
        void Handle();
    }

}
