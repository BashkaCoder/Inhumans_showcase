using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomFactory : MonoBehaviour
{
    [SerializeField] private GameObject buildingSlots;

    [SerializeField] private float scrollSensitivity = 10f;
    [SerializeField] private float MIN_SCALE; 
    [SerializeField] private float MAX_SCALE;
    private Vector3 initialScale; 

    void Start()
    {
        initialScale = buildingSlots.transform.localScale; 
    }

    void Update()
    {
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        ScaleWithClamp(scrollValue, MIN_SCALE, MAX_SCALE);
    }

    private void ScaleWithClamp(float scrollValue, float MIN_SCALE, float MAX_SCALE)
    {
        if (scrollValue != 0)
        {
            Vector3 newScale = initialScale;

            newScale.x = Mathf.Clamp(newScale.x + scrollValue * scrollSensitivity, MIN_SCALE, MAX_SCALE);
            newScale.y = Mathf.Clamp(newScale.y + scrollValue * scrollSensitivity, MIN_SCALE, MAX_SCALE);
            newScale.z = Mathf.Clamp(newScale.z + scrollValue * scrollSensitivity, MIN_SCALE, MAX_SCALE);

            buildingSlots.transform.localScale = Vector3.MoveTowards(buildingSlots.transform.localScale, newScale, scrollSensitivity * Time.deltaTime);
        }
    }
}
