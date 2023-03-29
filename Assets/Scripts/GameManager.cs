using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Mia player;
    public Spawner Spawner { get; private set; }

    public Text scoreText;
    public Text word;
    public GameObject playButton;
    public int score; //{ get; private set; }

    private void Awake()
    {
        Application.targetFrameRate = 60;

        player = FindObjectOfType<Mia>();
        Spawner = FindObjectOfType<Spawner>();

        Pause();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // C# é uma piada nmlr, olha o que tem que fazer pra popar um char
            if (word.text.Length > 0) 
                word.text = word.text.Substring(0, word.text.Length - 1);
        }
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        //gameOver.SetActive(false);

        word.text = string.Empty;

        Time.timeScale = 1f;
        player.enabled = true;
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

    public void AddLetter(string letter)
    {
        word.text += letter;
    }
}
