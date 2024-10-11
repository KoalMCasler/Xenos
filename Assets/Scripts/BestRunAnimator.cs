using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestRunAnimator : MonoBehaviour
{
    public Animator textAnim;
    public float value;
    private bool reversed;

    // Update is called once per frame
    void Update()
    {
        if(reversed)
        {
            value -= Time.deltaTime;
        }
        else
        {
            value += Time.deltaTime;
        }
        if(value > 1)
        {
            reversed = true;
        }
        if(value < 0)
        {
            reversed = false;
        }
        textAnim.SetFloat("Value",value);
    }
}
