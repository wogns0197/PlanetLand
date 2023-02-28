using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Move : MonoBehaviour
{
    private struct CharacterAnim
    {
        bool bIsJump;
        bool bIsPickup;
        bool bIsRun;
        bool bIsWave;
        bool bIsWalk;

        float fJumpSpeed_up, fJumpSpeed_float, fJumpSpeed_down;
        float fWalkSpeed, fRunSpeed;
        float fWaveSpeed, fPickupSpeed;

        public CharacterAnim(bool _bIsJump, bool _bIsPickup, bool _bIsRun, bool _bIsWave,
            bool _bIsWalk, float _fJumpSpeed_up, float _fJumpSpeed_float, float _fJumpSpeed_down, float _fWalkSpeed, float _fRunSpeed, float _fWaveSpeed, float _fPickupSpeed)
        {
            this.bIsJump    = _bIsJump;
            this.bIsPickup  = _bIsPickup;
            this.bIsRun     = _bIsRun;
            this.bIsWave    = _bIsWave;
            this.bIsWalk    = _bIsWalk;
            this.fJumpSpeed_up = _fJumpSpeed_up;
            this.fJumpSpeed_float = _fJumpSpeed_float;
            this.fJumpSpeed_down = _fJumpSpeed_down;
            this.fWalkSpeed = _fWalkSpeed;
            this.fRunSpeed = _fRunSpeed;
            this.fWaveSpeed = _fWaveSpeed;
            this.fPickupSpeed = _fPickupSpeed;
        }
    }

    CharacterAnim CharacterAnimData;
    private float x, y, fSpeed, fRodSpeed;
    private int dMoveForward, dRotLeft;
    private Rigidbody rg;

    private bool bInputWalk;
    void Start()
    {

        CharacterAnimData = new CharacterAnim(false, false, false, false, false, 0f,  0f, 0f, 0f, 0f, 0f, 0f);

        dMoveForward = 0;
        dRotLeft = 0;
        fSpeed = .1f;
        fRodSpeed = 4f;
        rg = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }
    // anim
    // use :::::: animator.SetBool("New Bool", true);

    void Move()
    {
        // w : 119, s : 115, d : 100, a : 97
        dMoveForward = 0;
        dRotLeft = 0;
        if ( Input.GetKey(KeyCode.W) )
        {
            dMoveForward = 1;
        }

        else if ( Input.GetKey(KeyCode.S) )
        {
            dMoveForward = -1;
        }
        
        if(Input.GetKey(KeyCode.D))
        {
            dRotLeft = 1;
     	}   
     	else if(Input.GetKey(KeyCode.A)){
            dRotLeft = -1;
        }

        if ( dMoveForward != 0 )
        {
            this.transform.Rotate(new Vector3(0,0.1f,0) * ( dMoveForward == 1 ? 1 : -1 ) * fRodSpeed );
        }

        if ( dRotLeft != 0 )
        {
            this.transform.Translate(new Vector3(0,0,0.1f) * ( dRotLeft == 1 ? 1 : -1 ) * fSpeed );
        }

        

     	// if(Input.GetKey(KeyCode.W)){
        //     int a = 0;
        //     a += (int)KeyCode.W;
     	// 	this.transform.Translate(new Vector3(0,0,0.1f) * fSpeed);
        //     // cameraReset();
        //     //ani.SetBool ("isWalking", true );
     	// }
        // else if(Input.GetKeyUp(KeyCode.W)){
        //     //ani.SetBool ("isWalking", false);

        // }
     	// if(Input.GetKey(KeyCode.S)){
     	// 	this.transform.Translate(new Vector3(0,0,-0.1f) * fSpeed);
     	// }   
     	// if(Input.GetKey(KeyCode.D)){
        //     this.transform.Rotate(new Vector3(0,0.1f,0) * fRodSpeed);
            
     	// }   
     	// if(Input.GetKey(KeyCode.A)){
        //     this.transform.Rotate(new Vector3(0,-0.1f,0) * fRodSpeed);
        // }
    }


}
