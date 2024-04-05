using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseData : ScriptableObject
{
    public string label;

    public virtual void DisplayDataBaseName()
    {
        Debug.Log("Base de " + label);
    }
}