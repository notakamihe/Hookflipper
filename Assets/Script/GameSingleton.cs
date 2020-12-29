using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSingleton : MonoBehaviour
{
    #region Singleton

    public static GameSingleton instance;

    void Awake ()
    {
        instance = this;
    }

    #endregion

    public PlayerMovement player;
    public RuntimeAnimatorController primitiveAnimatorController;
    public List<Enemy> allEnemies = new List<Enemy>();
    public string leftClickInputName = "Shoot";

    public void CallCoroutine (IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
