using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace NewPlay.ArcadeIdle
{
    public class FighterBaseController : RoleController
    {
        public CampType CampType = CampType.Enemy;

        public AIHealth Health { get; protected set; }

        protected HpBar m_HpBar;
        protected HpBar HpBar
        {
            get
            {
                if (m_HpBar == null)
                {
                    m_HpBar = RestaurantManager.Instance.CreateHpBar();
                    m_HpBar.Target = transform;
                }
                return m_HpBar;
            }
        }

        public virtual void TakeDamage(float damage)
        {
            Health?.TakeDamage(damage);
            HpBar.SetHp(transform, Health.CurrentHP, Health.MaxHP);
        }
    }
}
