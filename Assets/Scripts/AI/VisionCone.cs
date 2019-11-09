using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseSlender.AI {
    public class VisionCone : MonoBehaviour {
        public AgentSettings settings { get; set; }

        const int NUMBER_OF_RESULTS = 1;
        const float SECONDS_TO_DRAW = 0.1f;
        private RaycastHit[] results = new RaycastHit[NUMBER_OF_RESULTS];
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            for (int i = 0; i < settings.numberOfVisionRays; i++) {
                var random = Random.insideUnitCircle;
                var direction = transform.forward + new Vector3(random.x * settings.horizontalFieldOfView, random.y * settings.verticalFieldOfView);
                if (Physics.RaycastNonAlloc(transform.position, direction, results, settings.visionRayDistance) > 0) {
                    var hit = results[0];
                    if (settings.drawVisionCone) {
                        Debug.DrawLine(transform.position, hit.point, Color.green, SECONDS_TO_DRAW);
                    }
                } else {
                    if (settings.drawVisionCone) {
                        Debug.DrawRay(transform.position, direction * settings.visionRayDistance, Color.red, SECONDS_TO_DRAW);
                    }
                }
            }
        }
    }
}