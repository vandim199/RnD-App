using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(string levelName)
    {
        SetGameSpeed(1);
        SceneManager.LoadScene(levelName);
    }

    public void SetGameSpeed(int speed)
    {
        Time.timeScale = speed;
    }
}
