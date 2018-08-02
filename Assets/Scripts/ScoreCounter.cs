using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
	public static bool enable = true;
	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.tag.Equals("Astroid") && enable)
        {
            GameController.score++;
        }
	}
}
