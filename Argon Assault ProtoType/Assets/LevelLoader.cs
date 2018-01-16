using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Invoke("LoadFirstScene", 8f);
    }

    void LoadFirstScene()
    {
        SceneManager.LoadScene(1);
    }

}
