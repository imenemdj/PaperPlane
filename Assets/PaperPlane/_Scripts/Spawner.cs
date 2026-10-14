using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance;

    public GameObject Pipe;

    public float offset = 3F;

    [Header("Speed Settings")]
    public float normalCoolDown = 2f;
    public float fastCoolDown = 1.3f;

    private float currentCoolDown;
    float timeLeftTilNextPipe = 0;

    private void Awake()
    {
        Instance = this;
        currentCoolDown = normalCoolDown;
        Pipes.SetNormalSpeed(); // Reset static pipe speed when spawner initializes
    }

    // Update is called once per frame
    void Update()
    {
        if (timeLeftTilNextPipe <= 0 && Player.Instance.isAlive)
        {
            Instantiate(Pipe, new Vector3(transform.position.x, transform.position.y + Random.Range(-offset, offset),transform.position.z), Quaternion.identity);
            timeLeftTilNextPipe = currentCoolDown;
        }
        else
        {
            timeLeftTilNextPipe -= Time.deltaTime;
        }
    }

    public void SetNormalSpeed()
    {
        currentCoolDown = normalCoolDown;
        // Clamp timer if it exceeds new cooldown, otherwise keep current
        if (timeLeftTilNextPipe > currentCoolDown)
            timeLeftTilNextPipe = currentCoolDown;
    }

    public void SetFastSpeed()
    {
        currentCoolDown = fastCoolDown;
        // Clamp timer if it exceeds new cooldown, otherwise keep current
        if (timeLeftTilNextPipe > currentCoolDown)
            timeLeftTilNextPipe = currentCoolDown;
    }
}
