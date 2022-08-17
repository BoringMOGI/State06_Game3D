using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    [SerializeField] string gameSceneName;

    public void OnStartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
 
}
