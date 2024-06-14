using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public List<AudioSource> Sounds;
    public bool interact = false;
    public bool answer = false;
    public bool isWarm = true;
    public bool hasApple = false;
    public float speed = 100;
    public byte Inventory = 0;//0-nothing, 1-firewood, 2-charcoal
    public Transform AnimationObject;//Where animation
    public Transform CameraPosition;//Object what camera folow
    public Transform FamishedIMG;
    GameChanger GC;
    Rigidbody rig;
    public Animator anim;
    public bool isStop = false;
    bool isFamished = false;
    bool buyApple = false;
    Vector3 FamishedIMGStartPosition;
    float MaxSpeed = 7f;
    Vector3 AnimationCamPos = new Vector3(0, 3, -5);
    Vector3 DefaultCamPos;
    void Start()
    {
        DefaultCamPos = CameraPosition.localPosition;
        GC = Camera.main.GetComponent<GameChanger>();//on main camera
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        FamishedIMGStartPosition = FamishedIMG.transform.localPosition;
        FamishedIMG.transform.localPosition = Vector3.down * 1000;
    }
    void FixedUpdate()
    {
        if(!isStop&&GC.GameOn)
        {
            Moving();
        }
        Interacting();
        if(GC.isDay)
        {
            Sounds[4].mute = true;
        }
        else
        {
            if(GC.GameOn)
            {
                Sounds[4].mute = false;
            }
            else
            {
                Sounds[4].mute = true;
            }
        }
    }
    public void ToFamished(bool Eaten=false)
    {
        if(!Eaten)
        {
            MaxSpeed = 4;
            anim.SetFloat("Speed", 1);
            FamishedIMG.transform.localPosition = FamishedIMGStartPosition;
            isFamished = true;
            Sounds[5].Play();
            this.GetComponent<AudioSource>().pitch = 0.8f;
        }
        else
        {
            MaxSpeed = 7;
            anim.SetFloat("Speed", 2);
            FamishedIMG.transform.localPosition = Vector3.down * 1000;
            isFamished = false;
            this.GetComponent<AudioSource>().pitch = 1.5f;
            Sounds[0].Play();
            GC.AppleIMG.enabled = false;
            hasApple = false;
        }
    }
    void Interacting()
    {
        //While holding button
        if (Input.GetKey(KeyCode.Space))
        {
            if(!answer)
            {
                interact = true;
            }
        }else
        {
            interact = false;
            answer = false;
        }
    }
    void Moving()
    {
        if (rig.velocity.magnitude < MaxSpeed)
        {
            Vector3 direction = Vector3.zero;
            //Forward or back
            if (Input.GetKey(KeyCode.W))
            {
                direction += Vector3.forward;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                direction += Vector3.back;
            }
            //Left or right
            if (Input.GetKey(KeyCode.A))
            {
                direction += Vector3.left;
                AnimationObject.transform.rotation = Quaternion.Lerp(AnimationObject.transform.rotation,Quaternion.Euler(0, 180, 0),0.5f);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                direction += Vector3.right;
                AnimationObject.transform.rotation = Quaternion.Lerp(AnimationObject.transform.rotation, Quaternion.Euler(0, 0, 0), 0.5f);
            }
            //Prevent diagonal acceleration
            if (direction.magnitude > 1)
            {
                direction *= 0.75f;
            }
            if(direction!=Vector3.zero)
            {
                rig.AddForce(direction * speed, ForceMode.Force);
            }
        }
        if (rig.velocity.magnitude > 2)
        {
            anim.SetBool("isWalking", true);
            anim.SetBool("isCold", false);
        }
        else
        {
            if (AnimationObject.transform.rotation.eulerAngles.y >= 270 && AnimationObject.transform.rotation.eulerAngles.y < 350)
            {
                AnimationObject.transform.rotation = Quaternion.Lerp(AnimationObject.transform.rotation, Quaternion.Euler(0, 0, 0), 0.5f);
            }
            else if (AnimationObject.transform.rotation.eulerAngles.y > 181 && AnimationObject.transform.rotation.eulerAngles.y <= 270)
            {
                AnimationObject.transform.rotation = Quaternion.Lerp(AnimationObject.transform.rotation, Quaternion.Euler(0, 180, 0), 0.5f);
            }
            anim.SetBool("isWalking", false);
            if(!GC.isDay)
            {
                anim.SetBool("isCold", true);
            }
            else
            {
                anim.SetBool("isCold", false);
            }
        }
    }
    void GoodEndInitiate()
    {
        GC.ToGoodEnd();
    }
    //animation over
    void OfferingOver()
    {
        isStop = false;
        answer = false;
        anim.SetBool("isOffering", false);
        anim.SetBool("isSelling", false);
        anim.SetBool("isFrightening", false);
        if (isFamished)
        {
            FamishedIMG.transform.localPosition = FamishedIMGStartPosition;
        }
        else
        {
            FamishedIMG.transform.localPosition = Vector3.down*1000;
        }
        CameraPosition.localPosition = DefaultCamPos;
    }
    //Shrink vector to one lenght
    Vector3 BringUpToShort(Vector3 vecStart, Vector3 vecFinish)
    {
        //Pythagorean theorem
        float k = Mathf.Sqrt(Mathf.Pow(vecFinish.x - vecStart.x, 2) + Mathf.Pow(vecFinish.z - vecStart.z, 2));
        return (vecFinish - vecStart) / k;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!GC.isDay&&other.tag == "Fire" && other.GetComponent<FireBarrel>().isFire)
        {
            isWarm = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag=="Fire"&&other.GetComponent<FireBarrel>().isFire)
        {
            isWarm = true;
        }
        else if(!GC.isDay)
        {
            isWarm = false;
        }
        if(GC.isDay&&!isStop&&other.tag=="Dog"&&!other.GetComponent<Dog>().isAttacked)
        {
            isStop = true;
            anim.SetBool("isFrightening", true);
            CameraPosition.localPosition = AnimationCamPos;
            FamishedIMG.transform.localPosition = Vector3.down * 1000;
            Sounds[7].Play();
            //Turn to Dog face to face
            if (BringUpToShort(this.transform.position, other.transform.position).x > 0)
            {
                AnimationObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                AnimationObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        if(interact&&!isStop)
        {
            if(GC.isDay && other.tag == "Stranger" && other.GetComponent<Stranger>().isThink)
            {
                Sounds[6].Play();
                isStop = true;
                anim.SetBool("isOffering", true);
                CameraPosition.localPosition = AnimationCamPos;
                FamishedIMG.transform.localPosition = Vector3.down * 1000;
                //Turn to Stranger face to face
                if (BringUpToShort(this.transform.position, other.transform.position).x > 0)
                {
                    AnimationObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    AnimationObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                if (other.GetComponent<Stranger>().isBuy)
                {
                    anim.SetBool("isSelling", true);
                    CameraPosition.localPosition = AnimationCamPos;
                }
            }
            if(other.tag=="Firewood"&&Inventory==0)
            {
                Destroy(other.gameObject);
                Inventory = 1;
                GC.FuelIMG[0].enabled = true;
                GC.FirewoodCount--;
                GC.CanvasAnim.SetBool("ShakeFuel", true);
                Sounds[2].Play();
            }
            if (other.tag == "Store")
            {
                if(other.name=="Charcoal"&&GC.Bits>0&&Inventory==0)
                {
                    if(GC.isEasy)
                    {
                        if(GC.Bits > 1)
                        {
                            Inventory = 2;
                            GC.FuelIMG[1].enabled = true;
                            GC.BitsIMG[GC.Bits - 1].enabled = false;
                            GC.BitsIMG[GC.Bits - 2].enabled = false;
                            GC.Bits -= 2;
                            Sounds[1].Play();
                            GC.CanvasAnim.SetBool("ShakeFuel", true);
                        }
                    }
                    else
                    {
                        Inventory = 2;
                        GC.FuelIMG[1].enabled = true;
                        GC.BitsIMG[GC.Bits - 1].enabled = false;
                        GC.Bits -= 1;
                        Sounds[1].Play();
                        GC.CanvasAnim.SetBool("ShakeFuel", true);
                    }
                }
                if (!buyApple&&!hasApple&&other.name == "Apple" && GC.Bits > 0)
                {
                    buyApple = true;
                    StartCoroutine(AppleCoolDown());
                    GC.BitsIMG[GC.Bits - 1].enabled = false;
                    GC.Bits -= 1;
                    hasApple = true;
                    Sounds[3].Play();//take apple (bad sound)
                    if (isFamished)
                    {
                        ToFamished(true);
                    }
                    else
                    {
                        GC.AppleIMG.enabled = true;
                        GC.CanvasAnim.SetBool("ShakeApple", true);
                    }
                }
            }
        }
    }
    IEnumerator AppleCoolDown()
    {
        while (buyApple)
        {
            yield return new WaitForSeconds(1.5f);
            buyApple = false;
        }
    }
}
