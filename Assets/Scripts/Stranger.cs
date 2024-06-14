using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stranger : MonoBehaviour
{
    public List<AudioSource> Sounds;
    public float MaxSpeed = 5.5f; 
    Rigidbody rig;
    Animator anim;
    Vector3 MainDirection;
    float speed = 100;
    public bool isThink=false;
    GameChanger GC;
    bool isDiced = false;//was chance diced
    bool isMainGoal = false;
    bool isEnd = false;//come to point
    bool isStop = false;
    public bool isBuy = false;//accept offering
    bool isAlreadyBuyed = false;
    //list of goals
    List<GoalPoint> MainGoals = new List<GoalPoint>();
    List<GoalPoint> MiddleGoals = new List<GoalPoint>();
    //Indexes of goals
    int MainGoalID;
    int MiddleGoalID;
    void Start()
    {
        GC = Camera.main.GetComponent<GameChanger>();//on main camera
        rig = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        FillGoalsList();
        MiddleGoalID = Random.Range(0, MiddleGoals.Count);
        MainGoalID = Random.Range(0, MainGoals.Count);
    }
    private void Update()
    {
        //if achieved goal then go forward to end of the map 
        if(!isEnd)
        {
            //Avoid goal if Player there
            if (!isMainGoal)
            {
                if (MiddleGoals[MiddleGoalID].isPlayer)
                {
                    ChangeGoal();
                }
            }
            else
            {
                if (MainGoals[MainGoalID].isPlayer)
                {
                    ChangeGoal(true);
                }
            }
            isNearGoal();
        }
    }
    void FixedUpdate()
    {
        if(!isStop)
        {
            MoveTo(MainDirection);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
        if (!isMainGoal)
        {
            DirectionToGoal();//go to middle point
        }else
        {
            if(!isEnd)
            {
                DirectionToGoal(true);//go to main point
            }
            //else will go forwar till the end
        }
    }
    void isNearGoal()
    {
        float proximity = 1;
        if (!isMainGoal)
        {
            float distance = Vector3.Distance(MiddleGoals[MiddleGoalID].Position, transform.position);
            if(distance<proximity)
            {
                //change direction to main point
                isMainGoal = true;
            }
        }
        else
        {
            float distance = Vector3.Distance(MainGoals[MainGoalID].Position, transform.position);
            if (distance < proximity)
            {
                //go forward to end
                isEnd = true;
                MainDirection = Vector3.forward;
                StartCoroutine(TimerToDestroy());
            }
        }
    }
    void ChangeGoal(bool isMainGoal=false)
    {
        if(!isMainGoal)
        {
            //not same
            int newID = MiddleGoalID;
            while(newID==MiddleGoalID)
            {
                newID= Random.Range(0, MiddleGoals.Count);
            }
            MiddleGoalID = newID;
        }else
        {
            //not same
            int newID = MainGoalID;
            while (newID == MainGoalID)
            {
                newID = Random.Range(0, MainGoals.Count);
            }
            MainGoalID = newID;
        }
    }
    void FillGoalsList()
    {
        GameObject GO = GameObject.Find("MiddlePoints");
        foreach(Transform child in GO.transform)
        {
            MiddleGoals.Add(child.GetComponent<GoalPoint>());
        }
        GO = GameObject.Find("MainPoints");
        foreach (Transform child in GO.transform)
        {
            MainGoals.Add(child.GetComponent<GoalPoint>());
        }
    }
    void MoveTo(Vector3 direction)
    {
        if (rig.velocity.magnitude < MaxSpeed)
        {
            rig.AddForce(direction * speed, ForceMode.Force);
            if(rig.velocity.x>0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.5f);
            }else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), 0.5f);
            }
        }
    }
    void DirectionToGoal(bool isMainGoal=false)
    {
        Vector3 goal;
        if(!isMainGoal)
        {
            goal = MiddleGoals[MiddleGoalID].Position;
        }
        else
        {
            goal = MainGoals[MainGoalID].Position;
        }
        //y has be same
        goal.y = transform.position.y;
        MainDirection = BringUpToShort(transform.position, goal);
    }
    //Shrink vector to one lenght
    Vector3 BringUpToShort(Vector3 vecStart,Vector3 vecFinish)
    {
        //Pythagorean theorem
        float k = Mathf.Sqrt(Mathf.Pow(vecFinish.x - vecStart.x, 2) + Mathf.Pow(vecFinish.z - vecStart.z, 2));
        return (vecFinish - vecStart) / k;
    }
    public void AvoidPlayer(Vector3 PlayerPos)
    {
        if(!isStop)
        {
            //same height
            PlayerPos.y = transform.position.y;
            Vector3 direction = BringUpToShort(transform.position, PlayerPos);
            Debug.DrawLine(transform.position, transform.position + direction, Color.red);
            //If dot product positive that mean the player on the way
            float DotProd = Vector3.Dot(MainDirection, direction);
            //sharp angle
            if (DotProd > 0)
            {
                //Reflect main direction by player direction
                Vector2 vec2 = Vector2.Perpendicular(new Vector2(direction.x, direction.z));
                Vector3 vec3 = new Vector3(vec2.x, direction.y, vec2.y);
                //Add new vector to main direction
                direction = Vector3.Reflect(MainDirection, vec3) * (-DotProd * 2);//multiply on dot product to save lengh of vector
                MoveTo(direction + MainDirection);
                Debug.DrawLine(transform.position, transform.position + direction, Color.blue);
                Debug.DrawLine(transform.position, transform.position + direction + MainDirection, Color.green);
                Debug.DrawLine(transform.position, transform.position + MainDirection);

            }
        }
    }
    //Animation event
    public void AnimationOver()
    {
        isStop = false;
        isThink = false;//avoid replay player animation if one holding interact
        anim.SetBool("isThinking", false);
        anim.SetBool("isBuying", false);
        if(isBuy)
        {
            GC.Bits++;
            GC.BitsIMG[GC.Bits - 1].enabled = true;
            GC.MatchesIMG[--GC.AllMatches].enabled = false;
            GC.CanvasAnim.SetBool("ShakeBit", true);
            GC.CanvasAnim.SetBool("ShakeMatches", true);
            Sounds[0].Play();
        }
        isAlreadyBuyed = true;
    }
    public void PlaySoundAccept()
    {
        Sounds[2].Play();
    }
    public void PlaySoundRefuse()
    {
        Sounds[3].Play();
    }
    void Dicing(PlayerCtrl P)
    {
        if(!isDiced)
        {
            float Chance = (3 + (GC.isEasy ? 1 : 0) - GC.Bits-(P.hasApple?1:0)-((GC.isEasy?2:1)*(P.Inventory!=0?1:0))) * (33-(GC.isEasy?8:0));//every selled match reduce chance on 33%
            if (GC.AllMatches>0&&Random.Range(1, 101) <= Chance*0.75f)
            {
                isThink = true;
                Chance = (GC.RoundTime-GC.TimeNow)*100/ GC.RoundTime;//less time left then more chance
                int R = Random.Range(-25+(GC.isEasy?20:0) + GC.Days * 3, 26-GC.Days*5);
                //Debug.Log(Chance* Generosity+R);
                if (Random.Range(1, 101) <= Chance * (0.4f+(GC.isEasy?0.45f:0)) + R)
                {   
                    isBuy = true;
                }
            }
            isDiced = true;
            this.GetComponent<Interacrtable>().isInteractable = false;//not interactable anymore
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (GC.isDay && other.tag == "Player" &&!isAlreadyBuyed&& other.GetComponent<PlayerCtrl>().interact && !other.GetComponent<PlayerCtrl>().isStop)
        {
            if(!isDiced)
            {
                Dicing(other.GetComponent<PlayerCtrl>());
                if(isThink)
                {
                    other.GetComponent<PlayerCtrl>().answer = true;//that mean player is busy for dog
                    isStop = true;
                    anim.SetBool("isThinking", true);
                    Sounds[1].Play();
                    if (isBuy)
                    {
                        anim.SetBool("isBuying", true);
                    }
                }
                else
                {
                    PlaySoundRefuse();
                }
                isDiced = true;
            }
            if(isStop)
            {
                //Turn to Match Filly face to face
                if (BringUpToShort(this.transform.position, other.transform.position).x > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }
        }
    }
    private IEnumerator TimerToDestroy()
    {
        while (true)
        {
            yield return new WaitForSeconds(15);
            GameObject.Destroy(this.gameObject);
        }
    }
}
