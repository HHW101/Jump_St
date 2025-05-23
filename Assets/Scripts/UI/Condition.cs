using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Condition : MonoBehaviour
{
    public float nowValue;
    public float min;
    public float max;
    public Image bar;
    void Start()
    {
        nowValue = max;
    }
    private void Update()
    {
        bar.fillAmount = nowValue/max;
    }
    public void Add(float val)
    {
        nowValue += val;
        if (nowValue > max) nowValue = max;
        else if(nowValue < min) nowValue = min;

    }
    
    
}
