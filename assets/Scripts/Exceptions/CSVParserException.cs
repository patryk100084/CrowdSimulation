using System;
using UnityEngine;

/// <summary>
/// Exception thrown by CSVParser class
/// </summary>
public class CSVParserException : Exception {
    /// <summary>
    /// Basic exception constructor
    /// </summary>
    public CSVParserException()
	{
		
	}
    /// <summary>
    /// constructor containing message
    /// </summary>
    /// <param name="message">cause of exception</param>
    public CSVParserException(string message) : base(message)
	{
		
	}
}
