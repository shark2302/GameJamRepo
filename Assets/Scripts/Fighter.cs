using System;
using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
	public class Fighter : MonoBehaviour, IPointerDownHandler
	{
		private const float DamageToAllMultiplier = 0.3f;
		
		public Action<Fighter> DeathEvent;
		public Action<Fighter> ClickEvent;
		public enum FighterType
		{
			FRIEND,
			ENEMY
		}
		
		public enum AbilityType
		{
			NONE,
			HEAL,
			DAMAGE_TO_ALL
		}

		[Serializable]
		public class AbilityInfo
		{
			public AbilityType AbilityType;
			public string AbilityName;
			public int UseCount;
		}

		public string Name;

		public int DamageFrom;

		public int DamageTo;

		[SerializeField] 
		private float _maxHitPoints;
		
		[SerializeField] 
		private FighterType _fighterType;

		[SerializeField] 
		private AbilityInfo _specialAbility;

		private Animator _animator;

		[SerializeField] 
		private Text _hpText;
		
		private float _hitPoints;

		private int _specialAbilityUse;

		private void OnEnable()
		{
			_hitPoints = _maxHitPoints;
			_hpText.text = _hitPoints + "/" + _maxHitPoints;
			_animator = GetComponent<Animator>();
		}


		public void Damage(Fighter target, int damageValue)
		{
			target.ChangeHitPoints(-damageValue);
			PlayAttackAnimation();
		}

		public void Heal(Fighter target, int healValue)
		{
			if (target.GetFighterType() == _fighterType)
			{
				target.ChangeHitPoints(healValue);
			}
		}

		public void ChangeHitPoints(int value)
		{
			_hitPoints += value;
			if (_hitPoints > _maxHitPoints)
			{
				_hitPoints = _maxHitPoints;
			}

			if (_hitPoints < 0)
			{
				_hitPoints = 0;
			}

			_hpText.text = _hitPoints + "/" + _maxHitPoints;
			
			if (_hitPoints <= 0)
			{
				DeathEvent?.Invoke(this);
			}
		}

		public void UseSpecialAbility(Fighter target, int value)
		{
			if (_specialAbility.AbilityType == AbilityType.HEAL)
			{
				Heal(target, value);
			}
			else if (_specialAbility.AbilityType == AbilityType.DAMAGE_TO_ALL)
			{
				DamageTargets(AppController.FightController.GetAllEnemyFighters(), value + 1);
			}

			_specialAbilityUse++;
		}

		public void DamageTargets(Fighter[] targets, int value)
		{
			foreach (var target in targets)
			{
				Damage(target, (int)(value * DamageToAllMultiplier));
			}
		}

		public int AvailableSpecialAbilityUse()
		{
			return _specialAbility.UseCount - _specialAbilityUse;
		}

		public FighterType GetFighterType()
		{
			return _fighterType;
		}

		public string GetSpecialAbilityName()
		{
			return _specialAbility.AbilityName;
		}

		public AbilityType GetSpecialAbilityType()
		{
			return _specialAbility.AbilityType;
		}

		private void PlayAttackAnimation()
		{
			if (_animator != null)
			{
				_animator.SetBool("Attack", true);
				StartCoroutine(StopAttackAnimatioAfterDelay(1f));
			}
		}

		private IEnumerator StopAttackAnimatioAfterDelay(float delay)
		{
			yield return new WaitForSeconds(delay);
			_animator.SetBool("Attack", false);
		}
		
		public void OnPointerDown(PointerEventData eventData)
		{
			if (AppController.FightController.GetCurrentAbilityFighterType() == _fighterType && _hitPoints > 0)
			{
				ClickEvent?.Invoke(this);
			}
		}
	}
}