using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameChanger : MonoBehaviour
{
    public Text EasyModeTxt;
    public Image CharcoalDiscount;
    public Text GoToMenuL;
    public List<Image> Moon;
    public GameObject Pause;
    public List<AudioSource> Sounds;
    public Image EndIMG,GoodEndIMG;
    public Image EndWhite, EndBlack;
    public Animator TaxiAnim;
    public Animator CanvasAnim;
    public Text Timertxt;
    public List<Image> MatchesIMG;
    public List<Image> BitsIMG;
    public Image AppleIMG;
    public List<Image> FuelIMG;//0-firewood, 1-charcoal
    public Slider Temperatureslider;
    public Transform HourArrow;//on clock
    public List<Transform> FirewoodPoints;
    public GameObject Firewood;
    public Light Sun;
    public float RoundTime = 60;
    public float TimeNow;
    public int AllMatches = 20;
    public int Bits = 0;
    public int FirewoodCount = 0;//on ground
    public bool isDay = true;
    public SpriteRenderer IdleIMG, FreezeIMG;
    float HourStep;
    float EndTime = 0;
    PlayerCtrl Player;
    public float Temperature = 15;
    public Transform F2, F3, F4;
    public Image WhiteScreen;
    public bool GameOn = true;
    bool AllowToFinish=false;
    bool StartWhite = true;
    bool EndtWhite = false;
    public int Days = 0;
    bool isGoodEnd = false;
    public bool isEasy;
    private void Awake()
    {
        TimeNow = RoundTime;
        HourStep = 360 / RoundTime;//360 degree to round arrow
    }
    void Start()
    {
        MainMenu M = GameObject.Find("MainMenu").GetComponent<MainMenu>();
        Sounds[0].enabled = M.MusicEnable;
        Sounds[1].enabled = M.MusicEnable;
        Sounds[2].enabled = M.MusicEnable;
        Sounds[3].enabled = M.MusicEnable;
        if(M.isEng)
        {
            GoToMenuL.text = "Back to menu";
        }
        isEasy=M.isEasy;
        Destroy(GameObject.Find("MainMenu"));
        StartCoroutine(Timer());
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCtrl>();
        if(isEasy)
        {
            Days++;
            CharcoalDiscount.enabled = false;
        }
    }
    private void Update()
    {
        if(AllowToFinish&&Input.anyKeyDown)
        {
            BackToMenu();
        }
        if (GameOn&&Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                AudioListener.pause = false;
                Pause.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                AudioListener.pause = true;
                Pause.SetActive(true);
            }

        }
    }
    private void FixedUpdate()
    {
        if (StartWhite)
        {
            Color C = WhiteScreen.color;
            C.a = Mathf.Lerp(C.a, 0, 0.05f);
            WhiteScreen.color = C;
            if (C.a < 0.0001f)
            {
                StartWhite = false;
            }
        }
        if (EndtWhite)
        {
            Color C = WhiteScreen.color;
            C.a = Mathf.Lerp(C.a, 1.5f, 0.01f);
            WhiteScreen.color = C;
            if (C.a >0.999f)
            {
                StartWhite = true;
                EndtWhite = false;
                GoodEndIMG.enabled = true;
                EndBlack.enabled = true;
                StartCoroutine(TimeToAllowFinish());
                if(isEasy)
                {
                    if (GoToMenuL.text == "Back to menu")
                    {
                        EasyModeTxt.text = "Easy mode";
                    }
                    else
                    {
                        EasyModeTxt.text = "Лёгкий режим";
                    }
                }
            }
        }
        if (GameOn)
        {
            if (isDay)
            {
                Sun.intensity = Mathf.Clamp(Sun.intensity + 0.01f, 0, 1);
                this.GetComponent<AudioSource>().volume = 0.2f;
                Sounds[0].mute = false;
                Sounds[1].mute = true;
            }
            else
            {
                Sounds[1].mute = false;
                Sounds[0].mute = true;
                this.GetComponent<AudioSource>().volume = 0.5f;
                Sun.intensity = Mathf.Clamp(Sun.intensity - 0.01f, 0, 1);
                if (!FreezeIMG.gameObject.activeInHierarchy && !Player.isStop)
                {
                    IdleIMG.gameObject.SetActive(isDay);
                    FreezeIMG.gameObject.SetActive(!isDay);
                }
            }
            FreezingVignette();
            TaxiAnim.SetBool("isDay", isDay);
        }
        else//game over
        {
            if(!isGoodEnd)
            {
                Color C = WhiteScreen.color;
                C.a = Mathf.Lerp(C.a, 0, 0.01f);
                WhiteScreen.color = C;
            }
        }
    }
    public void BackToMenu()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        Pause.SetActive(false);
        SceneManager.LoadScene(0);
    }
    void SetFirewood ()
    {
        //3 is limit
        if(FirewoodCount < 3)
        {
            int i = Random.Range(0, FirewoodPoints.Capacity);
            Instantiate(Firewood, FirewoodPoints[i].position, Quaternion.Euler(90,Random.Range(0,360),0));
            FirewoodCount++;
        }
    }
    void DestroyAllFirewood()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Firewood"))
        {
            Destroy(g);
        }
        FirewoodCount = 0;
    }
    void FreezingVignette()
    {
        Vector3 vec = F4.localScale;
        vec =Vector3.Lerp(vec, Vector3.one * (Temperature * 0.0215f + 0.65f), 0.01f);//0.65 is optimal size        
        F2.localScale = vec;
        F3.localScale = vec;
        F4.localScale = vec;
        Color C = WhiteScreen.color;
        C.a = Mathf.Lerp(C.a, EndTime / 5, 0.1f);
        WhiteScreen.color = C;
    }
    public void ToGoodEnd()
    {
        EndtWhite = true;
        Sounds[3].Play();
    }
    private IEnumerator TimeToGoodEnd()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
        }
    }
    private IEnumerator Timer()
    {
        while(GameOn)
        {
            yield return new WaitForSeconds(1);
            //end of round
            if(TimeNow==0)
            {
                isDay = !isDay;
                TimeNow = RoundTime;
                SetFirewood();
                Player.isWarm = isDay;
                Temperature = 15;
                if (!Player.isStop)
                {
                    IdleIMG.gameObject.SetActive(isDay);
                    FreezeIMG.gameObject.SetActive(!isDay);
                }
                if (isDay)
                {
                    Moon[Days].enabled = true;
                    DestroyAllFirewood();
                    Days++;
                    if (Days == 6)
                    {
                        GameOn = false;
                        isGoodEnd = true;
                        Player.isStop = true;
                        Player.anim.SetBool("isGoodEnd", true);
                        Player.anim.SetBool("isWalking", false);
                        Sounds[0].mute = true;
                        Sounds[1].mute = true;
                        Sounds[4].Play();
                        this.GetComponent<AudioSource>().mute = true;//Wind noise
                        F2.localScale = Vector3.one*0.9f;
                        F3.localScale = Vector3.one * 0.9f;
                        F4.localScale = Vector3.one*0.9f;
                        Color C = WhiteScreen.color;
                        C.a = 0;
                        WhiteScreen.color = C;
                    }
                    else
                    {
                        if (Player.hasApple)
                        {
                            Player.ToFamished(true);
                        }
                        else
                        {
                            Player.ToFamished();
                        }
                        Player.Inventory = 0;
                        FuelIMG[0].enabled = false;
                        FuelIMG[1].enabled = false;
                    }
                }
            }
            if(!isDay)//until night
            {
                Temperature += Player.isWarm ? 1 : -1.5f;
                Temperature = Temperature < 0 ? 0 : Temperature;
                Temperature = Temperature > 15 ? 15 : Temperature;
                //each 5 seconds
                if (TimeNow%5==0)
                {
                    SetFirewood();
                }
                if(EndTime==0&&Temperature==0)
                {
                    EndTime++;
                    StartCoroutine(TimeToEnd());
                }
            }
            TimeNow--;//+=isDay? -1:-2;//night is two times swifter
            Timertxt.text = ((int)TimeNow).ToString();
            HourArrow.Rotate(Vector3.back * HourStep);//)*(isDay ? 1 : 2));//by z axis
            Temperatureslider.value = Temperature;
        }
    }
    private IEnumerator TimeToEnd()
    {
        while(Temperature==0)
        {
            yield return new WaitForSeconds(1);
            if (GameOn&&EndTime > 8)
            {
                Player.anim.SetBool("isWalking", false);
                GameOn = false;
                isDay = false;
                Sounds[0].mute = true;
                Sounds[1].mute = true;
                Sounds[2].Play();//End music
                //Disable UI
                AppleIMG.enabled = false;
                FuelIMG[0].enabled = false;
                FuelIMG[1].enabled = false;
                BitsIMG[0].enabled = false;
                BitsIMG[1].enabled = false;
                BitsIMG[2].enabled = false;
                BitsIMG[3].enabled = false;
                foreach (Image img in MatchesIMG)
                {
                    img.enabled = false;
                }
                this.GetComponent<AudioSource>().mute = true;//Wind noise
                EndIMG.enabled = true;
                EndWhite.enabled = true;
                Temperature = -1;
                StartCoroutine(TimeToAllowFinish());
            }
            EndTime++;
        }
        EndTime = 0;
    }
    private IEnumerator TimeToAllowFinish()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            AllowToFinish = true;
        }
    }
}
