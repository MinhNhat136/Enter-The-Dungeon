using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.UI;

public class TestPerformance : MonoBehaviour
{
    private UnityEvent _eventDoSth;

    public int testNumber;

    public void Awake()
    {
        _eventDoSth.AddListener(DoSomething);
    }


    private void DoSomething()
    {
        
    }

    private void TestPerformance1()
    {
        for(int i = 0; i < testNumber; i++) 
        {
            DoSomething();
        }
    }

    private void TestPerformance2()
    {
        for (int i = 0; i < testNumber; i++)
        {
            _eventDoSth.Invoke();
        }
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TestPerformance1 ();
            TestPerformance2 ();
        }
    }
    // Start is called before the first frame update
}
