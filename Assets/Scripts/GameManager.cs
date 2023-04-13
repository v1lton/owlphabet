using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Mia player;
    public Spawner Spawner { get; private set; }

    public Text scoreText;
    public Text word;
    public GameObject playButton;
    public int score; //{ get; private set; }
    private WordChecker wordChecker;
    private const string POP_CHAR = "!";

    // eu odeio a string do C#, plmds
    private char[] letters;
    private int wordLenght;
    private int wordIndex;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        word.fontSize = 100;
        player = FindObjectOfType<Mia>();
        Spawner = FindObjectOfType<Spawner>();

        string wordsFilePath = Application.dataPath + "/Scripts/words.txt";
        wordChecker = new WordChecker(wordsFilePath);

        Pause();
    }

    private void PopChar()
    {
        if (wordIndex == 0)
            return;
        letters[--wordIndex] = '_';
        word.text = String.Join("", letters);
    }

    private void NextLevel()
    {
        // Por enquanto, só tamanhos válidos de 1 ~ 10
        word.text = string.Empty;
        wordIndex = 0;
        letters = new char[wordLenght];
        for (int i = 0; i < wordLenght; i++)
            letters[i] = '_';
        word.text = String.Join(" ", letters);
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

        NextLevel();
    }

    public void GameOver()
    {
        player.ResetPlayer();
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
        if (letter == POP_CHAR)
        {
            PopChar();
            return; 
        }

        letters[wordIndex++] = Convert.ToChar(letter);
        word.text = String.Join(" ", letters);
        String str = String.Join("", letters);

        if (wordIndex == wordLenght) { 
            if (wordChecker.IsWordInTrie(str)) {
                IncreaseScore();
                NextLevel();
            } else {
                // TODO: lidar quando a palavra não existe
                // GameOver();
                NextLevel();
            }
        }
    }
}
