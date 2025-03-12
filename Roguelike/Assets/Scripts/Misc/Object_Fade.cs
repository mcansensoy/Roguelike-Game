using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Fade : MonoBehaviour, IEquatable<Object_Fade>
{

    [SerializeField] Vector3 pos_;


    public List<Renderer> renderers_ = new();
    public List<Material> materials_ = new();

    public float int_alpha;



    private void Start()
    {
        pos_ = transform.position;
        if (renderers_.Count == 0) { renderers_.AddRange(GetComponentsInChildren<Renderer>()); }

        foreach (Renderer renderer in renderers_) { materials_.AddRange(renderer.materials); }

        int_alpha = materials_[0].color.a;
    }


    public bool Equals(Object_Fade other)
    {
        return pos_.Equals(other.pos_);
    }

    public override int GetHashCode()
    {
        return pos_.GetHashCode();
    }

}
