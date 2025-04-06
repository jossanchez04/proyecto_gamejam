using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    private int enemiesRemaining;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        enemiesRemaining = 0;

    }

    public void RegisterEnemy() {
        enemiesRemaining++;
    }

    public void UnregisterEnemy() {
        enemiesRemaining--;
        Debug.Log("Enemy defeated. Remaining: " + enemiesRemaining);
    }

    public bool AllEnemiesDefeated() {

        return enemiesRemaining <= 0;
    }

    public void ResetEnemyCount() {

        enemiesRemaining = 0;
    }

    public void NextLevel() {

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName) {

        SceneManager.LoadSceneAsync(sceneName);
    }
}
