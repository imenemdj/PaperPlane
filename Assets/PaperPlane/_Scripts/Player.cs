using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public Rigidbody2D rigidBody2;
    public float flapForce = 3f;  // Instant upward velocity when pressing space
    public float glideAcceleration = 20f;  // Acceleration while holding space
    public float maxGlideVelocity = 5f;  // Max velocity cap while gliding
    public bool isAlive = true;

    public GameManager gameManager;

    void Awake() {
        Instance = this;  // Makes Player accessible globally
    }

    void Start() { }

    void Update()
    {
        if (!isAlive) return;

        // Instant flap on press
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidBody2.linearVelocityY = flapForce;
        }

        // Glide acceleration while holding (only if below max velocity)
        if (Input.GetKey(KeyCode.Space) && rigidBody2.linearVelocityY < maxGlideVelocity)
        {
            rigidBody2.linearVelocityY += glideAcceleration * Time.deltaTime;

            // Cap at max glide velocity
            if (rigidBody2.linearVelocityY > maxGlideVelocity)
            {
                rigidBody2.linearVelocityY = maxGlideVelocity;
            }
        }
        // When at or above max velocity: do nothing, gravity pulls down naturally
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAlive)
        {
            isAlive = false;
            AudioManager.Instance?.PlayHit();
            gameManager.GameOver();
        }
    }
}
