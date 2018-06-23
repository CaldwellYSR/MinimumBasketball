using UnityEngine;

public class GameManager : MonoBehaviour {

  public Camera main_camera;
  public GameObject basketballPrefab;
  public Transform goal_transform;

  public Vector3 offset;

  [Range(1f, 5f)]
  public float distance;

  private GameObject basketball;

  void Start() {
    CreateBall();
  }

  void Update() {
    main_camera.transform.LookAt(goal_transform);
  }

  public void Goal() {
    Debug.Log("GOAL");
  }

  private void CreateBall() {
    main_camera.transform.LookAt(goal_transform);
    Vector3 pos = main_camera.transform.position + (main_camera.transform.forward * distance) + offset;
    basketball = Instantiate(basketballPrefab, pos, Quaternion.identity);
    BallController basketballController = basketball.GetComponent<BallController>();
    basketballController.manager = this;
  }

}
