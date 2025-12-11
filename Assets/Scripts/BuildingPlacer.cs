using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    public LayerMask groundMask;
    public LayerMask buildingMask;
    public float checkRadius = 1f;
    public Material validMat;
    public Material invalidMat;

    [SerializeField] ClickCheckBuilding interaction;

    private GameObject preview;
    private Building buildingData;
    private bool isPlacing = false;
    private Renderer previewRenderer;

    [SerializeField] private UIManager UI;

    [SerializeField] private GameManager gameManager;

    void Update()
    {
        if (!isPlacing || preview is null)
            return;

        FollowMouse();
 
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TryPlace();
        }
    }

    // Launch Placement Mode
    public void StartPlacing(GameObject buildingPrefab, Building building)
    {
        buildingData = building;

        preview = Instantiate(buildingPrefab);
        previewRenderer = preview.GetComponentInChildren<Renderer>();

        // Deactivate colliders
        foreach (Collider c in preview.GetComponentsInChildren<Collider>())
            c.enabled = false;

        isPlacing = true;
    }

    // Make building's ghost follow mouse
    private void FollowMouse()
    {
        interaction.buildButton.interactable=false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
                Quaternion Rotation = preview.transform.rotation;
                Rotation.y += 90;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                Quaternion Rotation = preview.transform.rotation;
                Rotation.y -= 90;
            }
            if (Input.GetMouseButtonDown (1))
            {
                Destroy(preview);
                isPlacing = false;
                interaction.buildButton.interactable = true;
            }
            preview.transform.position = hit.point;

            //Check collider
            bool canPlace = !Physics.CheckSphere(hit.point, checkRadius, buildingMask);
            previewRenderer.material = canPlace ? validMat : invalidMat;
        }
    }

    // Try to place the building
    private void TryPlace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            bool blocked = Physics.CheckSphere(hit.point, checkRadius, buildingMask);
            if (!blocked) PlaceBuilding(hit.point);
        }
    }

    //Place Building
    private void PlaceBuilding(Vector3 position)
    {
        // Create real building
        GameObject finalBuilding = Instantiate(preview, position, preview.transform.rotation);
        buildingData = finalBuilding.GetComponent<Building>();
        // Activate collider
        foreach (Collider c in finalBuilding.GetComponentsInChildren<Collider>())
            c.enabled = true;

        Destroy(preview);  // Destroy building's ghost
        isPlacing = false; //End of placement
        gameManager._numberWood -= buildingData.cost[0];
        gameManager._numberStone -= buildingData.cost[1];
        UI.UpdateResourceText();
        buildingData.UpdateBuildEffect();
        interaction.buildButton.interactable = false;
    }
}