using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// script generating unique ID for character
/// </summary>
public class IDGenerator : MonoBehaviour {
    /// <summary>
    /// character ID
    /// </summary>
    private string ID;

    // Use this for initialization
    /// <summary>
    /// generates ID
    /// </summary>
    void Start () {
		ID = GenerateID();
	}
    /// <summary>
    /// generates ID 
    /// </summary>
    /// <returns>ID</returns>
    string GenerateID()
	{
		StringBuilder builder = new StringBuilder();
		Enumerable
   		.Range(65, 26)
    	.Select(e => ((char)e).ToString())
    	.Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
    	.Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
    	.OrderBy(e => Guid.NewGuid())
    	.Take(11)
    	.ToList().ForEach(e => builder.Append(e));
		return builder.ToString();
	}
    /// <summary>
    /// ID getter
    /// </summary>
    /// <returns>ID</returns>
    public string GetID()
	{
		return ID;
	}
}
