using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 地面 （まだ見ないで >< ）
/// </summary>
public class Ground : MonoBehaviour
{
    private const string MaterialColorPropertyName = "_BaseColor";

    [SerializeField]
    private MeshRenderer _renderer;

    private MaterialPropertyBlock _mpb;

    private void Awake()
    {
        _mpb = new MaterialPropertyBlock();
        _renderer.SetPropertyBlock(_mpb);
    }

    public void SetMaterialColor(Color color)
    {
        _mpb.SetColor(MaterialColorPropertyName, color);
    }
}
