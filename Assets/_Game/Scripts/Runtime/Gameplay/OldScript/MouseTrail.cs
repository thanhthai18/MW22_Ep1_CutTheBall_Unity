using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTrail : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    public TrailRenderer trail;

    private bool isHoldMouse;

    private void Awake()
    {
        isHoldMouse = false;
        mainCamera = Camera.main;
        trail = GetComponent<TrailRenderer>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            trail.enabled = false;
            isHoldMouse = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHoldMouse = false;
            
        }
        if (isHoldMouse)
        {
            transform.position = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            if (!trail.enabled)
            {
                trail.enabled = true;
            }
        }
    }

}
