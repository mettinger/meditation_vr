using System.Collections;
using System.Collections.Generic;
//using UnityEngine.UI;


using UnityEngine;


public class vrButtons : MonoBehaviour
{
    
    //public TextMesh displayText;

    public GameObject spherePrefab;
    static private int rows = 10;
    static private int cols = 10;
    private float zSphereArray = 30;
    private GameObject[,] newSphere = new GameObject[rows,cols];

    // Use this for initialization
    void Start()
    {
        /*
        for (int i = 0; i < 2; i++)
        {
            newSphere[i] = Instantiate(spherePrefab, new Vector3(0.0f + (float)i, 1.0f, 5.0f), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        }*/

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                newSphere[i,j] = Instantiate(spherePrefab, new Vector3((float)j - (((float)cols - 1.0f)/2.0f), (float)i - (((float)rows - 1.0f) / 2.0f), zSphereArray), new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            }
           
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        if (GvrControllerInput.AppButtonDown)
        {
            print("Click App button down");
            //displayText.text = "App button down";
        }
        if (GvrControllerInput.AppButtonUp)
        {
            print("Click App button up");
            //displayText.text = "App button up";
        }
    }

}
