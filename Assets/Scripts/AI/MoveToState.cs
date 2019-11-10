using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ReverseSlender.AI {
    public class MoveToState : StateMachineBehaviour {
        private AgentController agentController;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            agentController = animator.GetComponent<AgentController>();
            agentController.StartMovingTo(agentController.goal.position);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (agentController.goal) {
                agentController.StartMovingTo(agentController.goal.position);
            } else {
                agentController.StopMoving();
            }
        }

        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            agentController.StopMoving();
        }
    }
}