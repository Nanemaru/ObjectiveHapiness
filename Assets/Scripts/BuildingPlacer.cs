using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    public LayerMask groundMask;          // Layer du sol
    public LayerMask buildingMask;        // Layer des bâtiments
    public float checkRadius = 1f;        // Taille de la zone de vérif
    public Material validMat;             // Material vert
    public Material invalidMat;           // Material rouge

    private GameObject preview;           // Le ghost du bâtiment
    private Building buildingData;        // Les données du bâtiment
    private bool isPlacing = false;
    private Renderer previewRenderer;

    [SerializeField] private GameManager gameManager;

    void Update()
    {
        if (!isPlacing || preview == null)
            return;

        FollowMouse();
 
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TryPlace();
        }
    }

    // Lance le mode placement
    public void StartPlacing(GameObject buildingPrefab, Building building)
    {
        buildingData = building;

        preview = Instantiate(buildingPrefab);
        previewRenderer = preview.GetComponentInChildren<Renderer>();

        // Désactive les collisions
        foreach (Collider c in preview.GetComponentsInChildren<Collider>())
            c.enabled = false;

        isPlacing = true;
    }

    // Le ghost suit la souris
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            preview.transform.position = hit.point;

            // Vérification collisions
            bool canPlace = !Physics.CheckSphere(hit.point, checkRadius, buildingMask);
            Debug.Log("test");
            previewRenderer.material = canPlace ? validMat : invalidMat;
        }
    }

    // Tentative de placement
    private void TryPlace()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            bool blocked = Physics.CheckSphere(hit.point, checkRadius, buildingMask);

            if (!blocked)
            {
                PlaceBuilding(hit.point);
            }
        }
    }

    private void PlaceBuilding(Vector3 position)
    {
        // Création du vrai bâtiment
        GameObject finalBuilding = Instantiate(preview, position, preview.transform.rotation);

        // Réactive les collisions
        foreach (Collider c in finalBuilding.GetComponentsInChildren<Collider>())
            c.enabled = true;

        Destroy(preview);  // Supprime le ghost
        isPlacing = false; // Fin du placement
        gameManager._numberWood -= buildingData.cost[0];
        gameManager._numberStone -= buildingData.cost[1];
        buildingData.UpdateBuildEffect();
    }
}