using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public void OnPlayLevelButtonClicked()
    {
        SceneManager.LoadScene("The Lab");
    }

    public void OnTutorialButtonClicked()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}
