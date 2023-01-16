using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script responsible for forcing application frame rate
/// </summary>
public class SceneMenagerScript : MonoBehaviour
{
    /// <summary>
    /// frame rate to be forced
    /// </summary>
    public int animationFrameRate;

    /// <summary>
    /// forces aplication frame rate
    /// </summary>
    private void Start() 
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = animationFrameRate;
    }
}

