using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Laser : MonoBehaviour
{
    private float speed = 20f;
    private float lifetime = 0.5f;
    private int startTime = 10;
    private float currentTime;
    private bool acabou;

    private void Start()
    {
        currentTime = startTime;
    }

    private void Update()
    {
       
    }
    
 }
