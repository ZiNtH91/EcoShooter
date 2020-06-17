using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPlant : MonoBehaviour
{

    [SerializeField]
    private int goldAmount = 20;
    public GameObject explostion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (goldAmount <= 0)
        {
            Instantiate(explostion, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }

    public int EatGold()
    {
        goldAmount -= 1;
        return 1;
    }

    public int HarvestGold()
    {
        int availableGold = goldAmount;
        goldAmount -= availableGold;
        return availableGold;
    }

}
