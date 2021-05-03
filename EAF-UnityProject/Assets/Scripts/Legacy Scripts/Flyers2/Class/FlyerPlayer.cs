using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyerPlayer : Flyer
{
    protected float rotationZ;

    public int EasyDifHealth;
    public int MiddleDifHealth;
    public int HardDifHealth;

    protected GameObject DeathPanelUIObject;

    protected GameObject HealthBarUIObject;
    protected HealthBar HealthBarScript;

    protected override void Awake()
    {
        base.Awake();

        DeathPanelUIObject = GameObject.Find("Death panel");
    }

    protected override void Start()
    {
        base.Start();

        if (DifficultyLevels.DifficultyLevel < 3)
        {
            HealthBarUIObject = GameObject.Find("Health bar");
            HealthBarScript = HealthBarUIObject.GetComponent<HealthBar>();
        }

        if (DifficultyLevels.DifficultyLevel == 1) { Health = EasyDifHealth; HealthBarScript.SetMaxHealth(Health); }
        if (DifficultyLevels.DifficultyLevel == 2) { Health = MiddleDifHealth; HealthBarScript.SetMaxHealth(Health); }
        if (DifficultyLevels.DifficultyLevel == 3) { Health = HardDifHealth; }
    }

    protected override void FixedUpdate()
    {
        BorderControl();

        if(needFlyToCenter)
        {
            FlyToVector(CentralMapPoint);
        }
    }

    public override void SetDamage(int damage, string Type)
    {
        Health -= damage;
        HitVisual(Type);
        if (DifficultyLevels.DifficultyLevel < 3) { HealthBarScript.SetHealth(Health); }
        if (Health <= 0)
        {
            SetDeathSettings();
            DeathVisual();
        }
    }

    public override void SetDeathSettings()
    {
        Alive = false;
        AvailableAsTarget = false;
        bcollider.enabled = false;
        rb.constraints = RigidbodyConstraints.None;
        Invoke("DeactivateGameObject", 3f);
        DeathPanelUIObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
