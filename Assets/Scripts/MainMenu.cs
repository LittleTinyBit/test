using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMenu : MonoBehaviour
{
    public bool MusicEnable = true;
    public bool isEng = true;
    public bool isEasy = true;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void PlayClick()
    {
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("isPlay", true);
    }
    public void MusicToggle()
    {
        MusicEnable = !MusicEnable;   
        this.GetComponent<AudioSource>().mute = !MusicEnable;
        GameObject.Find("Canvas").GetComponent<Animator>().SetBool("isSad", !MusicEnable);        
    }
    public void EasyModeToggle()
    {
        isEasy = !isEasy;
        if(!isEasy)
        {
            GameObject.Find("Canvas").GetComponent<Animator>().SetBool("isYell", true);
            GameObject.Find("Yeah").GetComponent<AudioSource>().Play();
        }
    }
}
