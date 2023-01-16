using System;
using UnityEngine;

/// <summary>
/// Exception thrown by ScreenshotAndDataHandler class
/// </summary>
public class DataHandlerException : Exception {
    /// <summary>
    /// Basic exception constructor
    /// </summary>
    public DataHandlerException()
	{

	}

    /// <summary>
    /// constructor containing message
    /// </summary>
    /// <param name="message">cause of exception</param>
    public DataHandlerException(string message) : base(message)
	{
		
	} 
}
