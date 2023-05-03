using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameOverManager : MonoBehaviour
{
    public GameObject GameOverMenu;
    public TMPro.TextMeshProUGUI ScoreTextMesh;

    void Start()
    {
        ScoreTextMesh.text = PlayerPrefs.GetString("ScoreText");
    }

    void Update() {}
    
    public void PlayAgain()
    {
        SceneManager.LoadScene("Owlphabet", LoadSceneMode.Single);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("Start", LoadSceneMode.Single);
    }
}
