using Doozy.Runtime.Nody;
using UnityEngine;

public class Test : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0f; 
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Time.timeScale = 0f;
        }
    }
}
