using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : MonoBehaviour
{
    public LayerMask groundMask;          // Layer du sol
    public LayerMask buildingMask;        // Layer des b�timents
    public float checkRadius = 1f;        // Taille de la zone de v�rif
    public Material validMat;             // Material vert
    public Material invalidMat;           // Material rouge

    private GameObject preview;           // Le ghost du b�timent
    private Building buildingData;        // Les donn�es du b�timent
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

        // D�sactive les collisions
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

            // V�rification collisions
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
        // Cr�ation du vrai b�timent
        GameObject finalBuilding = Instantiate(preview, position, preview.transform.rotation);

        // R�active les collisions
        foreach (Collider c in finalBuilding.GetComponentsInChildren<Collider>())
            c.enabled = true;

        Destroy(preview);  // Supprime le ghost
        isPlacing = false; // Fin du placement
        gameManager._numberWood -= buildingData.cost[0];
        gameManager._numberStone -= buildingData.cost[1];
        buildingData.UpdateBuildEffect();
    }
}