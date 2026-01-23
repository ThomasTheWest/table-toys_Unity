using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class fxProximityText : MonoBehaviour
{
    private Camera mainCamera;
    private Mouse mouse;
    private TextMeshProUGUI text;
    [SerializeField] private RawImage bgSmear;
    Plane plane = new Plane(Vector3.up, 0);
    private Vector3 worldPosition;

    [Header("Fade Settings")]
    [SerializeField] private float fadeStartDistance = 0.125f;
    [SerializeField] private float fadeEndDistance = 0.25f; 
    void Start()
    {
        mainCamera = Camera.main;
        text = GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        float rayDistance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out rayDistance))
            worldPosition = ray.GetPoint(rayDistance);

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);

        //tooltip shouldn't be visible unless your mouse is close to the text element
        float distance = Vector3.Distance(transform.position, worldPosition);

        float alpha = Mathf.InverseLerp(fadeEndDistance, fadeStartDistance, distance);
        Color currentColor = text.color;
        currentColor.a = alpha;
        text.color = currentColor;

        if (bgSmear != null)
            bgSmear.color = currentColor;
    }
}
