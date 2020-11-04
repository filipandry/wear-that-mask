using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameController : MonoBehaviour
{
  //public variables
  public List<GameObject> Characters;
  public GameObject StartMenu;
  public GameObject GameOver;
  public GameObject GameUI;
  public UnityEngine.UI.Text ScoreText;

  //private variables
  int Score = 0;
  Coroutine spawnCoroutine;
  int speed = 1;

  int spawnCount = 0;

  // Start is called before the first frame update
  void Start()
  {
    Utils.IsGameStarted = false;
    GameOver.SetActive(false);
    GameUI.SetActive(false);

  }

  // Update is called once per frame
  void Update()
  {
    if (!Utils.IsGameStarted)
    {
      CheckStartGame();
    }
    ScoreText.text = Score.ToString();
  }

  void CheckStartGame()
  {
    if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
    {
      Utils.IsGameStarted = true;
      StartMenu.SetActive(false);
      GameOver.SetActive(false);
      GameUI.SetActive(true);
      spawnCoroutine = StartCoroutine(SpawnCharacter());
    }
    
  }

  IEnumerator SpawnCharacter()
  {
    var rndWait = Random.Range(0.2f, 4f);
    yield return new WaitForSeconds(rndWait);

    var rndX = Random.Range(-8.51f, 8.51f);
    var charIndex = Mathf.RoundToInt(Random.Range(0f, 1f));

    var obj = Instantiate(Characters[charIndex], new Vector3(rndX, 4.57f, 0), Quaternion.identity);
    var c = obj.GetComponent<CharacterController>();
    c.OnDie += C_OnDie;
    c.OnLifeSaved += C_OnLifeSaved;
    c.Speed = speed;
    spawnCount++;
    if(spawnCount% 10 == 0)
    {
      speed++;
    }
    spawnCoroutine = StartCoroutine(SpawnCharacter());
  }

  private void C_OnLifeSaved(object sender, System.EventArgs e)
  {
    Score++;
  }

  private void C_OnDie(object sender, System.EventArgs e)
  {
    StopCoroutine(spawnCoroutine);
    Utils.IsGameStarted = false;
    GameOver.SetActive(true);
    GameUI.SetActive(false);
  }
}