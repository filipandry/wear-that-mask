using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
  public GameObject Sick;
  public GameObject Healty;
  public GameObject Mask;

  public bool IsSick;
  public bool WearMask;

  public float Speed;

  public event EventHandler OnDie;
  public event EventHandler OnLifeSaved;

  int direction = 1;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (!Utils.IsGameStarted)
    {
      return;
    }
    ManageLayers();
    CheckClick();
    //var direction = Vector3.down;
    //if (IsSick)
    //{
    //  direction = Vector3.up;
    //}
    transform.Translate(Vector3.down * direction * Time.deltaTime * Speed);
  }
  void CheckClick()
  {
    if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began))
    {
      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

      RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
      if(hit.transform == null)
      {
        return;
      }
      if(hit.transform.GetInstanceID() == transform.GetInstanceID())
      {
        WearMask = true;
      }
    }
  }
  void ManageLayers()
  {
    if (IsSick && Healty.activeSelf && !Sick.activeSelf)
    {
      Sick.SetActive(true);
      Healty.SetActive(false);
    }
    else if (!IsSick && !Healty.activeSelf && Sick.activeSelf)
    {
      Sick.SetActive(false);
      Healty.SetActive(true);
    }
    if (WearMask && !Mask.activeSelf)
    {
      Mask.SetActive(true);
    }
    else if (!WearMask && Mask.activeSelf)
    {
      Mask.SetActive(false);
    }
  }
  private void OnTriggerEnter2D(Collider2D collision)
  {
    var listC = new List<string>
    {
      "Male","Female"
    };
    if (collision.gameObject.CompareTag("sick_zone"))
    {
      if (WearMask)
      {
        GetComponent<CapsuleCollider2D>().enabled = false;
        StartCoroutine(DestroyMe());
        OnLifeSaved?.Invoke(this, new EventArgs());
      }
      else if (IsSick)
      {
        //die
        OnDie?.Invoke(this, new EventArgs());
      }
      else
      {
        IsSick = true;
        direction *= -1;
      }
    }
    else if (collision.gameObject.CompareTag("healty_zone"))
    {
      IsSick = false;
      direction *= -1;
    }
    else
    {
      var c = collision.gameObject.GetComponent<CharacterController>();
      if (c != null && c.IsSick && !c.WearMask && !WearMask)
      {
        IsSick = true;
      }
    }
  }
  private void OnCollisionEnter2D(Collision2D collision)
  {
    
  }

  IEnumerator DestroyMe()
  {
    yield return new WaitForSeconds(1);
    Destroy(gameObject);
  }

}
