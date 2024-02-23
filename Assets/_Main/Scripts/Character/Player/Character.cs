using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public Player_Run stateRun;
    public Animator animator; 


    void Start()
    {
        animator = GetComponent<Animator>();
        stateRun = animator.GetBehaviour<Player_Run>();
    }

}
