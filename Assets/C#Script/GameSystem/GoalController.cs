using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && SceneManager.GetActiveScene().name == "S1ROAD")
            SceneManager.LoadScene("S1BOSS");
        if (other.CompareTag("Player") && SceneManager.GetActiveScene().name == "S2ROAD")
            SceneManager.LoadScene("S2BOSS");
    }
}
