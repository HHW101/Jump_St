using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void Awake()
    {
        EventBus.Subscribe("Restart", Restart);
    }
    public void Restart(object obj)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
