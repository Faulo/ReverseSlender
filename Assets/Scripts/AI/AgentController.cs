using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ReverseSlender.AI {
    public class AgentController : MonoBehaviour {
        [SerializeField]
        Animator animator;
        [SerializeField]
        NavMeshAgent agent;

        void Start() {
        }

        void Update() {

            animator.SetFloat("Speed", agent.velocity.magnitude);
        }
    }

}