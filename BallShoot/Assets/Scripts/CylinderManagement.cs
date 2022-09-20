using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CylinderManagement : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    
    private bool buttonPressed;
    public GameObject cylinderObject;
    [SerializeField] private float turnCaliber;
    [SerializeField] private string direction;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
    
    void Update()
    {
        if (buttonPressed)
        {
            if (direction == "Left")
            {
                cylinderObject.transform.Rotate(0,turnCaliber * Time.deltaTime,0,Space.Self);
            }
            else
            {
                cylinderObject.transform.Rotate(0,-turnCaliber * Time.deltaTime,0,Space.Self);
            }
            
        }
        else
        {
            
        }
    }

    
}
