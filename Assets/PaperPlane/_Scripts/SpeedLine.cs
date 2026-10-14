using UnityEngine;

public class SpeedLine : MonoBehaviour
{
    private float speed;
    private float lifetime;

    public void Initialize(float moveSpeed, float life)
    {
        speed = moveSpeed;
        lifetime = life;
    }

    private void Update()
    {
        // Move left
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Countdown and destroy
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
