using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject;
    private IInteractable curinteractable;
    public TextMeshProUGUI promptText;
    private Camera _camera;

    private PlayerInput playerInput;
    private InputActionMap inputActionMap;
    private InputAction interactAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputActionMap = playerInput.actions.FindActionMap("Player");
        interactAction = inputActionMap.FindAction("Interaction");
        interactAction.started += OnInteractInput;
    }
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        if (Time.time - lastCheckTime < checkRate)
            return;
        lastCheckTime = Time.time;
        CheckRay();
    }
    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curinteractable.GetInteractPrompt();
    }
    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if ( curinteractable != null)
        {
            curinteractable.OnInteract();
            curInteractGameObject = null;
            curinteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
    void CheckRay()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, maxCheckDistance, layerMask)){
            if (hit.collider.gameObject != curInteractGameObject)
            {
                curInteractGameObject = hit.collider.gameObject;
                curinteractable = hit.collider.GetComponent<IInteractable>();
                SetPromptText();
            }
            
        }
        else
        {
            curInteractGameObject = null;
            curinteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
