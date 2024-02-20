using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleBtn : MonoBehaviour
{
    public void BtnStart()
    {
        SceneManager.LoadScene("MainScenes");
    }

    public void BtnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
