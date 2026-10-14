using UnityEngine;

public class Trigger : MonoBehaviour
{
    GameManager gameManager;

    public void Start()
    {
        gameManager = GameObject.FindGameObjectsWithTag("GameController")[0].GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameManager.AddScore();
    }
}
