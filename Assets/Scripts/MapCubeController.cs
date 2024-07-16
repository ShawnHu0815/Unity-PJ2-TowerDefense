using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCubeController : MonoBehaviour
{   
    [HideInInspector] // 在Inspector面板中隐藏
    public bool isUpgraded = false; // 是否已经升级
    
    public TurretData turretData;
    [HideInInspector]
    public GameObject turretGo; // 保存当前Cube身上的炮台
    public GameObject buildEffect; // 建造炮台的特效
    private Renderer renderer; // 保存Cube的Renderer组件

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void BuildTurret(TurretData turretData)
    {
        this.turretData = turretData;
        // 在Cube上建造炮台
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        isUpgraded = false;
        // 创建炮台特效，释放特效后销毁
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    public void UpgradeTurret()
    {
        Destroy(turretGo);
        // 在Cube上建造炮台
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        isUpgraded = true;
        // 创建炮台特效，释放特效后销毁
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }

    public void DestroyTurret()
    {
        Destroy(turretGo);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
        // 创建炮台特效，释放特效后销毁
        GameObject effect = GameObject.Instantiate(buildEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1.5f);
    }
    
    private void OnMouseEnter()
    {
        if (turretGo == null &&  !EventSystem.current.IsPointerOverGameObject())
        {
            renderer.material.color = Color.red;
        }
    }

    private void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}
