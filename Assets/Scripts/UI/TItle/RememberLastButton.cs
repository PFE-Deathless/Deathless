using UnityEngine;
using UnityEngine.EventSystems;

public class RememberLastButton : MonoBehaviour
{
	[SerializeField] private EventSystem eventSystem;
	[SerializeField] private GameObject lastSelectedElement;

	private void Reset()
	{
		eventSystem = FindObjectOfType<EventSystem>();
        lastSelectedElement = eventSystem.firstSelectedGameObject;
		
	}

	void Update()
	{
		if (eventSystem.currentSelectedGameObject &&
            lastSelectedElement != eventSystem.currentSelectedGameObject)
			lastSelectedElement = eventSystem.currentSelectedGameObject;

		if (!eventSystem.currentSelectedGameObject && lastSelectedElement)
			eventSystem.SetSelectedGameObject(lastSelectedElement);
		
	}
}
