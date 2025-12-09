using UnityEngine;

public class GunAnimations : MonoBehaviour
{

    public Animator gunAnimator;
    public Camera cam;
    public LayerMask hitMask;

    [Header("FX")]
    public ParticleSystem muzzleFlash;


    //start
    void Start()
    {
        
    }

    //update
    void Update()
    {
        bool leftPressed = Input.GetMouseButtonDown(0);

        if (leftPressed)
        {

            //play fire animation
            gunAnimator.SetTrigger("Fire");
            if (muzzleFlash) muzzleFlash.Play(true);
        }
    }
}
