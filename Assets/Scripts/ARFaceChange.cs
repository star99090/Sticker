using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class ARFaceChange : MonoBehaviour
{
    ARFaceManager arFaceManager;

    public Material[] materials;

    private int switchCount = 0;

    private void Start()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        arFaceManager.facePrefab.GetComponent<MeshRenderer>().material = materials[0];
    }

    private void Update()
    {
        if(Input.touchCount>0&&Input.GetTouch(0).phase == TouchPhase.Began)
        {
            SwitchFaces();
        }
    }

    void SwitchFaces()
    {
        foreach(ARFace face in arFaceManager.trackables)
        {
            face.GetComponent<MeshRenderer>().material = materials[switchCount];
        }

        switchCount = (switchCount + 1) % materials.Length;
    }
}
