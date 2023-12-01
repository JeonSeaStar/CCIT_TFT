using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolf : Piece
{
    [SerializeField] GameObject effectOne;
    [SerializeField] GameObject effectTwo;
    [SerializeField] GameObject effectThree;
    [SerializeField] GameObject[] Settings;
    [SerializeField] WereWolfSkill[] wereWolfSkills;

    //토끼 시너지 처럼 뒤로 가는 부분 있어야 함
    public override IEnumerator Attack()
    {
        if (mana >= 100 && target != null)
        {
            StartSkill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartNextBehavior();
        }
        else
        {
            DoAttack();
        }
    }

    public override IEnumerator Skill()
    {
        Attackkill(200f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void Attackkill(float damage)
    {
        if (target != null)
        {
            Damage(damage);
        }
    }

    public void SetSkillOne()
    {
        Settings[0].SetActive(true);
        wereWolfSkills[0].damage = 200f;
        wereWolfSkills[0].effect = effectOne;
    }

    public void SetSkillTwo()
    {
        Settings[1].SetActive(true);
        wereWolfSkills[1].damage = 300f;
        wereWolfSkills[1].effect = effectTwo;
    }

    public void SetSkillThree()
    {
        Settings[2].SetActive(true);
        wereWolfSkills[2].damage = 500f;
        wereWolfSkills[2].effect = effectThree;
    }

    public void SetOffSkill()
    {
        for(int i = 0; i < Settings.Length; i++)
        {
            Settings[i].SetActive(false);
        }
    }
}
