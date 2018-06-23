using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

  public GameManager manager;

  private Vector3 startPos;
  private float startTime;

  void OnMouseDown() {
    startTime = Time.time;
    startPos = Input.mousePosition;
  }

  void OnMouseUp() {
    Vector3 endPos = Input.mousePosition;
    float endTime = Time.time;

    startPos += (Camera.main.transform.forward * 2);
    endPos += (Camera.main.transform.forward * 6);


    startPos = Camera.main.ScreenToWorldPoint(startPos);
    endPos = Camera.main.ScreenToWorldPoint(endPos);

    float duration = endTime - startTime;

    Vector3 direction = endPos - startPos;

    float distance = direction.magnitude;

    float power = distance / duration;

    Vector3 velocity = (transform.rotation * direction).normalized * power;

    GetComponent<Rigidbody>().useGravity = true;
    GetComponent<Rigidbody>().AddForce(velocity, ForceMode.Impulse);
  }

  void OnTriggerEnter(Collider collision) {
    if (collision.CompareTag("Goal")) {
      manager.Goal();
    }
  }
}
