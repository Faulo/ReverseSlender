using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ReverseSlender.AI {
    public class MoveToState : StateMachineBehaviour {
        private AgentController agentController;
        private NavMeshAgent agent;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            agentController = animator.GetComponent<AgentController>();
            agent = animator.GetComponent<NavMeshAgent>();

            agent.destination = agentController.goal.position;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            if (agentController.goal) {
                agent.destination = agentController.goal.position;
            } else {
                agentController.StopMoving();
            }
        }
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}
    }
}