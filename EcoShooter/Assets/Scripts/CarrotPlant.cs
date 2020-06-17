using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrotPlant : MonoBehaviour
{
    [SerializeField]
    private int carrotCount = 10;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (carrotCount <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }

    public int EatCarrot()
    {
        carrotCount -= 1;
        return 1;
    }

    public int HarvestCarrot()
    {
        int availableCarrots = carrotCount;
        carrotCount -= availableCarrots;
        return availableCarrots;
    }

}
