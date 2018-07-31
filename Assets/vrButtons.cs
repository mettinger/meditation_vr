//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;


using UnityEngine;
using System;


public class vrButtons : MonoBehaviour
{
    
    //public TextMesh displayText;

    public GameObject spherePrefab;

    // central array
    static private int rows = 20;
    static private int cols = 20;
    private float zSphereArray = 100;
    private GameObject[,] arraySphere = new GameObject[rows,cols];
    static private int numSphereArray = rows * cols;

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

    // event variables
    private int appRecoveryCount = 0;
    private float appEventProb = .1f;
    private int appEventDuration = 10;

    private int clickRecoveryCount = 0;
    private float clickEventProb = .1f;
    private int clickEventDuration = 10;

    private int updateCounter = 0;

    private bool appEventReadyFlag = true;
    private int appEventBeginCount = 0;
    private bool appInEventFlag = false;

    private bool clickEventReadyFlag = true;
    private int clickEventBeginCount = 0;
    private bool clickInEventFlag = false;

    private int appEventDetectCounter = 0;
    private int clickEventDetectCounter = 0;
    private int appFalsePositiveCounter = 0;
    private int clickFalsePositiveCounter = 0;

    private int appEventDetectionWindow = 2;
    private int clickEventDetectionWindow = 2;

    private bool appInWindowFlag = false;
    private bool clickInWindowFlag = false;

    // Use this for initialization
    void Start()
    {
        // MAKE CENTER ATTENTION ARRAY
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                arraySphere[i, j] = Instantiate(spherePrefab,
                                               new Vector3((float)j - (((float)cols - 1.0f) / 2.0f),
                                                           (float)i - (((float)rows - 1.0f) / 2.0f),
                                                           zSphereArray),
                                               new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));

                /*
                Renderer thisRend = arraySphere[i, j].GetComponent<Renderer>();
                Vector4 oldColor = thisRend.material.color;
                thisRend.material.color = new Color((float)Math.Sin( ((float)i)/rows), oldColor.y, oldColor.z, oldColor.w);
                */

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
        appEventLogic();
        clickEventLogic();

        appButtonLogic();
        clickButtonLogic();

        updateCounter += 1;


        /*
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
            */
    }


    void doAppEvent()
    {
        if (appEventBeginCount == updateCounter)
        {
            arraySphere[10, 10].transform.localScale = new Vector3(0, 0, 0);
        }
        else if (updateCounter >= appEventBeginCount + appEventDuration)
        {
            appInEventFlag = false;
            arraySphere[10, 10].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void doClickEvent()
    {
        if (clickEventBeginCount == updateCounter)
        {
            circleSphere[0].transform.localScale = new Vector3(0, 0, 0);
        }
        else if (updateCounter >= clickEventBeginCount + clickEventDuration)
        {
            clickInEventFlag = false;
            circleSphere[0].transform.localScale = new Vector3(1, 1, 1);
        }
    }


    void appEventLogic()
    {
        if (appEventReadyFlag && UnityEngine.Random.Range(0.0f, 1.0f) < appEventProb)
        {
            appEventReadyFlag = false;
            appEventBeginCount = updateCounter;
            appInEventFlag = true;
            appInWindowFlag = true;
        }

        if (appInEventFlag)
        {
            doAppEvent();
        }

        // are we ready for another event?
        if (!appEventReadyFlag && updateCounter >= appEventBeginCount + appRecoveryCount)
        {
            appEventReadyFlag = true;
        }

        // have we passed the event detection window?
        if (appInWindowFlag && updateCounter >= appEventBeginCount + appEventDetectionWindow)
        {
            appInWindowFlag = false;
        }
    }

    void clickEventLogic()
    {
        // CLICK EVENT LOGIC
        if (clickEventReadyFlag && UnityEngine.Random.Range(0.0f, 1.0f) < clickEventProb)
        {
            clickEventReadyFlag = false; 
            clickEventBeginCount = updateCounter;
            clickInEventFlag = true;
            clickInWindowFlag = true;
        }

        if (clickInEventFlag)
        {
            doClickEvent();
        }

        if (!clickEventReadyFlag && updateCounter >= clickEventBeginCount + clickEventDuration + clickRecoveryCount)
        {
            clickEventReadyFlag = true;
        }

        if (clickInWindowFlag && updateCounter >= clickEventBeginCount + clickEventDetectionWindow)
        {
            clickInWindowFlag = false;
        }
    }

    void appButtonLogic()
    {
        // DETECT APP BUTTON CLICK
        if (GvrControllerInput.AppButtonDown)
        {
            appButtonDownFlag = 1;
        }
        if (GvrControllerInput.AppButtonUp)
        {
            if (appButtonDownFlag == 1)
            {
                appButtonDownFlag = 0;
                if (appInWindowFlag)
                {
                    appEventDetectCounter += 1;
                }
                else
                {
                    appFalsePositiveCounter += 1;
                }
            }
        }
    }

    void clickButtonLogic()
    {
        // DETECT CLICK BUTTON CLICK
        if (GvrControllerInput.ClickButtonDown)
        {
            clickButtonDownFlag = 1;
        }
        if (GvrControllerInput.ClickButtonUp)
        {
            //displayText.text = "App button up";

            if (clickButtonDownFlag == 1)
            {
                clickButtonDownFlag = 0;
                if (clickInWindowFlag)
                {
                    clickEventDetectCounter += 1;
                }
                else
                {
                    clickFalsePositiveCounter += 1;
                }

                //clickClickCounter += 1;
                //print("click button click counter: " + clickClickCounter.ToString());
            }
        }
    }


}
