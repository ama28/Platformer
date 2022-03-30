using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChange : MonoBehaviour
{
    public Animator anim;
    public string levelToLoad;
    public bool finishTransition = false;
    public bool transition = false;
    public GameObject postProcessVolume, player;
    public float rewindTime, resumeTime;
    private Vector3 playerLocation;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerLocation = player.transform.position;
    }
    private void Update()
    {
        if (transition && !finishTransition)
        {

            //postProcessVolume.GetComponent<Animator>().SetTrigger("REWIND");
            StartCoroutine(TransitionStart());
            finishTransition = true;
        }

    }

    public void ToBoss()
    {
        SceneManager.LoadScene("Zun Scene");
    }

    IEnumerator ParticlesWithDelay()
    {
        yield return new WaitForSeconds(.4f);
        player.GetComponentInChildren<ParticleSystem>().Play();
    }

    private IEnumerator TransitionStart()
    {
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
        for (int i = 0; i < 10; i++) {

            //pop out in random locations
            float x = Random.Range(-3f, 3f);
            float y = 0;

            player.transform.position = playerLocation+ new Vector3(x, y, 0);

            StartCoroutine(ParticlesWithDelay());
            postProcessVolume.GetComponent<Animator>().SetTrigger("REWIND");
            yield return new WaitForSeconds(rewindTime);
            postProcessVolume.GetComponent<Animator>().SetTrigger("RESUME");
            yield return new WaitForSeconds(resumeTime);

            if(i == 8)
                FadeToLevel();
        }
        
       // FadeToLevel();
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
