using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float restartDelay = 1f;

    private bool gameHasEnded = false;

    public void EndGame()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        // reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
}
