using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildingPlacer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public LayerMask groundMask;
    public LayerMask buildingMask;
    public float checkRadius = 1f;
    public Material validMat;
    public Material invalidMat;

    private GameObject _preview;
    private Building _buildingData;
    [SerializeField] GameObject buildingPrefab;
    private bool _isPlacing = false;
    private Renderer _previewRenderer;

    [SerializeField] private UIManager uiManager;

    [SerializeField] private GameManager gameManager;


    private Button _buildButton;
    void Start()
    {
        _buildButton = GetComponent<Button>();
        _buildingData = buildingPrefab.GetComponent<Building>();
        EnabledButtonBuildingBasedOnResources();
        gameManager._eventUpdateBuildButton.AddListener(EnabledButtonBuildingBasedOnResources);
        gameManager._eventBuildButtonDisable.AddListener(DisableButtonState);
    }
    void Update()
    {
        if (!_isPlacing || _preview is null)
            return;
        FollowMouse();
 
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            TryPlace();
        }
    }

    // Launch Placement Mode
    public void StartPlacing()
    {
        _preview = Instantiate(buildingPrefab);
        _previewRenderer = _preview.GetComponentInChildren<Renderer>();
        UpdateResourceAndBuildButton(-_buildingData.cost[0], -_buildingData.cost[1], false);
        // Deactivate colliders
        foreach (Collider c in _preview.GetComponentsInChildren<Collider>())
            c.enabled = false;

        _isPlacing = true;
    }

    // Make building's ghost follow mouse
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0f) //Move building rotation
            {
                Vector3 rotation = _preview.transform.rotation.eulerAngles;
                rotation.y += 90 *  Input.GetAxis("Mouse ScrollWheel");
                _preview.transform.rotation =  Quaternion.Euler(rotation);
            }
            if (Input.GetMouseButtonDown (1)) //Let a user cancel their building construction choice
            {
                Destroy(_preview);
                _isPlacing = false;
                UpdateResourceAndBuildButton(_buildingData.cost[0], _buildingData.cost[1], true);
            }
            _preview.transform.position = hit.point;

            //Check collider
            bool canPlace = !Physics.CheckSphere(hit.point, checkRadius, buildingMask);
            _previewRenderer.material = canPlace ? validMat : invalidMat;
        }
    }

    private void UpdateResourceAndBuildButton(int resource1, int resource2, bool buttonEnabled)
    {
        gameManager._numberWood += resource1;
        gameManager._numberStone += resource2;
        uiManager.UpdateResourceText();
        if (!buttonEnabled) gameManager._eventBuildButtonDisable.Invoke();
        else gameManager._eventUpdateBuildButton.Invoke();
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
        GameObject finalBuilding = Instantiate(_preview, position, _preview.transform.rotation);
        _buildingData = finalBuilding.GetComponent<Building>();
        // Activate collider
        foreach (Collider c in finalBuilding.GetComponentsInChildren<Collider>())
            c.enabled = true;

        Destroy(_preview);  // Destroy building's ghost
        _isPlacing = false; //End of placement
        uiManager.UpdateResourceText();
        gameManager._eventUpdateBuildButton.Invoke();
        _buildingData.UpdateBuildEffect();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void EnabledButtonBuildingBasedOnResources()
    {
        bool canBuild = gameManager._numberWood >= _buildingData.cost[0] && gameManager._numberStone >= _buildingData.cost[1] && gameManager._numberMason >= _buildingData.cost[2];
        _buildButton.interactable = canBuild;
    }
    
    private void DisableButtonState()
    {
        _buildButton.interactable = false;
    }
    
}