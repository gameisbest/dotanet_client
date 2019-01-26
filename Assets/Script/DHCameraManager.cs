/******************************************************************************
 * 
 *   이 소스 코드의 권리를 명시하는 주석을 제거하지 마시오.
 * 
 *   소스 코드에 대한 모든 권리는 (주) 앱크로스에 있습니다.
 *   Copyright 2012 (c) Appcross All Rights Reserved.
 * 
 *   E-Mail : admin@appcross.co.kr
 * 
 ******************************************************************************/

using UnityEngine;
using System.Collections;

using DHTimeSingle = System.Single;

//----------------------------------------------------------------------------*/
public class DHCameraManager : MonoBehaviour
{
    //----------------------------------------------------------------------------*/

    public enum State
    {

        DoNothing,

        MoveWithDragging,

        FollowTarget,

        OnForcedMove,
    }


    private const float DRAGGING_SPEED_DELTA_X = 2.0f;
    /* 카메라를 드래그로 이동할 때 속도 변화량(Z축)
     * */
    private const float DRAGGING_SPEED_DELTA_Z = 2.0f;

    /* 카메라의 시야 확대 / 축소할 때 속도 변화량

     * */
    private const float ZOOM_SPEED = 15.0f;

    /* 카메라 객체 이름

     * */
    private const string GAMEOBJECTNAME_CAMERA = "Camera";

    //----------------------------------------------------------------------------*/
    /* 싱글톤 인스턴스 획득

    //----------------------------------------------------------------------------*/
    public static DHCameraManager Singleton
    {
        get
        {
            return singleton;
        }
    }

    //----------------------------------------------------------------------------*/
    public static void SetBlockTouchForSkill(bool Value)
    {
        if (singleton != null)
        {
            singleton.blockMoveEventForSkillTouch = Value;
        }
    }

    //----------------------------------------------------------------------------*/
    public static void SetFollowingTarget(UnityEntity Target)
    {
        if (singleton != null)
        {
            singleton.followingTarget = Target;
        }
    }

    //----------------------------------------------------------------------------*/
    public static void SetCameraType(State CameraState)
    {
        if (singleton != null)
        {
            singleton.currentState = CameraState;
        }
    }

    //----------------------------------------------------------------------------*/
    public static void ViewCameraToTargetCharacter()
    {
        
        //Debug.Log("ViewCameraToTargetCharacter");
        if (singleton.followingTarget == null)
        {
            return;
        }

        if (singleton.followingTarget.GetRenderingTransform == null)
        {
            return;
        }




        Vector3 NowPos = singleton.followingTarget.GetRenderingTransform.position;

        //Debug.Log(" ViewCameraToTargetCharacter position:" + NowPos);
        NowPos.y = singleton.transformOnMap.position.y;
        NowPos.z -= 6;
        singleton.transformOnMap.position = NowPos;
    }

    //----------------------------------------------------------------------------*/
    public static void SetCameraToPosition(Vector3 Position)
    {
        if (singleton != null)
        {
            singleton.transformOnMap.position = Position;
        }
    }



    //----------------------------------------------------------------------------*/
    /* 피가 없을때 맞으면 뜨는 이펙트

    //----------------------------------------------------------------------------*/
    public void ShowIllEffect()
    {
        SendMessage("OnCustomTriggerWithValue",
                    "ShowBloodScreen",
                    SendMessageOptions.DontRequireReceiver);
    }

    //----------------------------------------------------------------------------*/
    public Transform TransformOnMap
    {
        get
        {
            return transformOnMap;
        }
    }

    //----------------------------------------------------------------------------*/
    public State CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }

    //----------------------------------------------------------------------------*/
    private void Awake()
    {
        singleton = this;

        transformOnMap = transform;
    }

    //----------------------------------------------------------------------------*/
    private void Start()
    {
        /* 카메라 객체 찾아서 등록하기

         * */
        //Transform CameraTransform = transformOnMap.FindChild(GAMEOBJECTNAME_CAMERA);
        //attachedCamera = CameraTransform.GetComponent<Camera>();

        started = true;
    }

    //----------------------------------------------------------------------------*/
    private bool Initialize()
    {
        if (!started)
        {
            return false;
        }



        initialized = true;

        SetCameraType(State.FollowTarget);

        //DHUnitEntity MyCharacter = DHScene_GamePlay.Singleton.myCharacter;
        //DHCameraManager.SetFollowingTarget(MyCharacter);

        return true;
    }

    //----------------------------------------------------------------------------*/
    private void OnDestroy()
    {
        singleton = null;
    }

    //----------------------------------------------------------------------------*/
    private void Update()
    {
        if (!initialized)
        {
            if (!Initialize())
            {
                return;
            }
        }
    }

    //----------------------------------------------------------------------------*/
    private void LateUpdate()
    {
        switch (currentState)
        {
            case State.DoNothing:
                {

                }
                break;
            case State.FollowTarget:
                {
                    OnFollowTargetUpdate();
                }
                break;
            case State.MoveWithDragging:
                {

                }
                break;
            case State.OnForcedMove:
                {
                }
                break;
        }
    }

    //----------------------------------------------------------------------------*/
    /* State 처리들 모음

    //----------------------------------------------------------------------------*/
    private void OnFollowTargetUpdate()
    {
        ViewCameraToTargetCharacter();
    }



    public Vector3 CameraColiderSize()
    {
        return col.size;
    }

    //----------------------------------------------------------------------------*/
    /* 맴버 변수

    //----------------------------------------------------------------------------*/
    private static DHCameraManager singleton = null;

    /* 지형맵 상의 위치 변환 객체

     * */
    private Transform transformOnMap = null;
    /* 현재 상태

     * */
    private State currentState = State.MoveWithDragging;
    //private State beforeState = State.MoveWithDragging;
    /* 따라다니는 대상

     * */
    private UnityEntity followingTarget = null;

    private bool isWorldToScreenRatioCalculated = false;
    private Vector2 worldToScreenRatio = Vector2.zero;
    private bool blockMoveEventForSkillTouch = false;


    /// [ Camera 객체 관련 ]

    /* 카메라 오브젝트(transformOnMap을 부모로 둔다)
     * */
    //private Camera attachedCamera = null;
    /* 카메라 오브젝트가 위치할 수 있는 최소 거리 위치를 표시하는 객체

     * */
    private Transform cameraLocalCoordMin = null;
    /* 카메라 오브젝트가 위치할 수 있는 최대 거리 위치를 표시하는 객체

     * */
    private Transform cameraLocalCoordMax = null;


    /// [ 기타 ]

    /* Start() 함수의 성공 여부

     * */
    private bool started = false;
    /* 초기화 여부

     * */
    private bool initialized = false;

    private BoxCollider col = null;
}
