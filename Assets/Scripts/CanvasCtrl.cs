using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasCtrl : MonoBehaviour
{
    public AudioSource GoodEndSound;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void AnimationOver()
    {
        anim.SetBool("ShakeApple", false);
        anim.SetBool("ShakeFuel", false);
        anim.SetBool("ShakeBit", false);
    }
    void AnimationMatchesOver()
    {
        anim.SetBool("ShakeMatches", false);
    }
    void PlayGoodEndSound()
    {
        GoodEndSound.Play();
    }
    void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}
