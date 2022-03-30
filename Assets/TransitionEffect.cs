using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionEffect : MonoBehaviour
{
    public Animator anim;
    public string levelToLoad;
    public bool finishTransition = false;
    public bool transition = false;
    public GameObject postProcessVolume;

    private void Update()
    {
        if (transition && !finishTransition) {

            //postProcessVolume.GetComponent<Animator>().SetTrigger("REWIND");
            StartCoroutine(TransitionStart());
            finishTransition = true;
        }
         
    }

    private IEnumerator TransitionStart()
    {
        yield return new WaitForSeconds(3);
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
