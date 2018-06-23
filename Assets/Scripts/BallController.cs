using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

  public GameManager manager;

  private float expectedMinPower = 1f, expectedMaxPower = 25f;
  private float desiredMinPower = 200f, desiredMaxPower = 260f;

  private Vector3 startPos;
  private float startTime;

  private Vector3 lastPosition, deltaPosition;
  private bool held = false;

  void OnMouseDown() {
    held = true;
   }

  void Update() {
    if (held) {
      Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 2f));
      transform.position = position;
    }
    deltaPosition = transform.position - lastPosition;
    lastPosition = transform.position;
  }

  void OnMouseUp() {
    held = false;

    Vector3 direction = deltaPosition * 50f;

    float power = direction.magnitude;

    direction.y *= 0.75f;
    direction += Camera.main.transform.forward * (power * 0.75f);

    power -= expectedMinPower;
    power /= expectedMaxPower - expectedMinPower;

    power = Mathf.Clamp01(power);

    power *= desiredMaxPower - desiredMinPower;
    power += desiredMinPower;
    Debug.Log(power);

    Vector3 velocity = (transform.rotation * direction).normalized * power;

    GetComponent<Rigidbody>().useGravity = true;
    GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
  }

  void OnTriggerEnter(Collider collision) {
    if (collision.CompareTag("Goal")) {
      Destroy(gameObject);
      manager.Goal();
    } else if (collision.CompareTag("Ground")) {
      Destroy(gameObject);
      manager.MoveToNewPosition();
    }
  }
}
