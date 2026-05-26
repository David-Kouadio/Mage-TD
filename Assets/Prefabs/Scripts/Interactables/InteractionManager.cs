using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance { get;set; }
    
    public Weapon hoveredWeapon = null;
    public FireAmmo hoveredFireAmmo = null;
    public Camera playerCam;
    private PlayerInput inputActions;

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Awake()
    {
        inputActions = new PlayerInput();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        Ray ray = playerCam.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit))
        {
            GameObject objectHitByRaycast = hit.transform.gameObject;

            if (objectHitByRaycast.GetComponent<Weapon>() && objectHitByRaycast.GetComponent<Weapon>().isActiveWeapon == false)
            {
                
                hoveredWeapon = objectHitByRaycast.gameObject.GetComponent<Weapon>();
                hoveredWeapon.GetComponent<Outline>().enabled = true;

                if (inputActions.OnFoot.Interact.triggered)
                {
                    WeaponManager.Instance.PickupWeapon(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredWeapon)
                {
                    hoveredWeapon.GetComponent<Outline>().enabled = false;
                }
            }
            
            //FireAmmo
            if (objectHitByRaycast.GetComponent<FireAmmo>())
            {
                
                hoveredFireAmmo = objectHitByRaycast.gameObject.GetComponent<FireAmmo>();
                hoveredFireAmmo.GetComponent<Outline>().enabled = true;

                if (inputActions.OnFoot.Interact.triggered)
                {
                    WeaponManager.Instance.PickupAmmo(hoveredFireAmmo);
                    Destroy(objectHitByRaycast.gameObject);
                }
            }
            else
            {
                if (hoveredFireAmmo)
                {
                    hoveredFireAmmo.GetComponent<Outline>().enabled = false;
                }
            }
        }


    }



}
