﻿using Slothsoft.UnityExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        internal ISet<Collectible> collectiblesMemory = new HashSet<Collectible>();
        internal ISet<Hideout> hideoutMemory = new HashSet<Hideout>();

        [Header("In-game parameters")]
        [SerializeField, Range(-1, 1)]
        private float hurry = 0;
        private void AddHurry(float add) {
            hurry = Mathf.Clamp(hurry + add, -1, 1);
        }
        [SerializeField, Range(-1, 1)]
        private float fear = 0;
        private void AddFear(float add) {
            fear = Mathf.Clamp(fear + add, -1, 1);
        }
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
        private bool touchesHideout => goalType == GoalType.Hideout && goal != null && Vector3.Distance(transform.position, goal.position) < agent.stoppingDistance;

        private bool isDying;



        void Start() {
            vision = GetComponentInChildren<VisionCone>();
            vision.settings = settings;
            vision.onNoticeCollectible += RememberCollectible;
            vision.onNoticeHideout += RememberHideout;
            vision.onNoticePlayer += (PlayerController player, float attention) => {
                if (player.InScareMode) {
                    AddFear(attention * settings.monsterMultiplier);
                    if (hideoutMemory.Count == 0 && fear == 1) {
                        Die();
                    } else {
                        RecallHideout();
                    }
                } else {
                    AddHurry(attention * settings.ghostMultiplier);
                }
            };
        }

        void Update() {
            agent.speed = settings.speedOverHurry.Evaluate(hurry) * settings.speedOverFear.Evaluate(fear) * settings.baseSpeed;

            animator.SetFloat("hurry", hurry);
            animator.SetFloat("fear", fear);
            animator.SetBool("hasGoal", hasGoal);
            animator.SetBool("touchesCollectible", touchesCollectible);
            animator.SetBool("touchesHideout", touchesHideout);
        }

        public void LookAtGoal() {
            if (goal) {
                transform.LookAt(goal);
            }
        }

        public void DestroyGoal() {
            if (goal) {
                switch (goalType) {
                    case GoalType.Collectible:
                        AddFear(-settings.collectibleRest);
                        ForgetCollectible(goal.GetComponent<Collectible>());
                        break;
                    case GoalType.Hideout:
                        AddFear(-settings.hideoutRest);
                        ForgetHideout(goal.GetComponent<Hideout>());
                        break;
                }
                Destroy(goal.gameObject);
                goal = null;
            }
        }

        public void RememberCollectible(Collectible collectible) {
            if (!collectiblesMemory.Contains(collectible)) {
                collectiblesMemory.Add(collectible);
            }
            RecallCollectible();
        }
        public void RecallCollectible() {
            if (collectiblesMemory.Count > 0 && !animator.GetBool("hasGoal")) {
                goal = collectiblesMemory
                    .Select(collectible => collectible.transform)
                    .RandomWeightedElementDescending(t => Mathf.CeilToInt(Vector3.Distance(t.position, transform.position)));
                goalType = GoalType.Collectible;
            }
        }
        public void ForgetCollectible(Collectible collectible) {
            if (collectiblesMemory.Contains(collectible)) {
                collectiblesMemory.Remove(collectible);
            }
        }

        public void RememberHideout(Hideout hideout) {
            if (!hideoutMemory.Contains(hideout)) {
                hideoutMemory.Add(hideout);
            }
        }
        public void RecallHideout() {
            if (hideoutMemory.Count > 0 && goalType != GoalType.Hideout) {
                goal = hideoutMemory
                    .Select(hideout => hideout.transform)
                    .RandomWeightedElementDescending(t => Mathf.CeilToInt(Vector3.Distance(t.position, transform.position)));
                goalType = GoalType.Hideout;
            }
        }
        public void ForgetHideout(Hideout hideout) {
            if (hideoutMemory.Contains(hideout)) {
                hideoutMemory.Remove(hideout);
            }
        }

        private void Die() {
            if (!isDying) {
                isDying = true;
                animator.SetTrigger("isDying");
            }
        }
    }
}