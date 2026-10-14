using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    public static SpeedLines Instance;

    [Header("Line Settings")]
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private float spawnInterval = 0.15f;
    [SerializeField] private float lineSpeed = 20f;
    [SerializeField] private float lineLifetime = 1.2f;

    [Header("Spawn Area")]
    [SerializeField] private float spawnXOffset = 10f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    private bool isActive = false;
    private float spawnTimer = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!isActive || Player.Instance == null || !Player.Instance.isAlive) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnLine();
            spawnTimer = spawnInterval;
        }
    }

    private void SpawnLine()
    {
        if (linePrefab == null) return;

        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(spawnXOffset, randomY, 0f);

        GameObject line = Instantiate(linePrefab, spawnPos, Quaternion.identity, transform);
        SpeedLine lineScript = line.GetComponent<SpeedLine>();
        if (lineScript != null)
        {
            lineScript.Initialize(lineSpeed, lineLifetime);
        }
    }

    public void ShowLines()
    {
        isActive = true;
        spawnTimer = 0f;
    }

    public void HideLines()
    {
        isActive = false;
        // Destroy existing lines
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
}
