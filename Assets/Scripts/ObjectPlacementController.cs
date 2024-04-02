using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARRaycastManager))]
public class ObjectPlacementController : MonoBehaviour {
    public GameObject sampleObject;

    [SerializeField]
    public GameObject HUD;


    private TextMeshPro touchCountText;
    private TextMeshPro debugConsoleText;

    private GameObject spawnedObject;
    private ARRaycastManager raycastManager;
    private Vector2 touchPosition;

    private Touch touch;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake() {
        raycastManager = GetComponent<ARRaycastManager>();
        touchCountText = HUD.transform.Find("TouchCount").gameObject.GetComponent<TextMeshPro>();
        debugConsoleText = HUD.transform.Find("DebugConsole").gameObject.GetComponent<TextMeshPro>();
        if (raycastManager == null)
            debugConsoleText.text = "AR Raycast Manager component not found";
    }

    // Update is called once per frame
    void Update() {
        try {
            touchCountText.text = "TouchCount: " + Input.touchCount;
            if (Input.touchCount == 1) {
                touch = Input.GetTouch(0);
                touchPosition = touch.position;

                Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                List<ARRaycastHit> hits = new List<ARRaycastHit>();

                if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon)) {
                    var hitPose = hits[0].pose;
                    if (spawnedObject == null) {
                        spawnedObject = Instantiate(sampleObject, hitPose.position, Quaternion.identity);
                    } else {
                        spawnedObject.transform.position = hitPose.position;
                    }
                }
            }
        } catch (Exception e) {
            debugConsoleText.text = "Error: " + e;
        }
    }
}
