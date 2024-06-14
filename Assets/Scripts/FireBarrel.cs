using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBarrel : MonoBehaviour
{
    public List<AudioSource> Sounds;
    public Text FireTimeTXT;
    public MeshRenderer ColdIMG;
    public MeshRenderer WarmIMG;
    public Image CoalIMG;
    public Image WoodIMG;
    public Light LightParam;
    GameChanger GC;
    float FireTime = 0;
    public bool isFire = false;
    void Start()
    {
        GC = Camera.main.GetComponent<GameChanger>();//on main camera
        StartCoroutine(Flaming());
    }
    private void FixedUpdate()
    {
        if(GC.isDay)
        {
            this.GetComponent<Interacrtable>().isInteractable = false;
        }
        else
        {
            this.GetComponent<Interacrtable>().isInteractable = true;
        }
        if(GC.GameOn&&isFire&&FireTime>0)
        {
            //2.2 is minimum light
            LightParam.range = Mathf.Lerp(LightParam.range, Mathf.Clamp(2.2f + FireTime * 0.09f, 2.2f, 5f),0.01f);
            Sounds[2].mute = false;
            ColdIMG.enabled = false;
            WarmIMG.enabled = true;
        }
        else
        {
            Sounds[2].mute = true;
            ColdIMG.enabled = true;
            WarmIMG.enabled = false;
            LightParam.range = 0;
        }
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(!GC.isDay)
        {
            if (other.tag == "Player" && other.GetComponent<PlayerCtrl>().interact)
            {
                if (other.GetComponent<PlayerCtrl>().Inventory == 1)//put firewood
                {
                    GC.FuelIMG[0].enabled = false;
                    other.GetComponent<PlayerCtrl>().Inventory = 0;
                    FireTime +=4.5f+(GC.isEasy?0.4f:0);
                    WoodIMG.enabled = true;
                    Sounds[0].Play();
                }
                else if (other.GetComponent<PlayerCtrl>().Inventory == 2)//put charcoal
                {
                    GC.FuelIMG[1].enabled = false;
                    other.GetComponent<PlayerCtrl>().Inventory = 0;
                    FireTime += 15;
                    CoalIMG.enabled = true;
                    Sounds[0].Play();
                }
                else//just interact
                {
                    if (!isFire && FireTime > 0)
                    {
                        if (GC.AllMatches > 0)
                        {
                            GC.MatchesIMG[--GC.AllMatches].enabled = false;
                            GC.CanvasAnim.SetBool("ShakeMatches", true);
                            isFire = true;
                            CoalIMG.enabled = false;
                            WoodIMG.enabled = false;
                            ColdIMG.enabled = false;
                            WarmIMG.enabled = true;
                            Sounds[1].Play();
                        }
                    }
                }
                if (isFire)
                {
                    LightParam.range = Mathf.Clamp(2f + FireTime * 0.09f, 2.2f, 5f);//2.2 is minimum light
                    CoalIMG.enabled = false;
                    WoodIMG.enabled = false;
                }
                other.GetComponent<PlayerCtrl>().answer = true;
                other.GetComponent<PlayerCtrl>().interact = false;
            }
        }
    }
    private IEnumerator Flaming ()
    {
        while(true)
        {
            FireTimeTXT.text = FireTime.ToString();
            if (isFire)
            {
                if (FireTime > 0)
                {
                    FireTime -= 0.1f + 0.01f * Random.Range(0, GC.Days + 1);
                }
                else
                {
                    isFire = false;
                    LightParam.range = 0;
                }
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}
