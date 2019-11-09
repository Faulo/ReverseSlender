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
    }
}