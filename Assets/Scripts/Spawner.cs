using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private string[] alphabet = {
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M",
    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Z",
    "!"};

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
        
        char[] possibleLettersC = manager.GetPossibleLetters();
        string[] possibleLetters = Array.ConvertAll(possibleLettersC, c => c.ToString().ToUpper());

        // Chances se existe sugestão (uma letra sugerida = 12/20, uma do alfabeto = 5/20, ! = 3/20)
        // Chances se não existe sugestão (uma letra do alfabeto = 11/20, ! = 9/20)
        
        int random = UnityEngine.Random.Range(1, 21);
        if (possibleLetters.Length > 0) { // Caso exista sugestão
            if (random >= 18) {
                Debug.Log("É um (!)");
                possibleLetters = new string[] {"!"};
            } else if (random >= 13) {
                Debug.Log("Letra do Alfabeto");
                possibleLetters = Alphabet;
            } else {
                Debug.Log("Letra Sugestão");
            }
        } else { // Caso não exista sugestão
            if (random >= 12) {
                Debug.Log("É um (!)");
                possibleLetters = new string[] {"!"};
            } else {
                Debug.Log("Letra do Alfabeto");
                possibleLetters = Alphabet;
            }
        }
        
        string letter = possibleLetters[UnityEngine.Random.Range(0, possibleLetters.Length)];

        

        Vector3 position = new Vector3(LETTER_SPAWN_X_POSITION, UnityEngine.Random.Range(MIN_HEIGHT, MAX_HEIGHT), 0);
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
