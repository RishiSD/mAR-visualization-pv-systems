using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Touch;



public class MarExperienceManager : MonoBehaviour
{
    enum AppPhase
    {
        WELCOME_SCREEN,
        BUILDING_SETUP,
        PANEL_SETUP,
        FINAL_VIEW
    }

    private static AppPhase appPhase;
    public GameObject building;

    public GameObject panel;

    public GameObject grid;

    [SerializeField]
    public TextMeshProUGUI logTextElement;
    [SerializeField]
    public TextMeshProUGUI titleTextElement;
    [SerializeField]
    public TextMeshProUGUI distTextElement;
    private GameObject spawnedObject;
    private GameObject spawnedPanel;
    private Vector2 touchPosition;

    [SerializeField]
    private Camera arCamera;
    private void Awake()
    {
        appPhase = AppPhase.WELCOME_SCREEN;
        logTextElement.text = "Hello";
        titleTextElement.text = "PV MAR Visualization";
    }

    public void PlaceBuildingInScene()
    {
        appPhase =  AppPhase.BUILDING_SETUP;
        GameObject build = Instantiate(building, new Vector3(30,-1,152), default);
        build.tag = "Building";
        titleTextElement.text = "Setup Building";
    }

    public void DisableBuildingLeanTouch()
    {
        appPhase =  AppPhase.PANEL_SETUP;
        titleTextElement.text = "Setup Panel";
        if ( GameObject.FindWithTag("Building") != null ) 
        {
            logTextElement.text = "Building Found";
            GameObject.FindWithTag("Building").GetComponent<LeanDragTranslate>().enabled = false;
            GameObject.FindWithTag("Building").GetComponent<LeanTwistRotateAxis>().enabled = false;
        }
        else 
        {
            logTextElement.text = "Building Not Found";
        }   
    }

    public void ChangePVModel(string modelName)
    {
        GameObject panelTmp = GameObject.FindWithTag("Panel");
        if ( panelTmp )
        {
            logTextElement.text = "Panel Found in ChangePV";
            if ( modelName == "Panel" )
            {
                logTextElement.text = "Instantiate Panel";
                panelTmp.SetActive(false);
                spawnedPanel = Instantiate(panel, panelTmp.transform.position, 
                                                panelTmp.transform.rotation);
                spawnedObject.transform.localScale = panelTmp.transform.localScale;
                spawnedPanel.tag = "Panel";
            }
            else
            {
                logTextElement.text = "Instantiate Grid";
                panelTmp.SetActive(false);
                spawnedPanel = Instantiate(grid, panelTmp.transform.position, 
                                                panelTmp.transform.rotation);
                spawnedPanel.tag = "Panel";
                spawnedObject.transform.localScale = panelTmp.transform.localScale;
            }
        }
        
    }

    public void DisablePanelLeanTouch()
    {
        appPhase =  AppPhase.PANEL_SETUP;
        titleTextElement.text = "Final Scene View";
        if ( GameObject.FindWithTag("Panel") != null ) 
        {
            logTextElement.text = "Panel Found";
            GameObject.FindWithTag("Panel").GetComponent<LeanDragTranslate>().enabled = false;
            GameObject.FindWithTag("Panel").GetComponent<LeanPinchScale>().enabled = false;
        }
        else 
        {
            logTextElement.text = "Panel Not Found";
        }   
    }

    public void RemoveBuildingFromScene()
    {
        appPhase =  AppPhase.FINAL_VIEW;
        titleTextElement.text = "Setup Panel";
        if ( GameObject.FindWithTag("Building") != null ) 
        {
            GameObject build = GameObject.FindWithTag("Building");
            build.SetActive(false);
            logTextElement.text += "Building disabled";
        }
    }

    void Update()
    {   
        if (appPhase == AppPhase.BUILDING_SETUP)
        {
            GameObject build = GameObject.FindWithTag("Building");
            distTextElement.text = "Distance from user : ~ " + 
                                    build.transform.position.z.ToString() +
                                    " units";
        } 

        if (appPhase == AppPhase.PANEL_SETUP)
        {
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                
                touchPosition = touch.position;

                if(touch.phase == TouchPhase.Began)
                {
                    Ray ray = arCamera.ScreenPointToRay(touch.position);
                    RaycastHit hitObject;
                    if(Physics.Raycast(ray, out hitObject))
                    {
                        spawnedObject = hitObject.transform.gameObject;
                        GameObject build = GameObject.FindWithTag("Building");
                        logTextElement.text += "Collision found with " + 
                                                spawnedObject.tag + hitObject.point.ToString() +
                                                "Rotation " + build.transform.localRotation.eulerAngles.y.ToString();
                        if ( spawnedPanel == null ) 
                        {
                            spawnedPanel = Instantiate(panel, 
                                                new Vector3(hitObject.transform.position.x, 
                                                    hitObject.transform.position.y,
                                                    hitObject.transform.position.z - 2), 
                                            Quaternion.Euler(0, 
                                                            build.transform.localRotation.eulerAngles.y + 180, 
                                                            0));
                            spawnedPanel.tag = "Panel";
                        }
                    }
                    else 
                    {
                        logTextElement.text += "No object found";
                    }
                }
            }
        }

    }
}
