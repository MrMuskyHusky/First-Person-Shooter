using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Free for fall GameMode
/// </summary>
public class GameModeFFA : GameMode
{
    const string typeName = "FFA";
    GameObject[] gameObjects;
    private GameObject worldGame;
    private void Awake()
    {
        //gameObjects = worldGame.FindGameObjectsWithTag(typeName);
    }

    /*private void Start()
    {
        base.Start();
    }*/
}
