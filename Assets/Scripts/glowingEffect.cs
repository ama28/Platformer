using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class glowingEffect : MonoBehaviour
{
    public float changeRate;
    public float maxIntensity,minIntensity;
    private Light2D myLight;
    private bool scaleup;
    // Start is called before the first frame update
    void Start()
    {
        myLight = gameObject.GetComponent<Light2D>();
        scaleup = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(scaleup)
            myLight.intensity = Mathf.MoveTowards(myLight.intensity, maxIntensity, changeRate * Time.deltaTime);
        else
            myLight.intensity = Mathf.MoveTowards(myLight.intensity, minIntensity, changeRate * Time.deltaTime);

        if (myLight.intensity <= minIntensity)
            scaleup = true;

        if (myLight.intensity >= maxIntensity)
            scaleup = false;
    }
}
