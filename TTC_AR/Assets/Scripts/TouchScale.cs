using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TouchScale : MonoBehaviour
{
    private float initialTouchDistance;
    private Vector3 initialScale;
    private bool isScaling = false;
    [SerializeField] private bool useScrollRect = true; // Chọn chế độ: true = ScrollRect, false = Image
    [SerializeField] private ScrollRect objectScrollRect;
    [SerializeField] private RectTransform contentRectTransform;
    [SerializeField] private RectTransform viewRectTransform; // Khung giới hạn cho Image
    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 5.0f;

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    private RectTransform imageRectTransform;
    private Vector2 lastDragPosition;
    private bool isDragging;

    private void Start()
    {
        InitializeComponents();
    }

    private void Update()
    {
        HandleTouchScaling();
        HandleMouseZoom();
        if (useScrollRect) EnableObjectScrollRect();
        else HandleDragging();
    }

    private void InitializeComponents()
    {
        initialScale = transform.localScale;
        imageRectTransform = GetComponent<RectTransform>();
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        if (useScrollRect)
        {
            if (objectScrollRect != null && contentRectTransform == null)
            {
                contentRectTransform = objectScrollRect.content;
            }
        }
        else
        {
            if (viewRectTransform == null)
            {
                var parent = transform.parent as RectTransform;
                if (parent != null) viewRectTransform = parent;
                else Debug.LogWarning("viewRectTransform not assigned, using parent RectTransform as fallback.");
            }
        }
    }

#if ENABLE_LEGACY_INPUT_MANAGER
    private void HandleTouchScaling()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (BothTouchesOverGameObject(touch0.position, touch1.position))
            {
                ProcessScaling(touch0.position, touch1.position);
            }
        }
        else
        {
            ResetScalingState();
        }
    }
#elif ENABLE_INPUT_SYSTEM
    private void HandleTouchScaling()
    {
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count == 2)
        {
            var touch0 = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[0];
            var touch1 = UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches[1];

            if (BothTouchesOverGameObject(touch0.screenPosition, touch1.screenPosition))
            {
                ProcessScaling(touch0.screenPosition, touch1.screenPosition);
            }
        }
        else
        {
            ResetScalingState();
        }
    }
#endif

    private void HandleMouseZoom()
    {
        if (Application.isEditor && Input.touchCount == 0)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0)
            {
                float scaleFactor = 1.0f + scrollInput * 0.1f;
                Vector3 newScale = transform.localScale * scaleFactor;

                newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
                newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
                newScale.z = 1f;

                transform.localScale = newScale;
                UpdateContentSizeOrClamp();
                initialScale = transform.localScale;
            }
        }
    }

    private void ProcessScaling(Vector2 touch0Position, Vector2 touch1Position)
    {
        float currentTouchDistance = Vector2.Distance(touch0Position, touch1Position);

        if (!isScaling)
        {
            BeginScaling(currentTouchDistance);
        }
        else
        {
            ApplyScaling(currentTouchDistance);
        }
    }

    private void BeginScaling(float currentTouchDistance)
    {
        initialTouchDistance = currentTouchDistance;
        isScaling = true;
    }

    private void ApplyScaling(float currentTouchDistance)
    {
        float scaleFactor = currentTouchDistance / initialTouchDistance;
        Vector3 newScale = initialScale * scaleFactor;

        newScale.x = Mathf.Clamp(newScale.x, minScale, maxScale);
        newScale.y = Mathf.Clamp(newScale.y, minScale, maxScale);
        newScale.z = 1f;

        transform.localScale = newScale;
        UpdateContentSizeOrClamp();
    }

    private void ResetScalingState()
    {
        isScaling = false;
        initialScale = transform.localScale;
        UpdateContentSizeOrClamp();
    }

    private void UpdateContentSizeOrClamp()
    {
        if (useScrollRect)
        {
            if (objectScrollRect != null && contentRectTransform != null && imageRectTransform != null)
            {
                Vector2 imageSize = imageRectTransform.sizeDelta * imageRectTransform.localScale;
                contentRectTransform.sizeDelta = imageSize;
                contentRectTransform.pivot = new Vector2(0.5f, 0.5f);
            }
        }
        else
        {
            ClampToFrame();
        }
    }

    private bool BothTouchesOverGameObject(Vector2 touch0Position, Vector2 touch1Position)
    {
        return IsTouchOverGameObject(touch0Position) && IsTouchOverGameObject(touch1Position);
    }

    private bool IsTouchOverGameObject(Vector2 touchPosition)
    {
        if (pointerEventData == null)
        {
            pointerEventData = new PointerEventData(eventSystem);
        }
        pointerEventData.position = touchPosition;
        var results = new System.Collections.Generic.List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);

        return results.Exists(result => result.gameObject == gameObject);
    }

    private void EnableObjectScrollRect()
    {
        if (objectScrollRect != null)
        {
            objectScrollRect.enabled = true;
        }
    }

    private void HandleDragging()
    {
        if (!useScrollRect)
        {
            // Handle single finger drag for touch
            if (Input.touchCount == 1)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    isDragging = true;
                    lastDragPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    isDragging = false;
                }
                else if (isDragging && touch.phase == TouchPhase.Moved)
                {
                    var currentPosition = touch.position;
                    var delta = currentPosition - lastDragPosition;
                    MoveImage(delta);
                    lastDragPosition = currentPosition;
                }
            }
            // Handle mouse drag in Editor
            else if (Application.isEditor && Input.touchCount == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragging = true;
                    lastDragPosition = Input.mousePosition;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    isDragging = false;
                }
                else if (isDragging && Input.GetMouseButton(0))
                {
                    var currentPosition = (Vector2)Input.mousePosition;
                    var delta = currentPosition - lastDragPosition;
                    MoveImage(delta);
                    lastDragPosition = currentPosition;
                }
            }
        }
    }

    private void MoveImage(Vector2 screenDelta)
    {
        if (!useScrollRect && viewRectTransform != null)
        {
            var rectTransform = imageRectTransform;
            var canvas = rectTransform.GetComponentInParent<Canvas>();
            var scaleFactor = canvas.scaleFactor;

            // Convert screen delta to local delta
            var localDelta = screenDelta / scaleFactor;

            // Apply delta to anchored position
            var newPosition = rectTransform.anchoredPosition + localDelta;

            // Clamp position within view frame
            ClampToFrame(newPosition);

            // Update position
            rectTransform.anchoredPosition = newPosition;
        }
    }

    private void ClampToFrame(Vector2? newPosition = null)
    {
        if (!useScrollRect && viewRectTransform != null)
        {
            var rectTransform = imageRectTransform;
            var canvas = rectTransform.GetComponentInParent<Canvas>();
            var scaleFactor = canvas.scaleFactor;

            // Get current or new position
            var currentPosition = newPosition ?? rectTransform.anchoredPosition;

            // Calculate image size after scaling
            var imageSize = new Vector2(
                rectTransform.sizeDelta.x * rectTransform.localScale.x,
                rectTransform.sizeDelta.y * rectTransform.localScale.y
            );

            // Calculate view frame bounds
            var viewRect = viewRectTransform.rect;
            var viewSize = new Vector2(viewRect.width, viewRect.height) * scaleFactor;
            var viewMin = viewRectTransform.anchoredPosition - viewSize / 2f;
            var viewMax = viewRectTransform.anchoredPosition + viewSize / 2f;

            // Clamp position to keep image within frame
            var clampedX = Mathf.Clamp(currentPosition.x, viewMin.x - (imageSize.x - viewSize.x) / 2f, viewMax.x + (imageSize.x - viewSize.x) / 2f);
            var clampedY = Mathf.Clamp(currentPosition.y, viewMin.y - (imageSize.y - viewSize.y) / 2f, viewMax.y + (imageSize.y - viewSize.y) / 2f);

            rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
        }
    }
}