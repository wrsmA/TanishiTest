using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* 各種説明
 *
 * インスペクタとは
 * エディターの操作ウィンドウの一種
 *
 * [SerializeField] とは
 * フィールド（変数）をインスペクタ（エディター）から設定できるようにするためのおまじない
 *
 * [Serializable] とは
 * 自作したクラスや構造体を[SerializeField]できるようにするためのおまじない
 *
 * [Header] とは
 * インスペクタに表示できる注釈をつけるおまじない
 */

/// <summary>
/// グリッドの行列数クラス
/// </summary>
[Serializable]
public class GridSettings
{
    [SerializeField, Header("X軸に並べる数")]
    private int _x;
    public int GridX => _x;

    [SerializeField, Header("Y軸に並べる数")]
    private int _y;
    public int GridY => _y;

    [SerializeField, Header("１グリッド当たりの大きさ")]
    private Vector2 _gridSize;
    public Vector2 GridSize => _gridSize;

    public int GridCount => _x * _y;
}

/// <summary>
/// 地面を並べる方向
/// 例：TopLeft = 左上から右下へ向かって並べる
/// </summary>
public enum PlacementType
{
    TopLeft = 0,
    TopRight,
    BottomLeft,
    BottomRight
}

/// <summary>
/// グリッドマップクラス
/// </summary>
public class Game : MonoBehaviour
{
    [SerializeField, Header("グリッド")]
    private GridSettings _gridSettings;

    [SerializeField, Header("地面を並べる方向")]
    private PlacementType _placementType;

    [SerializeField, Header("地面プレハブ")]
    private Ground _groundPrefab;

    [SerializeField, Header("プレイヤープレハブ")]
    private Player _playerPrefab;

    private Ground[,] _groundObjects;

    // Start is called before the first frame update
    void Start()
    {
        // 地面を作る
        _groundObjects = new Ground[_gridSettings.GridX, _gridSettings.GridY];
        CreateGround();

        // プレイヤーを作る
        var player = Instantiate(_playerPrefab);
        player.SetGridSize(_gridSettings.GridSize);
        player.transform.position = Vector3.zero;
    }

    /// <summary>
    /// 地面を作る
    /// </summary>
    private void CreateGround()
    {
        Vector2 size = _gridSettings.GridSize;
        Vector2 max = new Vector2(_gridSettings.GridX * size.x, _gridSettings.GridY * size.y) * 0.5F - size * 0.5F;
        Vector2 min = -max;

        for ( int x = 0; x < _gridSettings.GridX; ++x )
        {
            for ( int y = 0; y < _gridSettings.GridY; ++y )
            {
                // 地面インスタンス生成
                Ground ground = Instantiate(_groundPrefab);
                ground.name = $"Ground[{x + 1},{y + 1}]";
                Vector2 position = _placementType switch
                {
                    PlacementType.TopLeft     => new Vector2(min.x + x * size.x, max.y - y * size.y),
                    PlacementType.TopRight    => new Vector2(max.x - x * size.x, max.y - y * size.y),
                    PlacementType.BottomLeft  => new Vector2(min.x + x * size.x, min.y + y * size.y),
                    PlacementType.BottomRight => new Vector2(max.x - x * size.x, min.y + y * size.y),
                    _ => Vector2.zero
                };
                ground.transform.parent = transform;
                ground.transform.position = new Vector3(position.x, 0, position.y);
                _groundObjects[x, y] = ground;
            }
        }
    }
}
