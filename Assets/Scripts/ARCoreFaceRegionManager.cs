using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;

public class ARCoreFaceRegionManager : MonoBehaviour
{
    [Header("Sticker Prefabs")]
    public GameObject nosePrefab;
    public GameObject leftHeadPrefab;
    public GameObject rightHeadPrefab;
    public Material fadeMaterial;
    [Space(10)]

    [Header("Debug Face")]
    public Material debugFaceMaterial;

    ARFaceManager arFaceManager;
    ARSessionOrigin sessionOrigin;

    NativeArray<ARCoreFaceRegionData> faceRegions;

    GameObject noseObject;
    GameObject leftHeadObject;
    GameObject rightHeadObject;

    bool isFox;

    void Start()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        sessionOrigin = GetComponent<ARSessionOrigin>();
    }

    public void IsFoxTrue()
    {
        isFox = true;

        foreach(ARFace face in arFaceManager.trackables)
           face.GetComponent<MeshRenderer>().material = fadeMaterial;
    }

    public void IsFoxFalse()
    {
        isFox = false;

        Destroy(noseObject);
        Destroy(leftHeadObject);
        Destroy(rightHeadObject);

        foreach (ARFace face in arFaceManager.trackables)
            face.GetComponent<MeshRenderer>().material = debugFaceMaterial;
    }

    void Update()
    {
        if (isFox)
        {
            ARCoreFaceSubsystem subsystem = (ARCoreFaceSubsystem)arFaceManager.subsystem;

            if(arFaceManager.trackables.count == 0)
            {
                Destroy(noseObject);
                Destroy(leftHeadObject);
                Destroy(rightHeadObject);
            }

            foreach (ARFace face in arFaceManager.trackables)
            {
                subsystem.GetRegionPoses(face.trackableId, Unity.Collections.Allocator.Persistent, ref faceRegions);

                foreach (ARCoreFaceRegionData faceRegion in faceRegions)
                {
                    ARCoreFaceRegion regionType = faceRegion.region;
                    
                    if (regionType == ARCoreFaceRegion.NoseTip)
                    {
                        if (!noseObject)
                            noseObject = Instantiate(nosePrefab, sessionOrigin.trackablesParent);

                        noseObject.transform.localPosition = faceRegion.pose.position;
                        noseObject.transform.localRotation = faceRegion.pose.rotation;
                    }
                    else if (regionType == ARCoreFaceRegion.ForeheadLeft)
                    {
                        if (!leftHeadObject)
                            leftHeadObject = Instantiate(leftHeadPrefab, sessionOrigin.trackablesParent);

                        leftHeadObject.transform.localPosition = faceRegion.pose.position;
                        leftHeadObject.transform.localRotation = faceRegion.pose.rotation;
                    }
                    else if (regionType == ARCoreFaceRegion.ForeheadRight)
                    {
                        if (!rightHeadObject)
                            rightHeadObject = Instantiate(rightHeadPrefab, sessionOrigin.trackablesParent);

                        rightHeadObject.transform.localPosition = faceRegion.pose.position;
                        rightHeadObject.transform.localRotation = faceRegion.pose.rotation;
                    }
                }
            }
        }
    }
}
