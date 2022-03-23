using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public Animator anim;
    public int levelToLoad;
    public bool transition = false;

    private void Update()
    {
        if (transition)
            FadeToLevel();
    }

    public void FadeToLevel()
    {
        anim.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
