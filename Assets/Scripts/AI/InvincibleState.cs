using UnityEngine;

namespace ReverseSlender.AI
{
    public class InvincibleState : StateMachineBehaviour
    {
        private AgentController agentController;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            agentController = animator.GetComponent<AgentController>();
            agentController.isInvincible = true;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            agentController.isInvincible = false;
        }
    }
}