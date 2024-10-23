using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class InteractableItem : MonoBehaviour
{
    [SerializeField] private bool isNeedInteractionTips = true;
    [SerializeField] protected GameObject interactionTips = null;
    [HideInInspector]
    public string tipsContent = "Press 'F' ";
    [SerializeField] private KeyCode keyCode = KeyCode.F;
    private bool isCollding = false;
    [SerializeField] private bool doOnce = false;
    private bool hasInteracted = false;

    void Start()
    {
        // check variable legality
        if (isNeedInteractionTips && interactionTips == null)
        {
            interactionTips = GameObject.Find("InteractionTips");
            print("Please Give the 'InteractionTips' to this gameObject: " + gameObject.name);
        }

        // correct the corresponding text content
        if (isNeedInteractionTips)
        {
            if (interactionTips.GetComponent<TextMeshProUGUI>())
                interactionTips.GetComponent<TextMeshProUGUI>().text = tipsContent;
            else if (interactionTips.GetComponent<Text>())
                interactionTips.GetComponent<Text>().text = tipsContent;
        }

    }
    
    // virtual method for interactable item
    public virtual void InteractableItemOnTriggerEnter(Collider other) {}
    public virtual void InteractableItemOnTriggerExit(Collider other) {}
    public virtual void AfterPressInteract() {}

    private void Update()
    {
        if (Input.GetKey(keyCode) && isCollding && (doOnce && !hasInteracted))
        { 
            print("AfterPressInteract");
            AfterPressInteract();
            hasInteracted = true;
            if (doOnce) interactionTips.SetActive(false);
        }
        
    }

    // Enter Trigger appear interaction tips, and exit to vanish it
    private void OnTriggerEnter(Collider other)
    {
        if (isNeedInteractionTips && other.gameObject.tag.Equals("Player") && !(doOnce && hasInteracted))
        {
            interactionTips.SetActive(true);
        }

        isCollding = true;
        print(isCollding);
        InteractableItemOnTriggerEnter(other);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (isNeedInteractionTips && other.gameObject.tag.Equals("Player"))
        {
            interactionTips.SetActive(false);
        }

        isCollding = false;
        InteractableItemOnTriggerExit(other);

    }
    
}
