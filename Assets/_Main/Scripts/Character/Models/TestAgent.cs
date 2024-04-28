using System;

namespace Atomic.Character
{

    public class TestAgent : BaseAgent
    {
        public override void Assign()
        {
            throw new System.NotImplementedException();
        }

        public void Awake()
        {
            base.Initialize();
        }
    }
}
