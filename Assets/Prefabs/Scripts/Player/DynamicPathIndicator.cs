using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class DynamicPathIndicator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Transform playerTrans;
    public float updateInterval = 0.2f;
    private float timer;

    [Header("Distance Settings")]
    public float disappearDistance = 10f; // Path disappears when within this distance from target
    private bool pathHasDisappeared = false;

    private NavMeshPath path;

    void Start()
    {
        playerTrans = transform; // Attached to Player

        // Add or get LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Configure LineRenderer
        lineRenderer.startWidth = 0.25f;
        lineRenderer.endWidth = 0.25f;
        lineRenderer.positionCount = 0;
        
        // Use a clean shader to make it look great
        Shader defaultShader = Shader.Find("Sprites/Default");
        if (defaultShader != null)
        {
            lineRenderer.material = new Material(defaultShader);
        }
        
        // Set beautiful glowing colors
        lineRenderer.startColor = Color.cyan;
        lineRenderer.endColor = Color.yellow;
        
        lineRenderer.alignment = LineAlignment.View;
        
        path = new NavMeshPath();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdatePath();
        }
    }

    void UpdatePath()
    {
        if (pathHasDisappeared)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        // Find closest target based on scene and state
        Transform target = FindClosestTarget();
        if (target == null)
        {
            lineRenderer.positionCount = 0; // Clear path if no target
            return;
        }

        // Check if player is close to the target (the fence/cercado in Level 1)
        string activeScene = SceneManager.GetActiveScene().name;
        float distToTarget = Vector3.Distance(playerTrans.position, target.position);
        if (activeScene == "Level1" && distToTarget <= disappearDistance)
        {
            pathHasDisappeared = true;
            lineRenderer.positionCount = 0; // Hide the path
            return;
        }

        // Sample start and end points on NavMesh to ensure they are on the walkable mesh
        Vector3 startPos = playerTrans.position;
        Vector3 endPos = target.position;

        // Use a generous 15m radius to guarantee snapping to a valid NavMesh point
        if (NavMesh.SamplePosition(playerTrans.position, out NavMeshHit startHit, 15f, NavMesh.AllAreas))
        {
            startPos = startHit.position;
        }
        if (NavMesh.SamplePosition(target.position, out NavMeshHit endHit, 15f, NavMesh.AllAreas))
        {
            endPos = endHit.position;
        }

        // Calculate path on NavMesh
        if (NavMesh.CalculatePath(startPos, endPos, NavMesh.AllAreas, path))
        {
            // Only draw path if it successfully calculated a valid path on the NavMesh
            if (path.status == NavMeshPathStatus.PathComplete && path.corners != null && path.corners.Length > 1)
            {
                lineRenderer.positionCount = path.corners.Length;
                for (int i = 0; i < path.corners.Length; i++)
                {
                    // Adjust height slightly so it floats above the ground
                    Vector3 pos = path.corners[i];
                    pos.y += 0.25f;
                    lineRenderer.SetPosition(i, pos);
                }
            }
            else
            {
                // Clear the path if it cannot be fully calculated on the NavMesh.
                // This prevents drawing straight fallback lines that pierce through houses/trees.
                lineRenderer.positionCount = 0;
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }

    Transform FindClosestTarget()
    {
        string activeScene = SceneManager.GetActiveScene().name;

        if (activeScene == "Tutorial")
        {
            // Check if player picked up the weapon (Frieren Staff)
            bool hasWeapon = false;
            if (WeaponManager.Instance != null && WeaponManager.Instance.activeWeaponSlot != null)
            {
                if (WeaponManager.Instance.activeWeaponSlot.transform.childCount > 0)
                {
                    hasWeapon = true;
                }
            }

            if (!hasWeapon)
            {
                // Target the inactive weapon (which is located inside the first enclosure/cercado)
                Weapon[] weapons = Object.FindObjectsByType<Weapon>(FindObjectsSortMode.None);
                foreach (var w in weapons)
                {
                    if (w != null && !w.isActiveWeapon && w.gameObject.activeInHierarchy)
                    {
                        return w.transform;
                    }
                }
                
                // Fallback: look for GameObject named "frieren-staff"
                GameObject staffObj = GameObject.Find("frieren-staff");
                if (staffObj != null) return staffObj.transform;
            }
            else
            {
                // Path disappears when the staff is picked up
                return null;
            }
        }
        else if (activeScene == "Level1")
        {
            // Target the GameObject named "Walllvl0 (1)" as explicitly requested
            GameObject targetObj = GameObject.Find("Walllvl0 (1)");
            if (targetObj != null)
            {
                return targetObj.transform;
            }
        }

        return null;
    }
}