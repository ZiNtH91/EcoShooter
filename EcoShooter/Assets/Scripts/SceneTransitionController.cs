using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public static Animator sceneTransitionAnimator;
    public static string sceneToLoad;

    private void Start()
    {
        Cursor.visible = true;
        sceneTransitionAnimator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public static void LoadScene(string name)
    {
        sceneToLoad = name;
        sceneTransitionAnimator.SetTrigger("fadeToBlack");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
