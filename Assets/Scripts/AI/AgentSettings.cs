using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseSlender.AI {
    [CreateAssetMenu(fileName = "New Agent Settings", menuName = "Gameplay/Agent Settings", order = 1)]
    public class AgentSettings : ScriptableObject {
        public AnimationCurve speedOverFear;
        public float maximumSpeed;
    }
}