using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

public static class Parameter
{
    public static PlayerParameter GetPlayerParameter()
    {
        var json = File.ReadAllText("parameter.json");
        var PlayerParameter = JsonUtility.FromJson<PlayerParameter>(json);
        return PlayerParameter;
    }
}

