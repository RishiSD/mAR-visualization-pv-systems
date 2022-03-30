using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Lean.Touch;

public class TapToPlace : MonoBehaviour
{
    public GameObject building;

    [SerializeField]
    public TextMeshProUGUI logTextElement;

    private GameObject spawnedObject;
    private Vector2 touchPosition;

    [SerializeField]
    private Camera arCamera;
    private void Awake()
    {
        Instantiate(building, new Vector3(30,-1,152), default);
        logTextElement.text = "Hello";
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }

    public void DisableBuildingLeanTouch()
    {
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

    void Update()
    {
        //if (!TryGetTouchPosition(out Vector2 touchPosition))
        //    return;
        
        if(Input.touchCount > 0)
        {
            logTextElement.text += "Touch found";
            
            Touch touch = Input.GetTouch(0);
            
            touchPosition = touch.position;

            if(touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);
                RaycastHit hitObject;
                if(Physics.Raycast(ray, out hitObject))
                {
                    //logTextElement.text += "Collision found";
                    spawnedObject = hitObject.transform.gameObject;
                    logTextElement.text += "Collision found" + hitObject.point.ToString();
                    //Gizmos.color = Color.red;
                    //Gizmos.DrawWireSphere(hitObject.point, 0.2f);
                }
            }
            //spawnedObject = Instantiate(gameObjectToInstantiate, new Vector3(30,-1,152), default);
        }
    }
}
