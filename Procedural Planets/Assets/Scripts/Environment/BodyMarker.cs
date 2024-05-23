using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BodyMarker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameTextField;
    [SerializeField] private RectTransform markerPivot;

    private float maxDisplayDst;

    private SolarCamController camController;
    private CelestialBody celestialBody;
    private Canvas canvas;
    private RectTransform canvasRect;

    private void Awake()
    {
        camController = FindObjectOfType<SolarCamController>();
    }

    public void InitializeMarker(Canvas canvas, CelestialBody celestialBody, float maxDisplayDst)
    {
        this.canvas = canvas;
        this.celestialBody = celestialBody;
        this.maxDisplayDst = maxDisplayDst;

        canvasRect = this.canvas.GetComponent<RectTransform>();
        nameTextField.text = this.celestialBody.name;
    }

    private void Update()
    {
        SetMarkerVisibility(CheckVisible());
        UpdatePosition();
    }

    public void SetMarkerVisibility(bool visible)
    {
        nameTextField.enabled = visible;
    }

    private void UpdatePosition()
    {
        Vector2 canvasPos;
        Vector2 screenPoint = camController.AttachedCamera.WorldToScreenPoint(celestialBody.transform.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, null, out canvasPos);
        markerPivot.localPosition = canvasPos;
    }

    private bool CheckVisible()
    {
        Vector3 screenSpacePos = camController.AttachedCamera.WorldToViewportPoint(celestialBody.transform.position);
        bool onScreen = screenSpacePos.x >= 0 && screenSpacePos.x <= 1 && screenSpacePos.y >= 0 && screenSpacePos.y <= 1 && screenSpacePos.z > 0;

        float dstToCam = Vector3.Distance(camController.transform.position, celestialBody.transform.position);
        return dstToCam < maxDisplayDst && dstToCam > 0f && onScreen && celestialBody.gameObject.activeSelf;
    }
}
