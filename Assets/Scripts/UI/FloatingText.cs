﻿using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public Text text;
    public Vector3 velocity = new Vector3(0, 0.1f, 0);

    public void Start()
    {
        text = GetComponentInChildren<Text>();

        var camera = Camera.main;
        if (camera != null)
        {
            transform.LookAt(camera.transform);
        }
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}