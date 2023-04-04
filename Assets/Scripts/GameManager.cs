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
    private WordChecker wordChecker;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        player = FindObjectOfType<Mia>();
        Spawner = FindObjectOfType<Spawner>();

        string wordsFilePath = Application.dataPath + "/Scripts/words.txt";
        wordChecker = new WordChecker(wordsFilePath);

        Pause();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // C# � uma piada nmlr, olha o que tem que fazer pra popar um char
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

        string wordsFilePath = Application.dataPath + "/Scripts/words.txt";
        wordChecker = new WordChecker(wordsFilePath);
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
        if (word.text.Length >= 3) { 
            if (wordChecker.IsWordInTrie(word.text)) {
                IncreaseScore();
            } else {
                GameOver();
            }
        }
    }
}
