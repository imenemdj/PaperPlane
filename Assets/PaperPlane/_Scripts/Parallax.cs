using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;

    [Header("Speed Settings")]
    public float normalScrollSpeed = 1f;
    public float fastScrollSpeed = 1.5f;

    private float currentScrollSpeed;
    public bool stopOnPlayerDeath = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        currentScrollSpeed = normalScrollSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // Stop moving if this layer should stop on death and player is dead
        if (stopOnPlayerDeath && Player.Instance != null && !Player.Instance.isAlive)
            return;

        // Move the background to the left
        float distance = currentScrollSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);

        // When the background has scrolled one full length, reset it
        if (transform.position.x <= startpos - length)
        {
            // Reset to starting position to create seamless loop
            transform.position = new Vector3(startpos, transform.position.y, transform.position.z);
        }
    }

    public void SetNormalSpeed()
    {
        currentScrollSpeed = normalScrollSpeed;
        Debug.Log($"Parallax {gameObject.name}: Set to normal speed {normalScrollSpeed}");
    }

    public void SetFastSpeed()
    {
        currentScrollSpeed = fastScrollSpeed;
        Debug.Log($"Parallax {gameObject.name}: Set to fast speed {fastScrollSpeed}");
    }
}
