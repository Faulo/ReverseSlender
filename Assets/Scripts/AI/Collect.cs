using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseSlender.AI {
    public class Collect : MonoBehaviour {
        private void OnCollisionEnter(Collision collision) {
            var collectable = collision.gameObject.GetComponent<Collectable>();
            if (collectable) {
                Debug.Log(string.Format("I, {0}, found an item!", gameObject));
                Destroy(collectable.gameObject);
            }
        }
    }
}