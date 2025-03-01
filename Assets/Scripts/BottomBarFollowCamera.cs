using UnityEngine;

public class BottomBarFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // Reference to your main camera
    public RectTransform bottomBarRect; // Reference to your bottom bar's RectTransform
    public float yOffset = 0.05f; // Y offset from the bottom of the screen
    public float zoomSpeed = 5f;  // Speed at which the camera zooms in and out
    public float minZoom = 5f;    // Minimum zoom level (camera's orthographic size)
    public float maxZoom = 600f;   // Maximum zoom level (camera's orthographic size)

    private Vector3 dragStartPosition;
    private Vector3 cameraStartPosition;
    private bool isDragging = false;

    void Start()
    {
        // If bottomBarRect is not assigned, try to get it from this GameObject
        if (bottomBarRect == null)
            bottomBarRect = GetComponent<RectTransform>();

        // Set the anchors to stretch horizontally at bottom
        bottomBarRect.anchorMin = new Vector2(0, 0);
        bottomBarRect.anchorMax = new Vector2(1, 0);
        bottomBarRect.pivot = new Vector2(0.5f, 0);

        // Set left and right to zero to stretch full width
        bottomBarRect.offsetMin = new Vector2(0, bottomBarRect.offsetMin.y);
        bottomBarRect.offsetMax = new Vector2(0, bottomBarRect.offsetMax.y);

        // Apply the y offset
        bottomBarRect.anchoredPosition = new Vector2(0, yOffset);
    }

    void Update()
    {
        // Handle camera dragging logic
        HandleCameraDragging();
        // Handle zooming functionality
        HandleZoom();
    }

    // Method to handle camera dragging (without moving the UI)
    void HandleCameraDragging()
    {
        if (Input.GetMouseButtonDown(0)) // Detect mouse click
        {
            isDragging = true;
            dragStartPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            cameraStartPosition = mainCamera.transform.position;
        }
        if (isDragging && Input.GetMouseButton(0)) // Detect mouse drag
        {
            Vector3 dragDelta = mainCamera.ScreenToWorldPoint(Input.mousePosition) - dragStartPosition;
            Vector3 newCameraPosition = cameraStartPosition - dragDelta;
            newCameraPosition.z = cameraStartPosition.z;
            mainCamera.transform.position = newCameraPosition;
        }
        if (Input.GetMouseButtonUp(0)) // Stop dragging on mouse release
        {
            isDragging = false;
        }
    }

    // Method to handle zoom functionality
    void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0f)
        {
            float zoomAmount = scrollInput * zoomSpeed;
            mainCamera.orthographicSize -= zoomAmount;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
        }
    }
}