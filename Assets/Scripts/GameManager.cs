using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

  public GameObject basketballPrefab;
  public Transform goal_transform;

  public int difficultyModifier = 1;

  private Camera main_camera;
  private int score;
  private Vector3 offset = new Vector3(0, -0.75f, 0);
  private bool needNewBall = true;
  private float distance = 2f;
  private Vector3 newPosition;
  private float moveTime = 2.5f;
  private float startTime = 0f;

  private Text scoreText;

  void Awake() {
    this.main_camera = Camera.main;
    this.scoreText = GameObject.Find("Score Text").GetComponent<Text>();;
  }

  void Start() {
    MoveToNewPosition();
  }

  void Update() {
    main_camera.transform.LookAt(goal_transform);
    if (Vector3.Distance(main_camera.transform.position, newPosition) > 0.05f) {
      float fracComplete = (Time.time - startTime) / moveTime;
      main_camera.transform.position = Vector3.Lerp(main_camera.transform.position, newPosition, fracComplete);
    } else if (needNewBall) {
      main_camera.transform.position = newPosition;
      CreateBall();
    }

  }

  public void Goal(bool hitRim) {
    var points = (hitRim) ? 2 : 3;

    this.score += points;

    UpdateScoreText();
    MoveToNewPosition();
  }

  public void MoveToNewPosition() {
    needNewBall = true;
    newPosition = NewPosition(20f, Random.Range(90f, 270f));
    startTime = Time.time;
  }

  public void CreateBall() {
    needNewBall = false;
    Vector3 pos = main_camera.transform.position + (main_camera.transform.forward * distance) + offset;
    GameObject basketball = Instantiate(basketballPrefab, pos, Quaternion.identity);
    basketball.transform.LookAt(goal_transform);
    BallController basketballController = basketball.GetComponent<BallController>();
    basketballController.manager = this;
  }

  private Vector3 NewPosition(float radius, float angle){
    float rad = angle * Mathf.Deg2Rad;
    Vector3 position = goal_transform.right * Mathf.Sin( rad ) + goal_transform.forward * Mathf.Cos( rad );
    Vector3 output = goal_transform.position + position * radius;
    output.y = 6f;
    return output;
  }

  private void UpdateScoreText() {
    this.scoreText.text = "Score: " + this.score;
  }

}
