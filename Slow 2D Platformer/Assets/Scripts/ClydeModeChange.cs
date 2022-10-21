using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeModeChange : StateMachineBehaviour
{
    private PlayerController2D pcd;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            if (pcd == null)
            {
                pcd = animator.GetComponent<PlayerController2D>();
                if (pcd == null)
                {
                    Debug.LogError($"No player controller found on {name}");
                }
            }

            animator.SetBool("IsHuman", pcd.isHuman);
        
    }
}
