using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    [SerializeField] private bool isNeedInteractionTips = true;
    [SerializeField] private GameObject interactionTips;
    public string tipsContent = "Press 'F' ";
    [SerializeField] private KeyCode keyCode = KeyCode.F;

    void Start()
    {
        // check variable legality
        if (isNeedInteractionTips && interactionTips == null)
        {
            interactionTips = GameObject.Find("InteractionTips");
            print("Please Give the 'InteractionTips' to this gameObject: " + gameObject.name);
        }

        // fill the corresponding text content
        if (interactionTips.GetComponent<TextMeshProUGUI>())
            interactionTips.GetComponent<TextMeshProUGUI>().text = tipsContent;
        else if (interactionTips.GetComponent<Text>())
            interactionTips.GetComponent<Text>().text = tipsContent;

    }
    
    // virtual method for interactable item
    public virtual void InteractableItemOnTriggerEnter() {}
    public virtual void InteractableItemOnTriggerExit() {}
    public virtual void AfterPressInteract() {}

    private void Update()
    {
        if (Input.GetKey(keyCode))
            AfterPressInteract();
    }

    // Enter Trigger appear interaction tips, and exit to vanish it
    private void OnTriggerEnter(Collider other)
    {
        if (isNeedInteractionTips && other.gameObject.tag.Equals("Player"))
        {
            interactionTips.SetActive(true);
        }

        InteractableItemOnTriggerEnter();
    }

    private void OnTriggerExit(Collider other)
    {
        if (isNeedInteractionTips && other.gameObject.tag.Equals("Player"))
        {
            interactionTips.SetActive(false);
        }
        
        InteractableItemOnTriggerExit();

    }
    
}
