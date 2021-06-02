using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARCore;
using Unity.Collections;

public class ARCoreFaceRegionManager : MonoBehaviour
{
    [Header("Sticker Prefabs")]
    [SerializeField] GameObject nosePrefab;
    [SerializeField] GameObject leftHeadPrefab;
    [SerializeField] GameObject rightHeadPrefab;
    [SerializeField] Material fadeMaterial;
    [Space(10)]

    [Header("Debug Face")]
    [SerializeField] Material debugFaceMaterial;

    [Header("ScreenShot")]
    [SerializeField] GameObject blink;
    [SerializeField] GameObject canvas;

    ARFaceManager arFaceManager;
    ARSessionOrigin sessionOrigin;

    NativeArray<ARCoreFaceRegionData> faceRegions;

    GameObject noseObject;
    GameObject leftHeadObject;
    GameObject rightHeadObject;

    bool isFox = false;
    bool isGreen = false;

    void Start()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        sessionOrigin = GetComponent<ARSessionOrigin>();
    }

    public void IsFoxToggle()
    {
        isGreen = false;

        if (!isFox)
        {
            isFox = true;

            foreach (ARFace face in arFaceManager.trackables)
                face.GetComponent<MeshRenderer>().material = fadeMaterial;
        }
        else
        {
            isFox = false;

            Destroy(noseObject);
            Destroy(leftHeadObject);
            Destroy(rightHeadObject);
        }
    }

    public void IsGreenToggle()
    {
        isFox = false;

        Destroy(noseObject);
        Destroy(leftHeadObject);
        Destroy(rightHeadObject);

        if (!isGreen)
        {
            isGreen = true;

            foreach (ARFace face in arFaceManager.trackables)
                face.GetComponent<MeshRenderer>().material = debugFaceMaterial;
        }
        else
        {
            isGreen = false;

            foreach (ARFace face in arFaceManager.trackables)
                face.GetComponent<MeshRenderer>().material = fadeMaterial;
        }
    }

    public void Screenshot() => StartCoroutine(Capture());
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

    IEnumerator Capture()
    {
        string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
        string fileName = "Screenshot" + timeStamp + ".png";
        string pathToSave = fileName;
        canvas.SetActive(false);
        ScreenCapture.CaptureScreenshot(pathToSave);
        yield return new WaitForEndOfFrame();
        Instantiate(blink, Vector2.zero, Quaternion.identity);
        canvas.SetActive(true);
    }
}
