using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ground : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (Asteroid.isPlay)
        {
            transform.Translate(Vector3.down * 3 * Time.deltaTime);
            if (transform.position.z <= -26)
            {
                transform.position = new Vector3(0, -10, 38);
            }
        }
        else
        {
            transform.Translate(transform.forward * -1 * Time.deltaTime * 0);
        }
    }
}