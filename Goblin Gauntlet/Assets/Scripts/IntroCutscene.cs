using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class IntroCutscene : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public Button skipButton;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.Play();

        videoPlayer.loopPointReached += OnVideoEnded;

        Invoke("ShowButton", 6.5f);
    }

    void ShowButton()
    {
        skipButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(skipButton.gameObject);
    }

    public void OnSkipClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void OnVideoEnded(VideoPlayer videoPlayer)
    {
        SceneManager.LoadScene("MainMenu");
    }
}
