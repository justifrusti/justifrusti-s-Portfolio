using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FractalMaster))]
public class RandomColorGenerator : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        FractalMaster master = (FractalMaster)target;

        if (GUILayout.Button("Randomize Colors"))
        {
            master.blackAndWhite = Random.Range(0.00f, 1.00f);
            master.redA = Random.Range(0.00f, 1.00f);
            master.greenA = Random.Range(0.00f, 1.00f);
            master.blueA = Random.Range(0.00f, 1.00f);
            master.redB = Random.Range(0.00f, 1.00f);
            master.greenB = Random.Range(0.00f, 1.00f);
            master.blueB = Random.Range(0.00f, 1.00f);
        }

        if (GUILayout.Button("Randomize Colors & Darkness"))
        {
            master.darkness = Random.Range(0, 100);

            master.blackAndWhite = Random.Range(0.00f, 1.00f);
            master.redA = Random.Range(0.00f, 1.00f);
            master.greenA = Random.Range(0.00f, 1.00f);
            master.blueA = Random.Range(0.00f, 1.00f);
            master.redB = Random.Range(0.00f, 1.00f);
            master.greenB = Random.Range(0.00f, 1.00f);
            master.blueB = Random.Range(0.00f, 1.00f);
        }

        if (GUILayout.Button("Randomize Fractal Speed"))
        {
            master.powerIncreaseSpeed = Random.Range(0.00f, 10.00f);
        }

        if(GUILayout.Button("Randomize All"))
        {
            master.darkness = Random.Range(0.00f, 100.00f);

            master.blackAndWhite = Random.Range(0.00f, 1.00f);
            master.redA = Random.Range(0.00f, 1.00f);
            master.greenA = Random.Range(0.00f, 1.00f);
            master.blueA = Random.Range(0.00f, 1.00f);
            master.redB = Random.Range(0.00f, 1.00f);
            master.greenB = Random.Range(0.00f, 1.00f);
            master.blueB = Random.Range(0.00f, 1.00f);

            master.powerIncreaseSpeed = Random.Range(0.00f, 10.00f);

            master.fractalPower = 1;
        }
    }
}
