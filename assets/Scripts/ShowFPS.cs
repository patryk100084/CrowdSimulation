using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// script showing FPS of the applcation
/// </summary>
public class ShowFPS : MonoBehaviour {
    /// <summary>
	/// FPS of appliaction
	/// </summary>
    private float fps = 30f;

    /// <summary>
    /// calculates FPS and prints it on top-left corner of application
    /// </summary>
    void OnGUI()
	{
		float newFPS = 1.0f / Time.deltaTime;
		fps = Mathf.Lerp(fps, newFPS, 0.02f);
		GUIStyle style = new GUIStyle();
		style.fontSize = 18;
		GUI.Label(new Rect(0, 0, 100, 100), "FPS: " + ((int)fps).ToString(), style);
	}
}
