using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class PolicyViewController 
{
    
    private PolicyService policyService;

    public PolicyViewController(PolicyService policyService)
    {
        this.policyService = policyService;
    }

    public void AcceptPolicy() => policyService.AcceptPolicy();
    
}
