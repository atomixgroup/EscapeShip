using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;
    public GameObject playerExplosion;
    public GameObject LeftButton;
    public GameObject RightButton;
    public static GameObject destroyedAstroid;
    public int pos = 1;
    public int currentPos = 1;
    private float[] positions;
    private bool animate = false;
    private int direction = 1;
 
    void Start()
    {
        positions = new float[3] {-5, 0, 5};
        Time.timeScale = 1f;
    }

    public void LeftButtonPressed()
    {
        if (pos != 0)
        {
            pos--;
            animate = true;
            direction = -1;
        }
    }

    public void RightButtonPressed()
    {
        switch (pos)
        {
            case 0:
                pos=1;
                animate = true;
                direction = 1;
                break;
            case 1:
                pos=2;
                animate = true;
                direction = 1;
                break;
        }
    }

    void Update()
    {
        if (animate)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(positions[pos], transform.position.y, transform.position.z), Time.deltaTime * moveSpeed);
            if (direction == -1)
            {
                if (transform.position.x - positions[pos] <= .1f)
                {
                    animate = false;
                    transform.position = new Vector3(positions[pos], transform.position.y, transform.position.z);
                }
            }
            else
            {
                if (transform.position.x - positions[pos] >= .1f)
                {
                    animate = false;
                    transform.position = new Vector3(positions[pos], transform.position.y, transform.position.z);
                }
            }
        }
       if (GameController.respawnEverything)
       {
           if (destroyedAstroid.transform.position.z <= -11.5)
           {
               destroyedAstroid.GetComponentInChildren<Renderer>().enabled = true;
               destroyedAstroid.GetComponentInChildren<Collider>().enabled = true;
               GameController.respawnEverything = false;
           }
       }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Astroid"))
        {
            GameController gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
            gm.EndGame();
            Instantiate(playerExplosion, gameObject.transform.position, transform.rotation);
            destroyedAstroid = other.gameObject;
            gameObject.SetActive(false);
            destroyedAstroid.GetComponentInChildren<Renderer>().enabled = false;
            destroyedAstroid.GetComponentInChildren<Collider>().enabled = false;
        }
    }
}