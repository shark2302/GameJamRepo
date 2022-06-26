using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DefaultNamespace
{
	public class Fighter : MonoBehaviour, IPointerDownHandler
	{
		
		
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

		public string ColorCode;

		public int DamageFrom;

		public int DamageTo;
		
		public float DamageToAllMultiplier = 0.3f;

		public Sprite[] DropAnimationSprite;

		[SerializeField] 
		private float _maxHitPoints;
		
		[SerializeField] 
		private FighterType _fighterType;

		[SerializeField] 
		private AbilityInfo _specialAbility;
		
		public bool UseSpecialAbilityOnly;

		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[SerializeField] 
		private GameObject _bulletPrefab;

		[SerializeField] 
		private Transform _bulletStart;

		private Animator _animator;
		

		[SerializeField] 
		private Text _hpText;
		
		private float _hitPoints;

		private int _specialAbilityUse;

		private bool _playDamageAnimation;

		private void OnEnable()
		{
			_hitPoints = _maxHitPoints;
			_hpText.text = _hitPoints + "/" + _maxHitPoints;
			_animator = GetComponent<Animator>();
		}


		public void Damage(Fighter target, int damageValue)
		{
			if (damageValue > 0)
			{
				target.ChangeHitPoints(-damageValue);
				PlayAttackAnimation(target);
			}
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
			if (value > 0)
			{
				foreach (var target in targets)
				{
					Damage(target, (int)(value * DamageToAllMultiplier));
				}
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

		private void PlayAttackAnimation(Fighter target)
		{
			if (_animator != null)
			{
				_animator.SetBool("Attack", true);
				StartCoroutine(StopAttackAnimatioAfterDelay(_animator.GetCurrentAnimatorStateInfo(0).length, target));
			}
		}

		private IEnumerator StopAttackAnimatioAfterDelay(float delay, Fighter target)
		{
			yield return new WaitForSeconds(delay);
			CreateBullet(target);
			if (_bulletPrefab == null && target != null)
			{
				target.PlayDamagedAnimation();
			}
			_animator.SetBool("Attack", false);
		}
		
		public void OnPointerDown(PointerEventData eventData)
		{
			if (AppController.FightController.GetCurrentAbilityFighterType() == _fighterType && _hitPoints > 0)
			{
				ClickEvent?.Invoke(this);
			}
		}

		public void PlayDamagedAnimation()
		{
			StartCoroutine(DamageAnimation());
		}

		private void CreateBullet(Fighter target)
		{
			if (_bulletPrefab != null && target != null)
			{
				var bullet = Instantiate(_bulletPrefab, _bulletStart.position, Quaternion.identity);
				if (bullet.TryGetComponent<Bullet>(out var b))
				{
					b.SetTarget(target);
				}
			}
		}
		private IEnumerator DamageAnimation()
		{
			
			for (int i = 0; i < 5; i++)
			{
				yield return new WaitForSeconds(0.1f);
				_spriteRenderer.color = Color.red;
				yield return new WaitForSeconds(0.1f);
				_spriteRenderer.color = Color.white;
			}
			
		}
		
	}
}