using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsVisibleCube : MonoBehaviour
{
    // external objects
    private ViewCubeManager VCM;

    [Header("ViewCubeManager")]
    [SerializeField]
    private GameObject viewInspectorGO;

    [Header("Animation")]
    [SerializeField]
    private bool isAnimated;
    [SerializeField]
    private GameObject animationPreFab;

    private string activeColorsMode;

    private bool isVisible = false;
    private GameObject UIItem;

    private float viewDistance;
    private GameObject player;

    private void Start()
    {
        // get other objects
        VCM = viewInspectorGO.GetComponent<ViewCubeManager>();
        player = VCM.GetPlayer();
        viewDistance = VCM.GetViewDistance();
        activeColorsMode = VCM.GetColorsMode();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (VCM.isActiveAndEnabled)
        {
            float adjacent = transform.position.z - player.transform.position.z;
            float opposite = transform.position.x - player.transform.position.x;

            float distance = Mathf.Sqrt(Mathf.Pow(adjacent, 2) + Mathf.Pow(opposite, 2));

            // if item was not visible
            if (isVisible == false)
            {
                // is item enter camera view
                if (distance < viewDistance)
                {
                    UIItem = VCM.BecameVisible(this.gameObject);

                    isVisible = true;
                }
            }
            // else item was already visible
            else
            {
                // if item go out of camera view
                if (distance > viewDistance)
                {
                    VCM.BecameInvisible(UIItem);

                    isVisible = false;

                    UIItem = null;
                }
                // else it stays in camera view
                else
                {
                    VCM.StillVisible(this.gameObject, UIItem);
                }
            }

            if (activeColorsMode != VCM.GetColorsMode())
            {
                if (isVisible == true)
                {
                    VCM.BecameInvisible(UIItem);
                    UIItem = VCM.BecameVisible(this.gameObject);
                }

                activeColorsMode = VCM.GetColorsMode();
            }
        }
    }

    private void OnDisable()
    {
        if (viewInspectorGO.gameObject != null && UIItem != null)
        {
            VCM.BecameInvisible(UIItem);
        }
    }

    public bool GetIsAnimated()
    {
        return isAnimated;
    }

    public GameObject GetAnimationPrefab()
    {
        return animationPreFab;
    }
}