using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
using UnityEngine.UIElements;

public class playerMove : MonoBehaviour
{
    [SerializeField]
    private float movespeed; //movement speed; can be freely modified and will apply automatically
    [SerializeField]
    private InputActionReference moveref; //movement reference; must be assigned and unchanged
    private Camera cam; //camera; assigned to main camera automatically (should be the only camera in the scene)

    //used in "collide and slide" collision calculations
    private Bounds bounds;

    private float sensitivity = 1f; //sensitivity of mouse movement

    [SerializeField]
    private float gravity = 18f; //gravity; higher gravity feels less floaty

    private int heldindex = 0;
    private int hotbarslots = 6;
    private Inventory inventory = new Inventory(6); //player inventory tracker, holds current items
    private VisualElement slot0;
    private VisualElement slot1;
    private VisualElement slot2;
    private VisualElement slot3;
    private VisualElement slot4;
    private VisualElement slot5;
    private VisualElement[] icons = new VisualElement[6];

    private VisualElement uidoc;
    [SerializeField]
    private GameObject UI;

    [SerializeField]
    private GameObject righthand;
    [SerializeField]
    private GameObject lefthand;

    public MyInputActions controls;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        notinmenu = true; //TODO ensure player doesn't startin the menu or pause screen, thus messing this up

        bounds = GetComponent<Collider>().bounds;
        bounds.Expand(-2 * skinwidth);

        yrotation = 0f;
        lookup = 0f;

        cam = Camera.main;

        controls = new MyInputActions();

        controls.Enable();

        if(UI.TryGetComponent<UIDocument>(out var temp))
        {
            uidoc = temp.rootVisualElement;
            //selectedname = uidoc.Q<Label>("selected_name");
            slot0 = uidoc.Q<VisualElement>("slot0");
            slot1 = uidoc.Q<VisualElement>("slot1");
            slot2 = uidoc.Q<VisualElement>("slot2");
            slot3 = uidoc.Q<VisualElement>("slot3");
            slot4 = uidoc.Q<VisualElement>("slot4");
            slot5 = uidoc.Q<VisualElement>("slot5");

            icons[0] = slot0.Q<VisualElement>("icon");
            icons[1] = slot1.Q<VisualElement>("icon");
            icons[2] = slot2.Q<VisualElement>("icon");
            icons[3] = slot3.Q<VisualElement>("icon");
            icons[4] = slot4.Q<VisualElement>("icon");
            icons[5] = slot5.Q<VisualElement>("icon");

            controls.Hotbar.SelectSlot1.performed += lamb => TryChangeHeld(0);
            controls.Hotbar.SelectSlot2.performed += lamb => TryChangeHeld(1);
            controls.Hotbar.SelectSlot3.performed += lamb => TryChangeHeld(2);
            controls.Hotbar.SelectSlot4.performed += lamb => TryChangeHeld(3);
            controls.Hotbar.SelectSlot5.performed += lamb => TryChangeHeld(4);
            controls.Hotbar.SelectSlot6.performed += lamb => TryChangeHeld(5);

            controls.Player.Attack.performed += UseHeld;
            
            UpdateUI();
        }   
        else
        {
            Debug.Log("UI doc not found...");
        }


    }

    bool Grounded()
    {
        if(Physics.Raycast(transform.position, Vector3.down, 1.0625f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private float movex;
    private float movey;
    void OnMove(InputValue move)
    {
        Vector2 movementVector = move.Get<Vector2>(); 

        movex = movementVector.x;
        movey = movementVector.y;
    }

    private int maxBounces = 5;
    private float skinwidth = 0.015f;
    private Vector3 CollideAndSlide(Vector3 vel, Vector3 pos, int depth = 0)
    {
        if(depth >= maxBounces)
        {
            return Vector3.zero;
        }

        float dist = vel.magnitude + skinwidth;

        RaycastHit hit;

        if(Physics.CapsuleCast(new Vector3(pos.x, pos.y + 0.5f, pos.z), new Vector3(pos.x, pos.y - 0.5f, pos.z), bounds.extents.x, vel.normalized, out hit, dist))
        {
            Vector3 snapToSurface = vel.normalized * (hit.distance - skinwidth);
            Vector3 leftover = vel - snapToSurface;

            if(snapToSurface.magnitude <= skinwidth)
            {
                snapToSurface = Vector3.zero;
            }

            float mag = leftover.magnitude;
            leftover = Vector3.ProjectOnPlane(leftover, hit.normal).normalized;
            leftover *= mag;

            return snapToSurface + CollideAndSlide(leftover, pos + snapToSurface, depth + 1);
        }

        return vel;
    }
    private void ChangeHeld()
    {

    }

    private float yrotation;
    private float lookup;
    private bool notinmenu;
    // Update is called once per frame
    void Update()
    {
        //wrap in boolean to disable when inside of a menu
        if(notinmenu)
        {
            Vector2 mousedelt = Mouse.current.delta.ReadValue();
            yrotation += mousedelt.x * sensitivity;
            lookup -= mousedelt.y * sensitivity;
            lookup = Mathf.Clamp(lookup, -85f, 85f);
            transform.rotation = Quaternion.Euler(0f, yrotation, 0f);
            cam.transform.rotation = Quaternion.Euler(lookup, yrotation, 0f);
            if(Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryInteract();
            }
            //debug test drop
            if(Keyboard.current.qKey.wasPressedThisFrame)
            {
                DropItem(heldindex);
            }
        }
        //not currently implemented
        /*if(Keyboard.current.iKey.wasPressedThisFrame)
        {
            if(notinmenu)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }*/
        //HandleNumberKeys();
        //HandleScrollWheel();

    }
    private float vertspeed;
    void FixedUpdate()
    {
        //get our movement and apply it
        Vector3 movement = new Vector3(movex * Time.deltaTime * movespeed, 0f, movey * Time.deltaTime * movespeed);
        movement = Quaternion.Euler(0f, yrotation, 0f) * movement;
        this.transform.position += CollideAndSlide(movement, this.transform.position);

        // fall if we're in the air, stop falling if we're on the ground
        if(!Grounded())
        {
            vertspeed = 0f - Mathf.Min(-(vertspeed) + (Time.deltaTime * gravity), 55f);
        }
        if(vertspeed != 0f)
        {
            transform.position += CollideAndSlide(new Vector3(0f, (vertspeed * Time.deltaTime), 0f), this.transform.position);
        }
        if(Grounded() && vertspeed <= 0f)
        {
            vertspeed = 0f;
        }
    }

    //TODO update to input manager
    void HandleScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f)) return;


        // Scroll down = next slot, scroll up = previous, wrapping around
        int next = scroll < 0
            ? (heldindex + 1) % hotbarslots
            : (heldindex - 1 + hotbarslots) % hotbarslots;

        TryChangeHeld(next);
    }
    
    float maxinteractdist = 2f;
    bool TryInteract()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, Quaternion.Euler(lookup, yrotation, 0f) * Vector3.forward, out hit, maxinteractdist))
        
        if(hit.collider != null)
        {
            if(hit.collider.TryGetComponent<Pickup>(out var item)) 
            {
                AddToInventory(item);
            }
            else if(hit.collider.TryGetComponent<Useable>(out var obj))
            {
                obj.Interact(heldindex, inventory, this);
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void DropItem(int ind)
    {
        if(inventory.NumItems() > 0 && inventory.NumItems() > ind && !inventory.SlotEmpty(ind))
        {
            Instantiate(RemoveItem(ind).item_prefab, 
                transform.position + (Quaternion.Euler(lookup, yrotation, 0f) * Vector3.forward), 
                Quaternion.identity);
        }
        else
        {
            UpdateUI();
            return;
        }
        UpdateUI();
    }
    //TODO this is public so interactables can take ingredients and keys when you use them, probably come up with a better solution
    public Item RemoveItem(int ind)
    {
        if(ind >= inventory.NumItems())
        {
            return null;
        }
        Item ret;
        inventory.TryGetItem(ind, out ret);
        inventory.RemoveItem(ind);
        UpdateUI();
        return ret;
    }
    void AddToInventory(Pickup item)
    {
        item.Grab(inventory);
        UpdateUI();
    }

    Item GetHeldItem()
    {
        Item ret;
        if(inventory.TryGetItem(heldindex, out ret))
        {
            return ret;
        }
        return null;
    }
    bool TryChangeHeld(int ind)
    {
        if(ind < hotbarslots){
            heldindex = ind;
            UpdateUI();
            return true;
        }
        else{
            return false;
        }
    }

    void OpenInventory()
    {
        notinmenu = false;

        UpdateUI();
    }
    void CloseInventory()
    {
        notinmenu = true;

        UpdateUI();
    } 

    void UpdateUI()
    {
        //selection updating
        slot0.RemoveFromClassList("slot-selected");
        slot1.RemoveFromClassList("slot-selected");
        slot2.RemoveFromClassList("slot-selected");
        slot3.RemoveFromClassList("slot-selected");
        slot4.RemoveFromClassList("slot-selected");
        slot5.RemoveFromClassList("slot-selected");
        switch(heldindex)
        {
            case 0:
                slot0.AddToClassList("slot-selected");
                break;
            case 1:
                slot1.AddToClassList("slot-selected");
                break;
            case 2:
                slot2.AddToClassList("slot-selected");
                break;
            case 3:
                slot3.AddToClassList("slot-selected");
                break;
            case 4:
                slot4.AddToClassList("slot-selected");
                break;
            case 5:
                slot5.AddToClassList("slot-selected");
                break;
        }
        
        int slots = inventory.GetSlots();
        for(int i = 0; i < slots; i++)
        {
            icons[i].style.backgroundImage = StyleKeyword.None;
        }
        for(int i = 0; i < slots; i++)
        {
            Item temp;
            if(inventory.TryGetItem(i, out temp))
            {
                icons[i].style.backgroundImage = new StyleBackground(temp.icon);
            }
        }
    }  

    void UseHeld(InputAction.CallbackContext ctx)
    {
        Item held = GetHeldItem();
        if(held != null)
        {
            held.Use(gameObject);
        }
        return;
    }
}