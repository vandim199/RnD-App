using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;
    public static GameManager GameManagerSingleton { get { return gameManager; } }

    [SerializeField]
    private TMPro.TMP_Text text;

    public int score;
    public UnityEngine.Events.UnityEvent GameEnd;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager != null && gameManager != this) Destroy(gameObject);
        else gameManager = this;

        text.text = "Score: 0";
        score = 0;

        SetGameSpeed(1);
    }

    public void LoadLevel(string levelName)
    {
        SetGameSpeed(1);
        SceneManager.LoadScene(levelName);
    }

    public void SetGameSpeed(float speed)
    {
        Time.timeScale = speed;
    }

    public void AddScore(int points)
    {
        score += points;
        text.text = "Score: " + score;
    }

    public void EndGame()
    {
        GameEnd.Invoke();
        SetGameSpeed(0);
    }
}
