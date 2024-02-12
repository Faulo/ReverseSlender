using UnityEngine;

namespace ReverseSlender.AI
{
    public class MoveToState : StateMachineBehaviour
    {
        private AgentController agentController;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            agentController = animator.GetComponent<AgentController>();
            agentController.StartMovingTo(agentController.goal.position);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (agentController.goal)
            {
                agentController.StartMovingTo(agentController.goal.position);
            }
            else
            {
                agentController.StopMoving();
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            agentController.StopMoving();
        }
    }
}