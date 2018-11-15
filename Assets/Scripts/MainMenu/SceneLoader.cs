using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(int level)
    {
        SceneManager.LoadScene(level);
        SoundManager.PlaySfx(SoundManager.GetSFX(SFX.startGame));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
