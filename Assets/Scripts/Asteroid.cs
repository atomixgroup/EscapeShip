using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {
    private GameController gameController;
    int[] XArray = new int[] { -5, 0, 5, -5, 0, 5, -5, 0, 5 };
    public GameObject astroidExplosion;
    public float traslateSpeed;
    public static bool isPlay = true;
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        StartCoroutine(IncreaseSpeed());
    }

	void Update()
    {
        if(isPlay)
        {
            transform.Translate(transform.forward * -1 * Time.deltaTime * traslateSpeed);
        }
        if (transform.position.z <= -12)
        {
            int X = XArray[Random.Range(0, XArray.Length)];
            transform.position = new Vector3(X, 0, 24);
        }
    }
    private IEnumerator IncreaseSpeed()
    {
        while (traslateSpeed < 30)
        {
            yield return new WaitForSeconds(1);
            traslateSpeed += .2f;
        }
    }
}