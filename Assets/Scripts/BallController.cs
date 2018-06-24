using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour {

  public GameManager manager;

  private float expectedMinPower = 1f, expectedMaxPower = 25f;
  private float desiredMinPower = 200f, desiredMaxPower = 260f;
  private float perfectPower = 215f;
  private bool scored = false, missed = false, hitRim = false;

  private Vector3 startPos;
  private float startTime;

  private Vector3 lastPosition, deltaPosition;
  private bool held = false;
  private float torqueModifier = 3f;

  private GameObject _target;
  private float _gravity;

  void Awake() {
    _gravity = Mathf.Abs(Physics.gravity.y);
    _target = GameObject.FindWithTag("Rim");
  }

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

    Vector3 desiredDirection = Vector3.Normalize(_target.transform.position - transform.position);
    desiredDirection.y = GetAngle(power);

    Vector3 velocity = Vector3.Lerp(direction, desiredDirection, 1 - manager.difficultyModifier * 0.01f).normalized;
    velocity *= Mathf.Lerp(power, perfectPower, 1 - manager.difficultyModifier * 0.01f);

    Rigidbody body = GetComponent<Rigidbody>();

    body.useGravity = true;
    body.AddForce(velocity, ForceMode.Impulse);
    body.AddTorque(transform.right * -power * torqueModifier);
  }

  void OnTriggerEnter(Collider collision) {
    if (collision.CompareTag("Goal") && !scored) {
      scored = true;
      Destroy(gameObject, 2f);
      manager.Goal(hitRim);
    }
  }

  void OnCollisionEnter(Collision other) {
    if (other.gameObject.CompareTag("Ground") && !scored && !missed) {
      missed = true;
      Destroy(gameObject, 3f);
      manager.MoveToNewPosition();
    }
    if (other.gameObject.CompareTag("Rim")) {
      hitRim = true;
    }
  }

  private float GetAngle(float _force) {
    float v2 = (_force * _force);
    float x = _target.transform.position.x - transform.position.x;
    float y = _target.transform.position.y - transform.position.y;

    return Mathf.Abs(Mathf.Atan((v2 + Mathf.Sqrt(v2*v2 - _gravity * (_gravity * x * x + 2 * y * v2))) / _gravity * x));
  }
}
