using System;
using UnityEngine;

/// <summary>
/// Exception thrown by ImageLoader class
/// </summary>
public class ImageLoaderException : Exception {
    /// <summary>
    /// Basic exception constructor
    /// </summary>
    public ImageLoaderException()
	{

	}
    /// <summary>
    /// constructor containing message
    /// </summary>
    /// <param name="message">cause of exception</param>
    public ImageLoaderException(string message) : base(message)
	{
		
	} 
}
