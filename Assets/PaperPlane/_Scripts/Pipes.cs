using System.Threading;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    [Header("Speed Settings")]
    public static float normalSpeed = 5f;
    public static float fastSpeed = 8.5f;
    private static float currentSpeedMultiplier = 1f;

    public float timeToLive = 7;

    // Update is called once per frame
    void Update()
    {
        // Only move AND countdown if player alive
        if (Player.Instance != null && Player.Instance.isAlive)
        {
            float speed = currentSpeedMultiplier == 1f ? normalSpeed : fastSpeed;
            transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);

            if (timeToLive > 0)
            {
                timeToLive -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject); // When dead: pipes do nothing - frozen forever
            }
        }
    }

    public static void SetNormalSpeed()
    {
        currentSpeedMultiplier = 1f;
    }

    public static void SetFastSpeed()
    {
        currentSpeedMultiplier = 1.2f;
    }
}
