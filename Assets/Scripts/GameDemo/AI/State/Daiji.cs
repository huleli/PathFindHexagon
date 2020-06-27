﻿using PathFind;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Daiji : GSNPCStateRemote
{
    //不移动 不追击 被动防守
    GameEntity entity;

    bool inAttackSight = false;
    public void EvalRule()
    {
        if(!inAttackSight)
        {
            entity.AimAtTargetEntity(null);
        }
    }

    public void ExecuteAction()
    {
    }

    public void GainFocus(GameEntity npc)
    {
        this.entity = npc;
    }

    public void LoseFocus()
    {
    }

    public async Task ExecuteActionAsync()
    {
        if (inAttackSight && entity.PAttack())
        {
            entity.DoAttack();
        }
    }

    public void UpdateSensor()
    {
        inAttackSight = entity.IsTargetEntityInAttackSight();
    }

    public void SendCmd(int fromID, Command msg, string arg)
    {
        if (msg == Command.CaughtDamage)
        {
            //如果在不移动的情况下可以攻击就还击
            GameEntity fromEntity = GameCore.GetRegistServices<GameEntityMgr>().GetGameEntity(fromID);
            if (fromEntity != null && fromEntity != entity.GetTargetEntity()
              && entity.IsEntityInAttackSight(fromEntity))
            {
                bool donotmove = false;
                entity.AimAtTargetEntity(fromEntity, donotmove);
            }
        }
    }
}