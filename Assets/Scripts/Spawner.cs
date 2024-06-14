using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> Character;//prefab
    public float Frequency = 4;
    GameChanger GC;
    void Start()
    {
        GC = Camera.main.GetComponent<GameChanger>();//on camera
        StartCoroutine(Timer());
    }
    void Update()
    {
        
    }
    private IEnumerator Timer()
    {
        while (true)
        {
            if(GC.isDay&&GC.GameOn&&GC.TimeNow>15)
            {
                //chance TimeNow per Half-TimeRound
                float R = GC.RoundTime / 2 - Mathf.Abs(GC.RoundTime / 2 - GC.TimeNow);
                R = R * 100 / GC.RoundTime / 2;//by percent
                if(Random.Range(0,100f)<=R+15)
                {
                    Instantiate(Character[Random.Range(0, Character.Capacity)], transform.position, Quaternion.identity);
                }
            }
            yield return new WaitForSeconds(Frequency);
        }
    }
}
