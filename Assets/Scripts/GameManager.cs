using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public Mia player;
    public Spawner Spawner { get; private set; }

    public Text scoreText;
    public Text word;
    public int score; //{ get; private set; }
    public Image[] heartImages;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    [SerializeField] private AudioSource completeWordSoundEffect;
    [SerializeField] private AudioSource failSoundEffect;
    [SerializeField] private AudioSource lostLetterSoundEffect;


    private WordChecker wordChecker;
    private const string POP_CHAR = "!";

    private char[] letters;
    private int wordLength;
    private int wordIndex;

    private int lives = 3;
    private Char lastLetter = 'a';
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
        word.fontSize = 100;
        player = FindObjectOfType<Mia>();
        Spawner = FindObjectOfType<Spawner>();

        string wordsFilePath = Application.dataPath + "/Scripts/words.txt";
        wordChecker = new WordChecker(wordsFilePath);

        Play();
    }

    private void PopChar()
    {
        if (wordIndex == 0)
            return;
        letters[--wordIndex] = '_';
        lastLetter = letters[wordIndex];
        word.text = String.Join(" ", letters);
    }

    private void NextLevel()
    {
        lastLetter = 'a';
        word.text = string.Empty;
        wordIndex = 0;
        if (score <= 4) {
            wordLength = UnityEngine.Random.Range(3, 5);
        } else if (score <= 12) {
            wordLength = UnityEngine.Random.Range(4, 6);
        } else {
            wordLength = UnityEngine.Random.Range(6, 12);
        }
        
        letters = new char[wordLength];
        for (int i = 0; i < wordLength; i++)
            letters[i] = '_';
        word.text = String.Join(" ", letters);
    }

    public void Play()
    {
        score = 0;
        scoreText.text = score.ToString();

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

        lives = 3;
        for (int i = 0; i < heartImages.Length; i++) {
            heartImages[i].sprite = fullHeart;
        }
        Quit();
    }

    public void Quit()
    {
        Time.timeScale = 0f;
        player.enabled = false;
        PlayerPrefs.SetString("ScoreText", scoreText.text);
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
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
            lostLetterSoundEffect.Play();
            PopChar();
            return; 
        }

        letters[wordIndex++] = Convert.ToChar(letter);
        lastLetter = Convert.ToChar(letter);
        word.text = String.Join(" ", letters);
        String str = String.Join("", letters);

        if (wordIndex == wordLength) { 
            if (wordChecker.IsWordInTrie(str)) {
                completeWordSoundEffect.Play();
                IncreaseScore();
                NextLevel();
            } else {
                lives--;
                failSoundEffect.Play();
                if (lives > 0) {
                    // atualiza a quantidade de imagens de vida na tela
                    if (lives < heartImages.Length) {
                        heartImages[lives].sprite = emptyHeart;
                    }
                    NextLevel();
                } else {
                    GameOver();
                }
            }
        }
    }

    public Char GetLastLetter()
    {
        return lastLetter;
    }



    public char[] GetPossibleLetters() {
        
        String str = string.Join("", Array.FindAll(letters, c => c != '_'));
        char[] possibleLetters = wordChecker.PossibleLettersInTrie(str, wordLength);
        return possibleLetters;
    }
}
