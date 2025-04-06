using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player entered FinishPoint");

            if (SceneController.instance.AllEnemiesDefeated())
            {
                Debug.Log("All enemies defeated, loading next level...");
                SceneController.instance.NextLevel();
            }
            else
            {
                Debug.Log("Enemies remaining, can't load next level yet.");
            }
        }
    }

}
