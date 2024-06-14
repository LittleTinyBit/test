using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public AudioSource Barking;
    public float MaxSpeed = 6.5f;
    Rigidbody rig;
    Animator anim;
    GameChanger GC;
    bool isMainGoal = false;
    bool isEnd = false;//come to point
    bool isStop = false;
    public bool isAttacked = false;
    Vector3 MainDirection;
    float speed = 100;
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
        if (!isEnd)
        {
            isNearGoal();
        }
    }
    void FixedUpdate()
    {
        if (!isStop)
        {
            MoveTo(MainDirection);
            anim.SetBool("isWalking", true);
            this.GetComponent<AudioSource>().mute=false;
        }
        else
        {
            anim.SetBool("isWalking", false);
            this.GetComponent<AudioSource>().mute = true;
        }
        if (!isMainGoal)
        {
            DirectionToGoal();//go to middle point
        }
        else
        {
            if (!isEnd)
            {
                DirectionToGoal(true);//go to main point
            }
            //else will go forwar till the end
        }
    }
    //Shrink vector to one lenght
    Vector3 BringUpToShort(Vector3 vecStart, Vector3 vecFinish)
    {
        //Pythagorean theorem
        float k = Mathf.Sqrt(Mathf.Pow(vecFinish.x - vecStart.x, 2) + Mathf.Pow(vecFinish.z - vecStart.z, 2));
        return (vecFinish - vecStart) / k;
    }
    void DirectionToGoal(bool isMainGoal = false)
    {
        Vector3 goal;
        if (!isMainGoal)
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
    void MoveTo(Vector3 direction)
    {
        if (rig.velocity.magnitude < MaxSpeed)
        {
            rig.AddForce(direction * speed, ForceMode.Force);
            if (rig.velocity.x > 0)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 180, 0), 0.5f);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, 0), 0.5f);
            }
        }
    }
    void isNearGoal()
    {
        float proximity = 1;
        if (!isMainGoal)
        {
            float distance = Vector3.Distance(MiddleGoals[MiddleGoalID].Position, transform.position);
            if (distance < proximity)
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
    void FillGoalsList()
    {
        GameObject GO = GameObject.Find("MiddlePoints");
        foreach (Transform child in GO.transform)
        {
            MiddleGoals.Add(child.GetComponent<GoalPoint>());
        }
        GO = GameObject.Find("MainPoints");
        foreach (Transform child in GO.transform)
        {
            MainGoals.Add(child.GetComponent<GoalPoint>());
        }
    }
    //Animation event
    public void AnimationOver()
    {
        isStop = false;
        anim.SetBool("isAttacking", false);
        if(GC.isEasy)
        {
            if(Random.Range(0, 2) == 1)
            {
                if (GC.Bits > 0)
                {
                    GC.BitsIMG[--GC.Bits].enabled = false;
                    GC.CanvasAnim.SetBool("ShakeBit", true);
                }
                else
                {
                    GC.MatchesIMG[--GC.AllMatches].enabled = false;
                    GC.CanvasAnim.SetBool("ShakeMatches", true);
                }
            }
        }
        else
        {
            if (GC.Bits > 0)
            {
                GC.BitsIMG[--GC.Bits].enabled = false;
                GC.CanvasAnim.SetBool("ShakeBit", true);
            }
            else
            {
                GC.MatchesIMG[--GC.AllMatches].enabled = false;
                GC.CanvasAnim.SetBool("ShakeMatches", true);
            }
        }
        isAttacked = true;
    }
    private void OnTriggerStay(Collider other)
    {
        if (GC.isDay&&!isAttacked&&other.tag == "Player" && !other.GetComponent<PlayerCtrl>().isStop)
        {
            //Turn to Match Filly face to face
            if (BringUpToShort(this.transform.position, other.transform.position).x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            isStop = true;
            anim.SetBool("isAttacking", true);
            Barking.Play();
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
