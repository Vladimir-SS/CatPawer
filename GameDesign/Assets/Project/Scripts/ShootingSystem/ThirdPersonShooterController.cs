using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;

public class ThirdPersonShooterController : MonoBehaviour
{
    //in case you want to change the camera itself later
    [SerializeField] private CinemachineVirtualCamera airmVirtualCamera;
    [SerializeField] private float normalSenstivity = 1f;
    [SerializeField] private float aimSenstivity = 0.5f;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;
        


    
    private void Update()
    {
        


        if (starterAssetsInputs.aim)
        {
            airmVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSenstivity);
            thirdPersonController.SetRotateOnMove(false);

            Vector3 aimPosition = AimRaycastProvider.GetCameraTargetPosition();

            Vector3 aimDirection = (aimPosition - transform.position).normalized;
            aimDirection.y = 0f;
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            airmVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSenstivity);
            thirdPersonController.SetRotateOnMove(true);
        }



    }

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
    }
}
