using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

  public GameManager manager;

  public float expectedMinPower = 4f, expectedMaxPower = 25f;
  public float desiredMinPower = 14f, desiredMaxPower = 17f;

  private Vector3 startPos;
  private float startTime;

  void OnMouseDown() {
    startTime = Time.time;
    startPos = Input.mousePosition;
  }

  void OnMouseUp() {
    Vector3 endPos = Input.mousePosition;
    float endTime = Time.time;

    startPos += (Camera.main.transform.forward * 2f);
    endPos += (Camera.main.transform.forward * 5f);

    startPos = Camera.main.ScreenToWorldPoint(startPos);
    endPos = Camera.main.ScreenToWorldPoint(endPos);

    float duration = endTime - startTime;

    Vector3 direction = endPos - startPos;

    float distance = direction.magnitude;

    direction.y = 4.5f;

    float power = distance / duration;
    Debug.Log(power);

    power -= expectedMinPower;
    power /= expectedMaxPower - expectedMinPower;

    power = Mathf.Clamp01(power);

    power *= desiredMaxPower - desiredMinPower;
    power += desiredMinPower;

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
      manager.CreateBall();
    }
  }
}
