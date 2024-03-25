using Atomic.Character.Model;
using System.Collections.Generic;
using UnityEngine;

public class AgentsManager : MonoBehaviour
{
    private List<BaseAgent> _agents = new();


    public void RegisterAgent(BaseAgent agent)
    {
        _agents.Add(agent);
    }

    public void UnRegisterAgent(BaseAgent agent)
    {
        _agents.Remove(agent);
    }

    public void UnRegisterAll()
    {
        _agents.Clear();
    }
}
