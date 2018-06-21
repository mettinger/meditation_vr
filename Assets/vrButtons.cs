//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;


using UnityEngine;


public class vrButtons : MonoBehaviour
{
    
    //public TextMesh displayText;

    public GameObject spherePrefab;

    // central array
    static private int rows = 20;
    static private int cols = 20;
    private float zSphereArray = 100;
    private GameObject[,] arraySphere = new GameObject[rows,cols];

    // peripheral circle
    static private int numSpherePeriph = 30;
    private float circleRadius = 10;
    private float zCircle = 15;
    private GameObject[] circleSphere = new GameObject[numSpherePeriph];

    // button click counters
    private int appClickCounter = 0;
    private int clickClickCounter = 0;

    // button status flags
    private int clickButtonDownFlag = 0;
    private int appButtonDownFlag = 0;

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

        // MAKE PERIPHERAL SPHERES
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
        // DETECT APP BUTTON CLICK
        if (GvrControllerInput.AppButtonDown)
        {
            appButtonDownFlag = 1;
        }
        if (GvrControllerInput.AppButtonUp)
        {
            //displayText.text = "App button up";

            if (appButtonDownFlag == 1)
            {
                appClickCounter += 1;
                appButtonDownFlag = 0;
                print("app button click counter: " + appClickCounter.ToString());
            }
        }

        // DETECT CLICK BUTTON CLICK
        if (GvrControllerInput.ClickButtonDown)
        {
            clickButtonDownFlag = 1;
        }
        if (GvrControllerInput.ClickButtonDown)
        {
            //displayText.text = "App button up";

            if (clickButtonDownFlag == 1)
            {
                clickClickCounter += 1;
                clickButtonDownFlag = 0;
                print("click button click counter: " + clickClickCounter.ToString());
            }
        }

        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
    }

}
