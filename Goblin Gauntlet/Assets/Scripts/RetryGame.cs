using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryGame : MonoBehaviour
{

    public string sceneName;

    public void Reloadlevel()
    {
        SceneManager.LoadScene(sceneName);
    }

}
