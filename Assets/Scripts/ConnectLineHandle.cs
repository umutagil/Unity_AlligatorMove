#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//Custom editor class to draw line handles between objects
//Draws a polygon starting from the first element to last element

[CustomEditor(typeof(ConnectedObjects))]
public class ConnectLineHandle : Editor {

    void OnSceneGUI()
    {
        ConnectedObjects connectedObjects = target as ConnectedObjects;
        if (connectedObjects == null || connectedObjects.objs.Length <= 2)
            return;

        for(int i = 1; i < connectedObjects.objs.Length; i++)
        {
            GameObject connectedObject1 = connectedObjects.objs[i - 1];
            GameObject connectedObject2 = connectedObjects.objs[i];
            Handles.DrawLine(connectedObject1.transform.position, connectedObject2.transform.position);
        }

        GameObject firstObject = connectedObjects.objs[0];
        GameObject lastObject = connectedObjects.objs[connectedObjects.objs.Length - 1];
        Handles.DrawLine(lastObject.transform.position, firstObject.transform.position);        
    }	
}

#endif
