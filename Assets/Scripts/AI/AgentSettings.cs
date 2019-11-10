using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseSlender.AI {
    [CreateAssetMenu(fileName = "New Agent Settings", menuName = "Gameplay/Agent Settings", order = 1)]
    public class AgentSettings : ScriptableObject {
        [Header("Movement")]
        [Range(0, 20)]
        public float baseSpeed;
        public AnimationCurve speedOverHurry;
        public AnimationCurve speedOverFear;

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
        [Range(0, 1000), Tooltip("How much Bryce scares ghost form.")]
        public float stunMultiplier;
        [Range(0, 1000), Tooltip("How much ghost form scares Bryce.")]
        public float ghostMultiplier;
        [Range(0, 1000), Tooltip("How much monster form scares Bryce.")]
        public float monsterMultiplier;
        [Range(0, 2), Tooltip("How much collecting gems restores sanity.")]
        public float collectibleRest;
        [Range(0, 2), Tooltip("How much successfully fleeing randomly restores sanity.")]
        public float fleeRest;
        [Range(0, 2), Tooltip("How much praying restores sanity.")]
        public float hideoutRest;

        [Header("Heartbeat")]
        [Tooltip("Heartbeats per minute in the range of -1 to 1.")]
        public AnimationCurve bpmOverFear;

        [Header("Flee Point Generation")]
        [Tooltip("Whether or not to flee at all when spotting the player in monster form.")]
        public bool useFleePoints;
        [Tooltip("Prefab to spawn when fleeing.")]
        public Transform fleePointPrefab;
        [Range(0, 100), Tooltip("Minimum distance to flee when spotting monster form.")]
        public float fleePointMinDistance;
        [Range(0, 100), Tooltip("Maximum distance to flee when spotting monster form.")]
        public float fleePointMaxDistance;
        [Range(0, 10), Tooltip("How much fleeing should resemble a straight line away from the player.")]
        public float fleePointTurnabout;
    }
}