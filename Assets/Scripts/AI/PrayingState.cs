using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseSlender.AI {
    public class PrayingState : StateMachineBehaviour {
        const string PRAYING_SOUNDNAME = "Praying";
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            AudioManager.Instance.PlaySound(PRAYING_SOUNDNAME);
        }
    }
}