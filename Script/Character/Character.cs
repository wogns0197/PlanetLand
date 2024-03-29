using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Utility;

enum ECharacterMoveType
{
    None,
    Ship,
    Car,
    Fly,
}

[System.Serializable]
public class UnityAnimationEvent : UnityEvent<string>{};
[RequireComponent(typeof(Animator))]

public class Character : MonoBehaviour
{
public delegate void OnTriggerFieldObjectDelegate(EFieldTrigger t);

public OnTriggerFieldObjectDelegate OnTriggerFieldObject;

private struct CharacterAnim
{
    public bool bIsJump, /*bIsFloat,*/ bIsFallDown, bIsPickup, bIsRun, bIsWave, bIsWalk;

    public float fJumpSpeed_up, fJumpSpeed_float, fJumpSpeed_down;
    public float fWalkSpeed, fRunSpeed;
    public float fWaveSpeed, fPickupSpeed;

    public CharacterAnim(bool _bIsJump, bool _bIsPickup, bool _bIsRun, bool _bIsWave,
        bool _bIsWalk, float _fJumpSpeed_up, float _fJumpSpeed_float, float _fJumpSpeed_down, float _fWalkSpeed, float _fRunSpeed, float _fWaveSpeed, float _fPickupSpeed)
    {
        this.bIsJump    = _bIsJump;
        //this.bIsFloat   = false;
        this.bIsFallDown = false;
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

//move
CharacterAnim CharacterAnimData;
public Animator Animator;
public UnityAnimationEvent OnAnimationStart;
public UnityAnimationEvent OnAnimationComplete;

private float x, y, fSpeed, fRodSpeed;
private int dMoveForward, dRotLeft;
private Rigidbody rg;

private bool bInputWalk;


//camera
public Camera Camera;
private float fRotX, fRotY, fRotSpeed;

//ui
UIInstance UIInstance;

//movetype
ECharacterMoveType MoveType;
GameObject Vehicle;

void Awake()
{
    DontDestroyOnLoad(this);

    for(int i=0; i < Animator.runtimeAnimatorController.animationClips.Length; i++)
    {
        AnimationClip clip = Animator.runtimeAnimatorController.animationClips[i];

        AnimationEvent animationStartEvent = new AnimationEvent();
        animationStartEvent.time = 0;
        animationStartEvent.functionName = "AnimationStartHandler";
        animationStartEvent.stringParameter = clip.name;

        AnimationEvent animationEndEvent = new AnimationEvent();
        animationEndEvent.time = clip.length;
        animationEndEvent.functionName = "AnimationCompleteHandler";
        animationEndEvent.stringParameter = clip.name;

        clip.AddEvent(animationStartEvent);
        clip.AddEvent(animationEndEvent);
    }

    OnTriggerFieldObject = new OnTriggerFieldObjectDelegate(OnTriggerFieldObj);
    SceneManager.sceneLoaded += OnSceneLoaded;
}

void Start()
{
    CharacterAnimData = new CharacterAnim(false, false, false, false, false, 1f, 1f, 1f, 1f, 1f, 1f, 1f);

    dMoveForward = 0;
    dRotLeft = 0;
    fSpeed = .1f;
    fRodSpeed = 4f;

    fRotSpeed = 700f;
    rg = this.GetComponent<Rigidbody>();

    UIInstance = UIInstance.GetUIInstance();

    MoveType = ECharacterMoveType.None;
    Vehicle = null;
}

void Update()
{
    ProcessMove();
    ProcessAnim();
    UpdateCameraInput();
    ProcessUIInput();

    //test
    test();
}
// anim
// use :::::: animator.SetBool("New Bool", true);
void OnWalk()   { CharacterAnimData.bIsWalk = true; }
void StopWalk() { CharacterAnimData.bIsWalk = false; }
void OnRun()    { CharacterAnimData.bIsRun = true; }
void StopRun()  { CharacterAnimData.bIsRun = false; }
void OnJump()   
{ 
    CharacterAnimData.bIsWalk = false;
    CharacterAnimData.bIsRun = false;
    CharacterAnimData.bIsJump = true; 
}

void StopJump() 
{ 
    CharacterAnimData.bIsJump = false; 
    //CharacterAnimData.bIsFloat = false;
    CharacterAnimData.bIsFallDown = false;
}
void OnPickUp() { CharacterAnimData.bIsPickup = true; }
void StopPickUp() { CharacterAnimData.bIsPickup = false; }
void OnWave()   { CharacterAnimData.bIsWave = true; }
void StopWave()   { CharacterAnimData.bIsWave = false; }
void SetAnimIdle()
{
    StopWalk();
    StopJump();
    StopRun();
    StopWave();
}

void ProcessMove()
{
    if ( MoveType == ECharacterMoveType.None )
    {
        Move();
        Jump();
    }
}

void ProcessAnim()
{
    Animator.SetBool("bIsJump", CharacterAnimData.bIsJump );
    //Animator.SetBool("bIsFloat", CharacterAnimData.bIsFloat );
    Animator.SetBool("bIsFallDown", CharacterAnimData.bIsFallDown );

    
    Animator.SetBool("bIsWalk", CharacterAnimData.bIsWalk );

    Animator.SetBool("bIsRun", CharacterAnimData.bIsRun );

    // Allow Only NOT On Jumping
    if ( !CharacterAnimData.bIsJump )
    {
        Animator.SetBool("bIsWave", CharacterAnimData.bIsWave );
        Animator.SetBool("bIsPickup", CharacterAnimData.bIsPickup );
    }
}

void Move()
{
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

    if ( dRotLeft != 0 )
    {
        this.transform.Rotate(new Vector3(0,0.1f,0) * ( dRotLeft == 1 ? 1 : -1 ) * fRodSpeed );
        OnWalk();
    }
    else
    {
        StopWalk();
    }

    if ( dMoveForward != 0 )
    {
        this.transform.Translate(new Vector3(0,0,0.1f) * ( dMoveForward == 1 ? 1 : -1 ) * fSpeed );
        OnWalk();
    }
    else
    {
        StopWalk();
    }
}

void Jump()
{
    if ( Input.GetKeyDown(KeyCode.Space) )
    {
        if ( this.rg.velocity.y > 0 == false && CharacterAnimData.bIsJump == false )
        {
            this.rg.AddForce(new Vector3(0, 40f, 0));
            OnJump();
        }
    }

    if ( CharacterAnimData.bIsJump && this.rg.velocity.y < -1 )
    {
        //Debug.DrawRay(transform.position, -transform.up*8,Color.red);
        RaycastHit hit;
        if( Physics.Raycast(transform.position , -transform.up* 8, out hit , 8) )
        {
            // .5가 매직넘버라 추후에 좀더 높은 곳에서 떨어 질 것을 고려하여 수정해야함
            // Float 변수 일단 삭제
            if ( hit.distance <= .5f && CharacterAnimData.bIsFallDown == false)
            {
                CharacterAnimData.bIsFallDown = true;
            }
        }
    }
}

void test()
{
    ;
}

public void AnimationStartHandler(string name)
{
    // Debug.Log($"{name} animation start.");
    OnAnimationStart?.Invoke(name);
}

public void AnimationCompleteHandler(string name)
{
    // Debug.Log($"{name} animation complete.");
    OnAnimationComplete?.Invoke(name);

    if (name == "jump-down")
    {
        StopJump();
    }
}

// private void OnCollisionEnter(Collision other) 
// {
//     // 캐릭터와 땅 닿는 처리는 땅에 무조건 StepGround 태그를 달아줘야함!!
//     if ( other.gameObject.tag == "StopGround" )
//     {
//         StopJump();
//     }
// }

// =============================== Camera =================================

void UpdateCameraInput()
{
    if ( !Input.GetMouseButton(1) ) { return; }

    if ( Input.GetMouseButton(1) )
    {
        fRotX = Input.GetAxis("Mouse X") * Time.deltaTime * fRotSpeed;
        fRotY = Input.GetAxis("Mouse Y") * Time.deltaTime * fRotSpeed;

        //Debug.Log("X = " + Input.GetAxis("Mouse X") + ", Y = " + Input.GetAxis("Mouse Y"));
    }

    Vector3 pos = this.transform.position;

    Camera.transform.RotateAround(pos, Vector3.right * 3, fRotY);
    Camera.transform.RotateAround(pos, Vector3.up * 3   , fRotX);
    Camera.transform.LookAt(pos);

    // Camera.transform.rotation = Quaternion.Euler(Mathf.Clamp(Camera.transform.rotation.eulerAngles.x, 5, 70), Camera.transform.rotation.eulerAngles.y, Camera.transform.rotation.eulerAngles.z);
    if ( Camera.transform.rotation.eulerAngles.x > 70 ) {
        Camera.transform.rotation = Quaternion.Euler(70, Camera.transform.rotation.eulerAngles.y, Camera.transform.rotation.eulerAngles.z);
    }
}

// ================================ UI Input ==============================
void ProcessUIInput()
{
    if ( UIInstance == null ) { return; }
    if ( Input.GetKeyDown(KeyCode.I) )
    {
        UIInstance.OpenUI(EUIType.Inventory, UIInstance.IsUIShow(EUIType.Inventory) ? false : true);
    }
}


// =============================== Trigger FieldObj =======================
void OnTriggerFieldObj(EFieldTrigger type)
{
    switch (type)
    {
        case EFieldTrigger.None:
            break;
        case EFieldTrigger.Vehicle:
            break;
        case EFieldTrigger.PickUp:
            break;
        case EFieldTrigger.Portal:
            OnMoveMapbyPortal();
            break;
        default:
            break;
    }
}

private void OnCollisionEnter(Collision other) 
{
    if ( other.gameObject.layer == LayerMask.NameToLayer("Water") && Vehicle == null )
    {
        SetAnimIdle();

        // 하드코딩 300001 = ship
        if ( PUtility.GetInventoryData()[EItemType.Equip].ContainsKey(300001) )
        {
            MoveType = ECharacterMoveType.Ship;
            Debug.Log("!!");
            ItemSlotInfo si = PUtility.GetInventoryData()[EItemType.Equip][300001];
            Vehicle = Instantiate<GameObject>(
                Resources.Load<GameObject>(si.Path),
                this.transform.position,
                Quaternion.Euler(-90, this.transform.rotation.y, 0) );
        }
    }
}

void OnMoveMapbyPortal()
{
    SceneManager.LoadScene("SeaPlanet");
}

void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    this.transform.position = new Vector3(0, 10, 0); // 맵 마다 생성 포지션 다르게 해야함
}
}
