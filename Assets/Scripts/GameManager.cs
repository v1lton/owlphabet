using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Mia player;
    private Spawner spawner;

    public Text scoreText;
    public GameObject playButton;
    //public GameObject gameOver;
    public int score; //{ get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        player = FindObjectOfType<Mia>();
        spawner = FindObjectOfType<Spawner>();

        Pause();
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        //gameOver.SetActive(false);

        Time.timeScale = 1f;
        player.enabled = true;

        Columns[] columns = FindObjectsOfType<Columns>();

        for (int i = 0; i < columns.Length; i++) {
            Destroy(columns[i].gameObject);
        }
    }

    public void GameOver()
    {
        playButton.SetActive(true);
        //gameOver.SetActive(true);

        Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = false;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
