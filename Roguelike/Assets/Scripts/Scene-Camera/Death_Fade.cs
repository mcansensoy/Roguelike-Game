using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death_Fade : MonoBehaviour
{
    public static Death_Fade ins_;

    [SerializeField] private Transform player;
    //[SerializeField] CinemachineVirtualCamera virtual_camera;
    [Space(10)]
    [SerializeField] private Canvas _canvas;
    [SerializeField] private Image _black_screen;
    [SerializeField] private float fade_dur = 2;

    private Vector3 player_screenPos;
    Rect canvasRect;


    private float screenWidth = Screen.width;
    private float screenHeight = Screen.height;
    private float canvasWitdth, canvasHeight;




    private Vector2 _player_Canvas_pos;

    private static readonly int RADIUS = Shader.PropertyToID("_Radius");
    private static readonly int CenterX = Shader.PropertyToID("_CenterX");
    private static readonly int CenterY = Shader.PropertyToID("_CenterY");

    //private Material _mat;


    private void Awake()
    {
        if (ins_ == null) { ins_ = this; }
        else { Destroy(gameObject); }


        _canvas = GetComponent<Canvas>();
        _black_screen = GetComponentInChildren<Image>();


        //_mat = _black_screen.material;
    }

    private void Start()
    {
        _black_screen.material.SetFloat(RADIUS, 1);
        //_mat.SetFloat(RADIUS, 1);


        player_screenPos = UnityEngine.Camera.main.WorldToScreenPoint(player.position);
        canvasRect = _canvas.GetComponent<RectTransform>().rect;

        canvasWitdth = canvasRect.width;
        canvasHeight = canvasRect.height;
    }



    //private void Update()
    //{
    //    //Debug.Log("\n canvas " + _player_Canvas_pos + "\n screen\n "
    //    //    + Camera.main.WorldToScreenPoint(player.position));


    //    //if (Input.GetKeyDown(KeyCode.L))
    //    //{
    //    //    Open_Black_screen();
    //    //}
    //    //else if (Input.GetKeyDown(KeyCode.K))
    //    //{
    //    //    Close_Black_screen();
    //    //}
    //}




    public void Close_Black_screen()
    {
        StartCoroutine(Fade_Circle(fade_dur, 1, 0));
    }

    public void Open_Black_screen()
    {
        StartCoroutine(Fade_Circle(fade_dur, 0, 1));
    }












    private void Death_Fading()
    {
        var player_screenPos = UnityEngine.Camera.main.WorldToScreenPoint(player.position);


        _player_Canvas_pos = new Vector2
        {
            x = (player_screenPos.x / screenWidth) * canvasWitdth,
            y = (player_screenPos.y / screenHeight) * canvasHeight
        };



        float squareValue;
        if (canvasWitdth > canvasHeight)
        {
            squareValue = canvasWitdth;
            _player_Canvas_pos.y += (canvasWitdth - canvasHeight) * .5f;
        }
        else
        {
            squareValue = canvasHeight;
            _player_Canvas_pos.x += (canvasHeight - canvasWitdth) * .5f;
        }

        _player_Canvas_pos /= squareValue;

        var mat = _black_screen.material;
        mat.SetFloat(CenterX, _player_Canvas_pos.x);
        mat.SetFloat(CenterY, _player_Canvas_pos.y);


        _black_screen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
    }






    private IEnumerator Fade_Circle(float duration, float start_rad, float end_rad)
    {
        float time = 0f;

        _black_screen.material.SetFloat(RADIUS, start_rad);
        //Debug.Log("baþlangýc " + start_rad);
        //_mat.SetFloat(RADIUS, start_rad);

        while (time <= duration)
        {

            Death_Fading();

            time += Time.fixedDeltaTime;
            var t = time / duration;
            var rad_ = Mathf.Lerp(start_rad, end_rad, t);
            rad_ = Math.Clamp(rad_, 0, 1);

            //Debug.Log("ara " +rad_);
            _black_screen.material.SetFloat(RADIUS, rad_);
            //_mat.SetFloat(RADIUS, rad_);

            yield return null;
        }

        //_mat.SetFloat(RADIUS, end_rad);
        _black_screen.material.SetFloat(RADIUS, end_rad);
        //Debug.Log("son " + end_rad);
    }
}
