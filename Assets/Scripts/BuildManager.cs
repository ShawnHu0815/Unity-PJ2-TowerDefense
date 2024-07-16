using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{   
    public TurretData laserTurretData;
    public TurretData missileTurretData;
    public TurretData standardTurretData;
    
    // 当前选择要建造的炮塔
    private TurretData selectedTurretData;
    // 当前选择的场景中的方块
    private MapCubeController selectedMapCube;
    
    public Text moneyText;
    public int money = 1000;
    public Animator moneyAnimator;
    
    public GameObject upgradeCanvas;
    private Animator upgradeCansvasAnimator;
    public Button buttonUpgrade;

    private void Start()
    {
        upgradeCansvasAnimator = upgradeCanvas.GetComponent<Animator>();
    }

    private void ChangeMoney(int change = 0)
    {
        money += change;
        moneyText.text = "余额￥" + money;
    }
    
    
    private void Update()
    {   
        // 鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {   
            // 检测是否点击在UI上
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                // 建造炮台的逻辑
                // 构造一个射线，从摄像机发射到鼠标点击的位置
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                // 射线与MapCube层发生碰撞，返回碰撞信息
                RaycastHit hit;
                bool isCollider = Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("MapCube"));
                if (isCollider)
                {
                    MapCubeController mapCube = hit.collider.GetComponent<MapCubeController>(); // 获取碰撞到的cube，即鼠标点击的cube
                    // 若当前cube上没有炮台，并且当前选中了炮台，则可以建造炮台
                    if (selectedTurretData != null && mapCube.turretGo == null)
                    {
                        // 若当前钱足够
                        if (money > selectedTurretData.cost)
                        {  
                            mapCube.BuildTurret(selectedTurretData);
                            ChangeMoney(-selectedTurretData.cost);
                        }
                        // 若当前钱不够
                        else 
                        {
                            moneyAnimator.SetTrigger("flicker");
                        }
                    }
                    // 若当前cube上有炮台
                    else if(mapCube.turretGo != null)
                    {   
                        // 若当前cube是选中的cube，并且升级面板是激活状态
                        if (mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy)
                        {
                            // 关闭升级面板
                            StartCoroutine(HideUpgradeUI());
                        }
                        else
                        {   
                            // 显示升级面板
                            ShowUpgradeUI(mapCube.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;
                    }
                }
                
            }
            
        }
    }

    public void OnLaserSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }
    
    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = missileTurretData;
        }
    }
    
    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = standardTurretData;
        }
    }
    
    // 显示升级面板
    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrade = false)
    {
        StopCoroutine("HideUpgradeUI");
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos + new Vector3(0,3,1);
        buttonUpgrade.interactable = !isDisableUpgrade;
    }

    // 隐藏升级面板
    IEnumerator HideUpgradeUI()
    {
        upgradeCansvasAnimator.SetTrigger("hide");
        yield return new WaitForSeconds(0.5f);
        upgradeCanvas.SetActive(false);
    }

    public void OnUpgradeButtonDown()
    {
        if (money >= selectedMapCube.turretData.costUpgraded)
        {
            ChangeMoney(-selectedMapCube.turretData.costUpgraded);
            selectedMapCube.UpgradeTurret();
        }
        else
        {
            moneyAnimator.SetTrigger("flicker");
        }
        StartCoroutine(HideUpgradeUI());
    }
    
    public void OnDestoryButtonDown()
    {
        selectedMapCube.DestroyTurret();
        StartCoroutine(HideUpgradeUI());
    }
}
