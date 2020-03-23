using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Compass : MonoBehaviour
{
    public RawImage compassScrollTexture;
    public Transform playerPositionInWorld;

    
    void Update()
    {
        compassScrollTexture.uvRect = new Rect(playerPositionInWorld.localEulerAngles.y / 360f,0,1,1);
    }
}
