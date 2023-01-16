using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// interface contaning method describing movement behaviour
/// </summary>
public interface IMovementStrategy {
    /// <summary>
    ///  method representing movement behaviour
    /// </summary>
    /// <param name="speed">character rotation speed</param>
    /// <param name="trs">position towards which character is heading</param>
    /// <param name="targrtRot">rotation towards which character is heading</param>
    void MoveCharacter(ref float speed, Transform trs, Quaternion targrtRot);
}
