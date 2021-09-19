﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class Control6DOF : MonoBehaviour 
{
  #region Private Variables
  private MLInput.Controller _controller;
  #endregion

  #region Unity Methods
  void Start () 
  {
    _controller = MLInput.GetController(MLInput.Hand.Right);
    Debug.Log(_controller);
  }

  void Update () 
  {
    //Attach the Beam GameObject to the Control
    transform.position = _controller.Position;
    transform.rotation = _controller.Orientation;
  }
  #endregion
}


