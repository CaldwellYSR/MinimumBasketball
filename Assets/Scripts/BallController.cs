using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

  public GameObject goal;

  [Range(0, 25f)]
  public float speed;

  public ParticleSystem goal_burst;

  private Rigidbody body;

  void Awake() {
    body = GetComponent<Rigidbody>();
    goal_burst.emissionRate = 0f;
  }

  void OnTriggerEnter(Collider collision) {
    if (collision.CompareTag("Goal")) {
      goal_burst.Emit(50);
    }
  }
}
