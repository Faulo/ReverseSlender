
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReverseSlender.AI {
    public class VisionCone : MonoBehaviour {
        public System.Action<Collectible> onNoticeCollectible;
        public System.Action<Hideout> onNoticeHideout;
        public System.Action<PlayerController, float> onNoticePlayer;

        public AgentSettings settings { get; set; }


        const int NUMBER_OF_RESULTS = 4;
        const float SECONDS_TO_DRAW = 0.1f;
        const int VISIBLE_LAYERS = ~0;
        private RaycastHit[] results;
        // Start is called before the first frame update
        void Start() {

        }

        // Update is called once per frame
        void Update() {
            for (int i = 0; i < settings.numberOfVisionRays; i++) {
                results = new RaycastHit[NUMBER_OF_RESULTS];
                var random = Random.insideUnitCircle;
                var direction = transform.forward + transform.right * random.x * settings.horizontalFieldOfView + transform.up * random.y * settings.verticalFieldOfView;
                if (Physics.RaycastNonAlloc(transform.position, direction, results, settings.visionRayDistance, VISIBLE_LAYERS) > 0) {
                    var hit = results
                        .Where(h => h.collider)
                        .OrderBy(h => h.distance)
                        .First();
                    if (hit.rigidbody && hit.rigidbody.TryGetComponent(out Collectible collectible)) {
                        onNoticeCollectible?.Invoke(collectible);
                        DrawVision(hit.point, Color.green);
                        return;
                    }
                    if (hit.collider.TryGetComponent(out Hideout hideout)) {
                        onNoticeHideout?.Invoke(hideout);
                        DrawVision(hit.point, Color.yellow);
                        return;
                    }
                    if (hit.collider.TryGetComponent(out PlayerController player)) {
                        onNoticePlayer?.Invoke(player, Time.deltaTime * (1f / settings.numberOfVisionRays));
                        DrawVision(hit.point, Color.magenta);
                        return;
                    }
                    DrawVision(hit.point, Color.blue);
                } else {
                    DrawVision(transform.position + direction * settings.visionRayDistance, Color.red);
                }
            }
        }
        void DrawVision(Vector3 position, Color color) {
            if (settings.drawVisionCone) {
                Debug.DrawLine(transform.position, position, color, SECONDS_TO_DRAW);
            }
        }
    }
}