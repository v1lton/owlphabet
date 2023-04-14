using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private string[] alphabet = {
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M",
    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Z",
    "!"};

    private string[] AlphabetAfterConsonant = {
        "A", "E", "I", "O", "U", "R", "!"
    };

    private const float MAX_HEIGHT = 5f;
    private const float MIN_HEIGHT = -1.5f;
    private const float LETTER_MOVE_SPEED = 10f;
    private const float LETTER_DESTROY_X_POSITION = -13f;
    private const float LETTER_SPAWN_X_POSITION = 13f;

    private List<GameObject> letters;
    private float letterSpawnTimer;
    private float letterSpawnDelay;

    public global::System.String[] Alphabet { get => alphabet; set => alphabet = value; }

    private void Awake()
    {
        letters = new();
        letterSpawnDelay = 1f;
    }

    private void Start() { }

    private void Update()
    {
        HandleLettersMovement();
        HandleLettersSpawning();
    }
    
    private void HandleLettersSpawning()
    {
        letterSpawnTimer -= Time.deltaTime;
        if (letterSpawnTimer <= 0f)
        {
            letterSpawnTimer += letterSpawnDelay;
            Spawn();
        }
    }

    private void HandleLettersMovement()
    {
        for (int i = 0; i < letters.Count; i++)
        {
            GameObject letter = letters[i];
            letter.transform.position += new Vector3(-1, 0, 0) * LETTER_MOVE_SPEED * Time.deltaTime;
            if (letter.transform.position.x < LETTER_DESTROY_X_POSITION)
            {
                DestroyLetter(letter);
                i--;
            }
        }
    }

    private void Spawn()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        string lastLetter = manager.GetLastLetter().ToString();
        string letter = Alphabet[Random.Range(0, Alphabet.Length)];
        int chanceIndex = Random.Range(0, 4);
        if (!AlphabetAfterConsonant.ToList().Contains(lastLetter) && chanceIndex >= 1) {
            letter = AlphabetAfterConsonant[Random.Range(0, AlphabetAfterConsonant.Length)];
        }
        Vector3 position = new Vector3(LETTER_SPAWN_X_POSITION, Random.Range(MIN_HEIGHT, MAX_HEIGHT), 0);
        GameObject letterObject = Instantiate(prefab, position, Quaternion.identity);
        letterObject.GetComponentInChildren<TextMesh>().text = letter;
        letters.Add(letterObject);
    }

    public void DestroyLetter(GameObject letter)
    {
        Destroy(letter);
        letters.Remove(letter);
    }
}
