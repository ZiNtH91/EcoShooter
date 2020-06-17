using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField]
    private Sprite[] treeSprites;
    private int ownSprite;

    private SpriteRenderer srenderer;

    // Start is called before the first frame update
    void Start()
    {
        srenderer = GetComponent<SpriteRenderer>();
        ownSprite = Random.Range(0, treeSprites.Length);
        srenderer.sprite = treeSprites[ownSprite];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
