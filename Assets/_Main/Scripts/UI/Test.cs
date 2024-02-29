using Doozy.Runtime.Nody;
using UnityEngine;

public class Test : MonoBehaviour
{
    public FlowController controller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Test stop");
            Time.timeScale = 0f;
        }  
      if(Input.GetKeyUp(KeyCode.S))
        {
            Time.timeScale = 1f;
            controller.ResumeFlow();

        }
    }
}
