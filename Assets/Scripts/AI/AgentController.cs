using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ReverseSlender.AI {
    public class AgentController : MonoBehaviour {
        [Header("Settings")]
        [SerializeField]
        AgentSettings settings;
        [SerializeField]
        Animator animator;
        [SerializeField]
        NavMeshAgent agent;

        [Header("Auto-detected fields")]
        [SerializeField]
        internal Transform goal;
        [SerializeField]
        internal GoalType goalType;
        [SerializeField]
        internal VisionCone vision;
        internal ISet<Transform> collectibles = new HashSet<Transform>();

        [Header("In-game parameters")]
        [SerializeField, Range(-1, 1)]
        private float hurry = 0;
        [SerializeField, Range(-1, 1)]
        private float fear = 0;
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
            vision = GetComponentInChildren<VisionCone>();
            vision.settings = settings;
            vision.onNoticeCollectible += (collectible) => {
                if (!collectibles.Contains(collectible)) {
                    collectibles.Add(collectible);
                }
            };
        }

        void Update() {
            agent.speed = settings.speedOverHurry.Evaluate(hurry) * settings.speedOverFear.Evaluate(fear) * settings.baseSpeed;

            animator.SetFloat("hurry", hurry);
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
                if (collectibles.Contains(goal)) {
                    collectibles.Remove(goal);
                }
                Destroy(goal.gameObject);
                goal = null;
            }
        }
    }

}