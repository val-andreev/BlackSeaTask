using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    public bool boxOnTarget;
    private SpriteRenderer spr_Renderer;
    private void Awake()
    {
        spr_Renderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void CheckIfBoxOnTargetAndChangeColour(List<Vector2> allTargets)
    {
        if (allTargets.Contains(transform.position))
        {
            spr_Renderer.color = Color.red;
            boxOnTarget = true;
            return;
        }
        else
        {
            spr_Renderer.color = Color.white;
            boxOnTarget = false;
        }


    }
    public void SetBoxOnTargetWhenLaunching()
    {
        spr_Renderer.color = Color.red;
        boxOnTarget = true;
    }



}