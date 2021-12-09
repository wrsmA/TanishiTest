using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー設定クラス
/// </summary>
[Serializable]
public class PlayerSettings
{
    // 移動速度
    [SerializeField, Header("移動速度")]
    private float _speed;
    public float Speed => _speed;

    // ジャンプ高さ
    [SerializeField, Header("ジャンプの高さ")]
    private float _jumpHeight;
    public float JumpHeight => _jumpHeight;
}

public class Player : MonoBehaviour
{
    // プレイヤー設定
    [SerializeField, Header("プレイヤー設定")]
    private PlayerSettings _playerSettings;

    // 1グリッドの大きさ≒プレイヤーの歩幅
    private Vector2 _gridSize;

    // 移動方向
    private Vector2 _direction;

    private void Update()
    {
        _direction = Vector2.zero;


        /* Inputクラス：入力の取得
         * GetKey()     = 押しているか？
         * GetKeyDown() = そのフレームで押したか？
         * GetKeyUp()   = そのフレームで離したか？
         */
        bool up    = Input.GetKeyDown(KeyCode.UpArrow);
        bool down  = Input.GetKeyDown(KeyCode.DownArrow);
        bool right = Input.GetKeyDown(KeyCode.RightArrow);
        bool left  = Input.GetKeyDown(KeyCode.LeftArrow);

        if (up)
        {
            _direction.y = 1.0F;
        }
        else if (down)
        {
            _direction.y = -1.0F;
        }

        if (right)
        {
            _direction.x = 1.0F;
        }
        else if (left)
        {
            _direction.x = -1.0F;
        }

        if (up || down || right || left)
        {
            Step(_direction);
        }
    }

    public void SetGridSize(Vector2 gridSize)
    {
        _gridSize = gridSize;
    }

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="direction"></param>
    private void Step(Vector2 direction)
    {
        StartCoroutine(StepCoroutine(direction));
    }

    /// <summary>
    /// 移動コルーチン
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator StepCoroutine(Vector2 direction)
    {
        float collapse = 0;

        Vector2 origin = new Vector2(transform.position.x, transform.position.z);
        Vector2 moveto = origin + direction * _gridSize;

        float jumporigin = 0;
        float jumphalf = jumporigin + _playerSettings.JumpHeight;

        while (true)
        {
            // 移動処理

            // X軸
            float x = Mathf.Lerp(origin.x, moveto.x, collapse);

            // Y軸 (ベジエ計算）
            float p1 = Mathf.Lerp(jumporigin, jumphalf, collapse);
            float p2 = Mathf.Lerp(jumphalf, jumporigin, collapse);
            float y  = Mathf.Lerp(p1, p2, collapse);

            // Z軸
            float z = Mathf.Lerp(origin.y, moveto.y, collapse);

            // 合算
            transform.position = new Vector3(x, y, z);

            if (collapse >= 1.0F)
            {
                break;
            }

            collapse += _playerSettings.Speed * Time.deltaTime;
            yield return null;
        }
    }
}
