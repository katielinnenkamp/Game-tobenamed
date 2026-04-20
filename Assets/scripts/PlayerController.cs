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

    private Inventory inventory = new Inventory(5); //player inventory tracker, holds current items

    private VisualElement uidoc;
    [SerializeField]
    private GameObject UI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        notinmenu = true; //TODO ensure player doesn't startin the menu or pause screen, thus messing this up

        bounds = GetComponent<Collider>().bounds;
        bounds.Expand(-2 * skinwidth);

        yrotation = 0f;
        lookup = 0f;

        cam = Camera.main;

        if(UI.TryGetComponent<UIDocument>(out var temp))
        {
            /*uidoc = temp.rootVisualElement;
            selectedname = uidoc.Q<Label>("selected_name");*/
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
                TryPickup();
            }
            //debug test drop
            if(Keyboard.current.fKey.wasPressedThisFrame)
            {
                if(inventory.NumItems() > 0)
                {
                    Instantiate(inventory.GetItem(inventory.NumItems() - 1).item_prefab, transform.position + (Quaternion.Euler(lookup, yrotation, 0f) * Vector3.forward), Quaternion.identity);
                    inventory.RemoveItem(inventory.NumItems() - 1);
                }
            }
            if(Keyboard.current.iKey.wasPressedThisFrame)
            {
                if(notinmenu)
                {
                    OpenInventory();
                }
                else
                {
                    CloseInventory();
                }
            }
        }
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
    
    float maxpickupdist = 2f;
    bool TryPickup()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, Quaternion.Euler(lookup, yrotation, 0f) * Vector3.forward, out hit, maxpickupdist))
        
        if(hit.collider != null)
        {
            if(hit.collider.TryGetComponent<Pickup>(out var item)) 
            {
                item.Grab(inventory);
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    void OpenInventory()
    {
        notinmenu = false;


    }
    void CloseInventory()
    {
        notinmenu = true;


    }
}
