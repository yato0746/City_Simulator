using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntiyStateMachineBehaviour : StateMachineBehaviour
{
    private EntityModel model;
    [SerializeField] EntityState entityState;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!model)
        {
            model = animator.GetComponent<EntityModel>();
        }

        // OnStateExit
        model.Entity.HandleStateBehaviour((int)model.Entity.State * 10 + 2);

        // OnStateEnter
        model.Entity.HandleStateBehaviour((int)entityState * 10);

        model.Entity.State = entityState;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (model.Entity.State != entityState)
        {
            // You are already not in this state
            return;
        }

        // OnStateUpdate
        model.Entity.HandleStateBehaviour((int)entityState * 10 + 1);
    }
}
