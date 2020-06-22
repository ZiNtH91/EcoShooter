using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject[] plantStages;
    public float[] stageTransitions;
    private ParticleSystem ps_stageChange;

    private int stage = 0;
    private float age;

    public Sprite sprite;


    void Start()
    {
        ps_stageChange = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
        ManageStage();
    }

    private void ManageStage()
    {
        if(stage < stageTransitions.Length)
        {
            age += Time.deltaTime;

            if (age > stageTransitions[stage])
            {
                ChangeStage();
            }           
        }
    }

    private void ChangeStage()
    {
        stage += 1;
        plantStages[stage].SetActive(true);
        plantStages[stage - 1].SetActive(false);
        ps_stageChange.Play();
    }

}
