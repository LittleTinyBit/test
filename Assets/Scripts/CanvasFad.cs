using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasFad : MonoBehaviour
{
    public Transform T;
    public Transform C;
    void LoadMainGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OpenTutorial()
    {
        T.transform.localPosition = Vector3.zero;
    }
    public void CloseTutorial()
    {
        T.transform.localPosition = Vector3.right * 10000;
    }
    public void ScrollTutorial()
    {
        if (C.transform.localPosition.x > 0)
        {
            C.transform.localPosition = Vector3.zero;
        }
        else if (C.transform.localPosition.x < -2000)
        {
            C.transform.localPosition = Vector3.left * 2000;
        }
    }
}
