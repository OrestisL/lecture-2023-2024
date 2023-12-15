using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class SimpleControls : MonoBehaviour
{
    public InputActionAsset controls;
    [SerializeField] private PatrolMovement patrolMovement;
    public Camera main, cam;

    private void Start()
    {
        controls.FindActionMap("Keyboard").asset.Enable();
        controls.FindAction("leftclick", true).performed += context => 
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                if (patrolMovement == null) //no agent selected
                {
                    if (hit.transform.TryGetComponent(out patrolMovement))
                    {
                        return;
                    }
                }
                else //agent selected
                {
                    if (NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, 2.0f, NavMesh.AllAreas))
                    {
                        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.position = navHit.position;
                        cube.transform.localScale = Vector3.one * 0.5f;
                        cube.transform.up = hit.normal;
                        patrolMovement.AddPosition(navHit.position);
                        Destroy(cube, 2f);
                    }
                }

            }
        };

        controls.FindAction("rightclick").performed += context => { patrolMovement = null; };

        controls.FindAction("middleclick").performed += ctx => 
        {
            main.enabled = !main.enabled;
            cam.enabled = !cam.enabled;
        };
    }
}
