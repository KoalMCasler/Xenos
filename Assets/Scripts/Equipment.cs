using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public enum equipType{Matirial, Engine, Wing, Booster}
    public equipType type;
    public string equipmentName;
    public float cost;
    public string discription;
    public float modValue;
    public Sprite image;
    
    // Start is called before the first frame update
    void Start()
    {
        if(image != null)
        {
            this.GetComponent<Image>().sprite = image;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
