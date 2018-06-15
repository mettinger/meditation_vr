//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;


using UnityEngine;


public class vrButtons : MonoBehaviour
{
    
    //public TextMesh displayText;

    public GameObject spherePrefab;

    // central array
    static private int rows = 10;
    static private int cols = 10;
    private float zSphereArray = 100;
    private GameObject[,] arraySphere = new GameObject[rows,cols];

    // peripheral circle
    static private int numSpherePeriph = 30;
    private float circleRadius = 10;
    private float zCircle = 15;
    private GameObject[] circleSphere = new GameObject[numSpherePeriph];

    // Use this for initialization
    void Start()
    {
        // MAKE CENTER ATTENTION ARRAY
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arraySphere[i,j] = Instantiate(spherePrefab, 
                                               new Vector3((float)j - (((float)cols - 1.0f)/2.0f),
                                                           (float)i - (((float)rows - 1.0f)/2.0f),
                                                           zSphereArray), 
                                               new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            }
           
        }

        // MAKE PERIPHERAL
        for (int i = 0; i < numSpherePeriph; i++)
        {
            
            circleSphere[i] = Instantiate(spherePrefab, 
                                          new Vector3(circleRadius * (float)System.Math.Cos(2*System.Math.PI*i/numSpherePeriph), 
                                                      circleRadius * (float)System.Math.Sin(2*System.Math.PI*i/numSpherePeriph), 
                                                      zCircle), 
                                          new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
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
