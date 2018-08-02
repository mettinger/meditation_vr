//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.UI;


using UnityEngine;
using System;
using UnityEngine.UI;


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

    // button status flags
    private int clickButtonDownFlag = 0;
    private int appButtonDownFlag = 0;

    // event variables - do NOT modify
    private bool appEventReadyFlag = true;
    private float appEventBeginSec;
    private bool appInEventFlag = false;

    private bool clickEventReadyFlag = true;
    private float clickEventBeginSec;
    private bool clickInEventFlag = false;

    private int appEventDetectCounter = 0;
    private int clickEventDetectCounter = 0;
    private int appFalsePositiveCounter = 0;
    private int clickFalsePositiveCounter = 0;

    private int appEventCounter = 0;
    private int clickEventCounter = 0;

    private bool appInWindowFlag = false;
    private bool clickInWindowFlag = false;

    private int randomPeripheralIndex;
    private int randomAttentionRow;
    private int randomAttentionColumn;

    public Text attentionEventCounterText;
    public Text attentionEventDetectedText;
    public Text attentionFalsePositiveText;

    public Text awarenessEventCounterText;
    public Text awarenessEventDetectedText;
    public Text awarenessFalsePositiveText;

    private int frameCounter = 0;

    // event parameters - modify at will!
    // click button is for peripheral
    // app button is for array (attention)
    private float appRecoverySecs = 4.0f; // recovery time after end of event
    private float appEventProb = .01f;
    private float appEventDurationSecs = 0.1f;
    private float appEventDetectionWindowSecs = 1.0f; // window for detection from beginning of event

    private float clickRecoverySecs = 4.0f; // recovery time after end of event
    private float clickEventProb = .01f;
    private float clickEventDurationSecs = 0.1f;
    private float clickEventDetectionWindowSecs = 1.0f; // window for detection from beginning of event


    // Use this for initialization
    void Start()
    {

        updateDashboard();

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

        updateDashboard();

        appEventLogic();
        clickEventLogic();

        appButtonLogic();
        clickButtonLogic();

        frameCounter += 1;

        /*
        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");
            */
    }


    void updateDashboard()
    {
        //attentionEventCounterText.text = "frame count: " + frameCounter.ToString();

        attentionEventCounterText.text = "attention events total: " + appEventCounter.ToString();
        attentionEventDetectedText.text = "attention events detected: " + appEventDetectCounter.ToString();
        attentionFalsePositiveText.text = "attention false positives: " + appFalsePositiveCounter.ToString();

        awarenessEventCounterText.text = "awareness events total: " + clickEventCounter.ToString();
        awarenessEventDetectedText.text = "awareness events detected: " + clickEventDetectCounter.ToString();
        awarenessFalsePositiveText.text = "awareness false positives: " + clickFalsePositiveCounter.ToString();
    }


    void doAppEvent()
    {
        if (Mathf.Approximately(appEventBeginSec, Time.time))
        {
            randomAttentionRow = UnityEngine.Random.Range(0, rows - 1);
            randomAttentionColumn = UnityEngine.Random.Range(0, cols - 1);
            arraySphere[randomAttentionRow, randomAttentionColumn].transform.localScale = new Vector3(0, 0, 0);
        }
        else if (Time.time >= appEventBeginSec + appEventDurationSecs)
        {
            appInEventFlag = false;
            arraySphere[randomAttentionRow, randomAttentionColumn].transform.localScale = new Vector3(1, 1, 1);
        }
    }


    void doClickEvent()
    {
        if (Mathf.Approximately(clickEventBeginSec, Time.time))
        {
            randomPeripheralIndex = UnityEngine.Random.Range(0, numSpherePeriph - 1);
            circleSphere[randomPeripheralIndex].transform.localScale = new Vector3(0, 0, 0);
        }
        else if (Time.time >= clickEventBeginSec + clickEventDurationSecs)
        {
            clickInEventFlag = false;
            circleSphere[randomPeripheralIndex].transform.localScale = new Vector3(1, 1, 1);
        }
    }


    void appEventLogic()
    {
        if (appEventReadyFlag && UnityEngine.Random.Range(0.0f, 1.0f) < appEventProb)
        {
            appEventReadyFlag = false;
            appEventBeginSec = Time.time;
            appInEventFlag = true;
            appInWindowFlag = true;
            appEventCounter += 1;
        }

        if (appInEventFlag)
        {
            doAppEvent();
        }

        // are we ready for another event?
        if (Time.time >= appEventBeginSec + appEventDurationSecs + appRecoverySecs)
        {
            appEventReadyFlag = true;
        }

        // have we passed the event detection window?
        if (Time.time >= appEventBeginSec + appEventDetectionWindowSecs)
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
            clickEventBeginSec = Time.time;
            clickInEventFlag = true;
            clickInWindowFlag = true;
            clickEventCounter += 1;
        }

        if (clickInEventFlag)
        {
            doClickEvent();
        }

        // are we ready for another event?
        if (Time.time >= clickEventBeginSec + clickEventDurationSecs + clickRecoverySecs)
        {
            clickEventReadyFlag = true;
        }

        // have we passed the event detection window?
        if (Time.time >= clickEventBeginSec + clickEventDetectionWindowSecs)
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
                    appInWindowFlag = false;
                    print("app correct: " + appEventDetectCounter.ToString());
                }
                else
                {
                    appFalsePositiveCounter += 1;
                    print("app false positive: " + appFalsePositiveCounter.ToString());
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
                    clickInWindowFlag = false;
                    print("click correct: " + clickEventDetectCounter.ToString());
                }
                else
                {
                    clickFalsePositiveCounter += 1;
                    print("click false positive: " + clickFalsePositiveCounter.ToString());
                }
            }
        }
    }


}
