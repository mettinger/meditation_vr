using UnityEngine;
using System;

public class changeColor : MonoBehaviour
{

    Renderer thisRend;
    float i;

    // Use this for initialization
    void Start()
    {

        thisRend = GetComponent<Renderer>();
        i = 0;

    }

    // Update is called once per frame
    void Update()
    {

        Vector4 oldColor = thisRend.material.color;
        i = i + .1f;
        //Color color = new Color((float)Math.Sin(i), oldColor.y, oldColor.z, oldColor.w);
        Color color = new Color((float)(oldColor.x + .001f) % 1.0f, oldColor.y, oldColor.z, oldColor.w);
        thisRend.material.SetColor("_Color", color);

    }
}
