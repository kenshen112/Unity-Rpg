﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PScreen : ScriptableObject
{
    public GameObject Screen;
    Shader ScreenShader;
    public GameObject CurrentScreen;
    bool isOpened;

    public void Open(GameObject CScreen) // Instantiates the GameObject Screen. This is meant only to display what needs to be drawn in the current design.
    {
        if (CScreen)
        {
            if (CurrentScreen)
            {
                Destroy(CurrentScreen);
            }
            
            Screen = GameObject.Find("Menu");
            Screen.SetActive(true);
            CurrentScreen = (GameObject)Instantiate(CScreen);
            CurrentScreen.GetComponent<AppData>().Init();
            CurrentScreen.transform.SetParent(Screen.transform);
            CurrentScreen.transform.localPosition = new Vector2(0, 0);
        }
        //Screen.SetActive(true);
    }

    public GameObject GetScreen
    {     
        get
        {
            return Screen;
        }
    }

    public void Draw()
    {
        // This is more like an update function? OR is this what spawns the screen from the get go.
        // Should I use shaders on the screen to make it look more lcd like?
        if (CurrentScreen)
        {
            CurrentScreen.GetComponent<AppData>().Draw();
        }
    }

    public void Close()
    {
        foreach(Transform Widget in CurrentScreen.transform)
        {
            Destroy(Widget.gameObject);
        }

        Destroy(CurrentScreen);    
        //Screen.SetActive(false);
    }
}
