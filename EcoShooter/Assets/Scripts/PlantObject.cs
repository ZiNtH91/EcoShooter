using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantObject : ScriptableObject
{

    GameObject[] stages;
    float[] stageTransitions;

    float age;
    int stage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageStage();
    }

    private void ManageStage()
    {
        if (stage < stageTransitions.Length)
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
        stages[stage].SetActive(true);
        stages[stage - 1].SetActive(false);
    }
}
