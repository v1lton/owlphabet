using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Spawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    private string[] alphabet = {
    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M",
    "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Z",
    "!"
    };

    private const float MAX_HEIGHT = 4.3f;
    private const float MIN_HEIGHT = -1.5f;
    private const float BUBBLE_MOVE_SPEED = 10f;
    private const float BUBBLE_DESTROY_X_POSITION = -13f;
    private const float BUBBLE_SPAWN_X_POSITION = 13f;

    private List<GameObject> bubbles;
    private float bubbleSpawnTimer;
    private float bubbleSpawnDelay;

    public global::System.String[] Alphabet { get => alphabet; set => alphabet = value; }

    private void Awake()
    {
        bubbles = new();
        bubbleSpawnDelay = 1f;
    }

    private void Start() { }

    private void Update()
    {
        HandleBubblesMovement();
        HandleBubblesSpawning();
    }
    
    private void HandleBubblesSpawning()
    {
        bubbleSpawnTimer -= Time.deltaTime;
        if (bubbleSpawnTimer <= 0f)
        {
            bubbleSpawnTimer += bubbleSpawnDelay;
            Spawn();
        }
    }

    private void HandleBubblesMovement()
    {
        for (int i = 0; i < bubbles.Count; i++)
        {
            GameObject bubble = bubbles[i];
            bubble.transform.position += new Vector3(-1, 0, 0) * BUBBLE_MOVE_SPEED * Time.deltaTime;
            if (bubble.transform.position.x < BUBBLE_DESTROY_X_POSITION)
            {
                DestroyBubble(bubble);
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

        // Spawn a bubble with the letter inside it
        Vector3 position = new Vector3(BUBBLE_SPAWN_X_POSITION, UnityEngine.Random.Range(MIN_HEIGHT, MAX_HEIGHT), 0);
        GameObject bubbleObject = Instantiate(bubblePrefab, position, Quaternion.identity);
        GameObject letterObject = bubbleObject.transform.Find("Letter").gameObject;
        letterObject.GetComponentInChildren<TextMesh>().text = letter;
        bubbles.Add(bubbleObject);

        Debug.Log(bubbleObject);
    }

    public void DestroyBubble(GameObject bubble)
    {
        // Destroy the letter immediatly
        GameObject letterObject = bubble.transform.Find("Letter").gameObject;
        Destroy(letterObject);

        // Destroy the bubble after delay for animating
        Destroy(bubble, 0.3f);
        bubbles.Remove(bubble);
    }
}
