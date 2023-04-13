using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mia : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;

    private Vector3 direction;

    private Vector3 targetPosition;
    private bool isMouseButtonPressed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void OnEnable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        UpdateTargetPosition();

        // Define a velocidade do movimento do personagem
        float speed = 5.0f;
        if (!isMouseButtonPressed) {
            targetPosition.y -= 0.04f;
            targetPosition.x += 0.01f;
        }
        // Move o personagem em direção ao targetPosition com interpolação linear
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

        // Atualiza a rotação do personagem para olhar em direção ao targetPosition
        Vector3 direction = targetPosition - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (direction.x < 0) {
            transform.localScale = new Vector3(-1, 1, 1);
            angle += 180;
        } else {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Implementa a rotação suavizada
        if (isMouseButtonPressed) {
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else {
            transform.rotation = Quaternion.AngleAxis(angle/3, Vector3.forward);
        }
    }

    private void UpdateTargetPosition()
    {
        if (Input.GetMouseButton(0)) {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -Camera.main.transform.position.z;
            Vector3 newTargetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            targetPosition = Vector3.Lerp(targetPosition, newTargetPosition, Time.deltaTime * 10);

            isMouseButtonPressed = true;
        } else {
            isMouseButtonPressed = false;
        }
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length) {
            spriteIndex = 0;
        }

        if (spriteIndex < sprites.Length && spriteIndex >= 0) {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle")) {
            FindObjectOfType<GameManager>().GameOver();
        } else if (other.gameObject.CompareTag("Letter"))
        {
            FindObjectOfType<GameManager>().Spawner.DestroyLetter(other.gameObject);
            FindObjectOfType<GameManager>().AddLetter(other.gameObject.GetComponentInChildren<TextMesh>().text);
        }
    }

    public void ResetPlayer() {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        transform.rotation = Quaternion.identity;
        direction = Vector3.zero;
        targetPosition = Vector3.zero;
    }
}
