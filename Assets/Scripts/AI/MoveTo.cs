using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace ReverseSlender.AI {
    public class MoveTo : MonoBehaviour {
        private Collectable goal;
        private IEnumerable<Collectable> goals => FindObjectsOfType<Collectable>();

        private NavMeshAgent agent;

        void Start() {
            agent = GetComponent<NavMeshAgent>();
        }

        void Update() {
            if (!goal) {
                goal = goals.RandomElement();
            }
            if (goal) {
                agent.destination = goal.transform.position;
            } else {
                agent.isStopped = true;
            }
        }
    }

}