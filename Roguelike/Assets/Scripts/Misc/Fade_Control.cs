using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade_Control : MonoBehaviour
{
    [SerializeField] private LayerMask layer_mask;
    [SerializeField] private Transform target;
    [SerializeField] private UnityEngine.Camera cam;
    [SerializeField] private bool retainShadows = true;
    [SerializeField] private Vector3 target_Pos_offset = Vector3.up;
    [SerializeField][Range(0, 1)] private float faded_alpha;
    [SerializeField] private float fade_Speed, sphereRadius;




    [Header("Read Only Data")]
    [SerializeField] private List<Object_Fade> object_block = new();
    [SerializeField] private Dictionary<Object_Fade, Coroutine> run_coroutines = new();

    private RaycastHit[] hits_ = new RaycastHit[10]; //kaç tane obje ise



    private void Awake()
    {
        //cam = GameObject.FindGameObjectWithTag("MainCamera");
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        StartCoroutine(Check_Objects());
    }


















    #region Funcs




    private IEnumerator Check_Objects()
    {
        while (true)
        {
            Vector3 direction = (target.transform.position + target_Pos_offset - cam.transform.position).normalized;
            float distance = Vector3.Distance(cam.transform.position, target.transform.position + target_Pos_offset);

            int hits = Physics.SphereCastNonAlloc(cam.transform.position, sphereRadius, direction, hits_, distance, layer_mask);

            Debug.DrawLine(cam.transform.position, target.transform.position + target_Pos_offset, Color.cyan);

            if (hits > 0)
            {
                Debug.DrawLine(cam.transform.position, target.transform.position + target_Pos_offset, Color.magenta);

                for (int i = 0; i < hits; i++)
                {
                    Object_Fade fade_object = Get_Fade_objects(hits_[i]);

                    if (fade_object != null && !object_block.Contains(fade_object))
                    {
                        if (run_coroutines.ContainsKey(fade_object))
                        {
                            if (run_coroutines[fade_object] != null)
                            {
                                StopCoroutine(run_coroutines[fade_object]);
                            }
                            run_coroutines.Remove(fade_object);
                        }
                        run_coroutines.Add(fade_object, StartCoroutine(Fade_object_out(fade_object)));
                        object_block.Add(fade_object);
                    }
                }
            }

            Fade_object_non();
            Clear_hits();

            yield return null;
        }

    }











    private IEnumerator Fade_object_in(Object_Fade fdi_obj)
    {

        float time_ = 0;

        while (fdi_obj.materials_[0].color.a < fdi_obj.int_alpha)
        {
            foreach (Material _mat in fdi_obj.materials_)
            {
                if (_mat.HasProperty("_Color"))
                {
                    _mat.color = new(_mat.color.r, _mat.color.g, _mat.color.b,
                        Mathf.Lerp(faded_alpha, fdi_obj.int_alpha, time_ * fade_Speed));
                }
            }
            time_ += Time.deltaTime;
            yield return null;
        }


        foreach (Material mat in fdi_obj.materials_)
        {
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.SetInt("_Surface", 0);

            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

            mat.SetShaderPassEnabled("DepthOnly", true);
            mat.SetShaderPassEnabled("SHADOWCASTER", true);

            mat.SetOverrideTag("RenderType", "Opaque");

            mat.DisableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        }


        if (run_coroutines.ContainsKey(fdi_obj))
        {
            StopCoroutine(run_coroutines[fdi_obj]);
            run_coroutines.Remove(fdi_obj);
        }
    }










    private IEnumerator Fade_object_out(Object_Fade fd_obj)
    {
        foreach (Material mat in fd_obj.materials_)
        {
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_Surface", 1);

            mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            mat.SetShaderPassEnabled("DepthOnly", false);
            mat.SetShaderPassEnabled("SHADOWCASTER", retainShadows);

            mat.SetOverrideTag("RenderType", "Transparent");

            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        }
        float time_ = 0;

        while (fd_obj.materials_[0].color.a > faded_alpha)
        {
            foreach (Material _mat in fd_obj.materials_)
            {
                if (_mat.HasProperty("_Color"))
                {
                    _mat.color = new(_mat.color.r, _mat.color.g, _mat.color.b,
                        Mathf.Lerp(fd_obj.int_alpha, faded_alpha, time_ * fade_Speed));
                }
            }
            time_ += Time.deltaTime;
            yield return null;
        }


        if (run_coroutines.ContainsKey(fd_obj)) ////////// þüpheli
        {
            StopCoroutine(run_coroutines[fd_obj]);
            run_coroutines.Remove(fd_obj);
        }

    }








    private void Fade_object_non()
    {
        List<Object_Fade> obj_remove = new(object_block.Count);

        foreach (Object_Fade fd_obj_ in object_block)
        {
            bool obj_hit = false;

            for (int i = 0; i < hits_.Length; i++)
            {
                Object_Fade hit_fd_obj = Get_Fade_objects(hits_[i]);

                if (hit_fd_obj != null && fd_obj_ == hit_fd_obj)
                {
                    obj_hit = true; break;
                }
            }

            if (!obj_hit)
            {
                if (run_coroutines.ContainsKey(fd_obj_))
                {
                    if (run_coroutines[fd_obj_] != null)
                    {
                        StopCoroutine(run_coroutines[fd_obj_]);
                    }
                    run_coroutines.Remove(fd_obj_);
                }
                run_coroutines.Add(fd_obj_, StartCoroutine(Fade_object_in(fd_obj_)));
                obj_remove.Add(fd_obj_);
            }
        }
        foreach (Object_Fade remove_obj in obj_remove)
        {
            object_block.Remove(remove_obj);
        }
    }



    #endregion




    private void Clear_hits()
    {
        System.Array.Clear(hits_, 0, hits_.Length);
    }






    private Object_Fade Get_Fade_objects(RaycastHit hit)
    {
        return hit.collider != null ? hit.collider.GetComponent<Object_Fade>() : null;
    }
}
