using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseSlender.AI {
    [CreateAssetMenu(fileName = "New Agent Settings", menuName = "Gameplay/Agent Settings", order = 1)]
    public class AgentSettings : ScriptableObject {
        [Header("Movement")]
        public AnimationCurve speedOverHurry;
        public AnimationCurve speedOverFear;
        [Range(0, 20)]
        public float baseSpeed;

        [Header("Vision")]
        public bool drawVisionCone;
        [Range(0, 1000)]
        public int numberOfVisionRays;
        [Range(0, 1000)]
        public int visionRayDistance;
        [Range(0, 10)]
        public float horizontalFieldOfView;
        [Range(0, 10)]
        public float verticalFieldOfView;

        [Header("Player Interaction")]
        [Range(0, 1000), Tooltip("How much Ghost-Avatar scares Bryce")]
        public float ghostMultiplier;
        [Range(0, 1000), Tooltip("How much Monster-Avatar scares Bryce")]
        public float monsterMultiplier;
        [Range(0, 2), Tooltip("How much collecting gems restores sanity.")]
        public float collectibleRest;
        [Range(0, 2), Tooltip("How much successfully fleeing randomly restores sanity.")]
        public float fleeRest;
        [Range(0, 2), Tooltip("How much praying restores sanity.")]
        public float hideoutRest;
        [Tooltip("Prefab to spawn when randomly fleeing.")]
        public Transform fleePointPrefab;
        [Range(0, 100), Tooltip("Distance to flee when spotting Monster-Avatar.")]
        public float fleePointDistance;
    }
}