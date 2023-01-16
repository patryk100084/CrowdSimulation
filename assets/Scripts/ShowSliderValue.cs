using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// prints FPS value chosen using slider
/// </summary>
public class ShowSliderValue : MonoBehaviour {
    /// <summary>
    /// slider
    /// </summary>
    private Slider slider;
    /// <summary>
    /// text box above slider 
    /// </summary>
    private Text txt;
    /// <summary>
    /// finds slider and text box on menu
    /// </summary>
    private void Awake()
	{
		slider = gameObject.GetComponent<Slider>();
		Transform trs = gameObject.transform.Find("Text");
		txt = trs.gameObject.GetComponent<Text>();
	}
    /// <summary>
    /// shows slider value
    /// </summary>
    private void OnGUI() 
	{
		txt.text = "Frame Rate: " + slider.value;
	}
}
