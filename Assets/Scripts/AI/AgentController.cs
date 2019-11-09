using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ReverseSlender.AI {
    public class AgentController : MonoBehaviour {
        [SerializeField]
        AgentSettings settings;
        [SerializeField]
        Animator animator;
        [SerializeField]
        NavMeshAgent agent;

        [SerializeField]
        internal Transform goal;
        [SerializeField]
        internal GoalType goalType;

        [SerializeField, Range(-1, 1)]
        private float speed = -1;
        [SerializeField, Range(-1, 1)]
        private float fear = -1;
        private bool hasGoal {
            get {
                if (goal != null) {
                    var path = new NavMeshPath();
                    if (agent.CalculatePath(goal.position, path)) {
                        if (path.status == NavMeshPathStatus.PathComplete) {
                            return true;
                        }
                    }
                }
                return false;
            }
        }
        private bool touchesCollectible => goalType == GoalType.Collectible && goal != null && Vector3.Distance(transform.position, goal.position) < agent.stoppingDistance;

        void Start() {
        }

        void Update() {
            agent.speed = speed * settings.speedOverFear.Evaluate(fear) * settings.maximumSpeed;

            animator.SetFloat("speed", speed);
            animator.SetFloat("fear", fear);
            animator.SetBool("hasGoal", hasGoal);
            animator.SetBool("touchesCollectible", touchesCollectible);
        }

        public void LookAtGoal() {
            if (goal) {
                transform.LookAt(goal);
            }
        }

        public void DestroyGoal() {
            if (goal) {
                Debug.Log(string.Format("I, {0}, found an item!", gameObject));
                Destroy(goal.gameObject);
                goal = null;
            }
        }
    }

}