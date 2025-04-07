using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetElementToSelectOnInteraction : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject elementToSelect;

    // pour visualiser la navigation
    [SerializeField] private bool showVisualization;
    [SerializeField] private Color navigationColor = Color.white;

    private void OnDrawGizmos()
    {
        // visualiser la navigation
        Gizmos.color = navigationColor;
        Gizmos.DrawLine (gameObject.transform.position, elementToSelect.gameObject.transform.position);
    }

    private void Reset()
    {
        eventSystem = FindAnyObjectByType<EventSystem>();
    }

    public void JumpToElement()
    {
        eventSystem.SetSelectedGameObject(elementToSelect.gameObject);
    }

}
