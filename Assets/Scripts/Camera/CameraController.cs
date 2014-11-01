using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float horizontalSpeed = 10f;
	public float verticalSpeed = 10f;
	public float smoothness = 5f;
	
	public RectTransform selectionBox;
	bool isDragging = false;
	bool isSelecting = false;
	Rect selectionRectangle = new Rect(0,0,0,0);
	Vector2 startSelectionPosition = Vector2.zero;

	void Update()
	{
		// Start selection
		if (Input.GetMouseButtonDown(0))
		{
			isDragging = true;
			startSelectionPosition = Input.mousePosition;

			// The anchor is set to the same place.
			selectionBox.anchoredPosition = startSelectionPosition;
		}
		// Stop selection, reset everything
		else if (Input.GetMouseButtonUp(0))
		{
			isDragging = false;
			isSelecting = false;
			startSelectionPosition = Vector2.zero;
			selectionBox.anchoredPosition = Vector2.zero;
			selectionBox.sizeDelta = Vector2.zero;
		}

		if (isDragging && Input.GetMouseButton(0))
		{
			// Store the current mouse position in screen space.
			Vector2 currentMousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

			// How far have we moved the mouse?
			Vector2 difference = currentMousePosition - startSelectionPosition;

			// Copy the initial click position to a new variable. Using the original variable will cause
			// the anchor to move around to wherever the current mouse position is,
			// which isn't desirable.
			Vector2 startPoint = startSelectionPosition;
			
			// The following code accounts for dragging in various directions.
			if (difference.x < 0)
			{
				startPoint.x = currentMousePosition.x;
				difference.x = -difference.x;
			}
			if (difference.y < 0)
			{
				startPoint.y = currentMousePosition.y;
				difference.y = -difference.y;
			}

			isSelecting = difference.SqrMagnitude() > 0;

			// Set the anchor, width and height every frame.
			selectionBox.anchoredPosition = startPoint;
			selectionBox.sizeDelta = difference;

			selectionRectangle.x = startSelectionPosition.x;
			selectionRectangle.y = WorldToScreenY(startSelectionPosition.y);
			selectionRectangle.width = Input.mousePosition.x - startSelectionPosition.x;
			selectionRectangle.height = WorldToScreenY(Input.mousePosition.y) - WorldToScreenY(startSelectionPosition.y);

			if (selectionRectangle.width < 0)
			{
				selectionRectangle.x += selectionRectangle.width;
				selectionRectangle.width = -selectionRectangle.width;
			}
			if (selectionRectangle.height < 0)
			{
				selectionRectangle.y += selectionRectangle.height;
				selectionRectangle.height = -selectionRectangle.height;
			}
		}
	}

	void FixedUpdate()
	{
		// Get movement input
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		// Calculate offset
		Vector3 offset = new Vector3 (h * horizontalSpeed, 0f, v * verticalSpeed);

		// Change the camera's transform
		transform.position = Vector3.Lerp (transform.position, 
		                                   transform.position + offset, 
		                                   smoothness * Time.deltaTime);
	}

	// Use only after checking IsSelecting() == true
	public Rect GetSelectionRectangle()
	{
		return selectionRectangle;
	}

	public bool IsSelecting()
	{
		return isSelecting;
	}

	public static float WorldToScreenY(float y)
	{
		return Screen.height - y;
	}
}
