using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class FightController : MonoBehaviour
{
	public enum SelectAbilityState
	{
		NONE,
		DAMAGE,
		SPECIAL
	}
	
	private const string YourTurnString = "Ваш ход\nПеерсонаж:{0}";

	private const string EnemyTurn = "Ход соперника";

	private const string Tip1 = "Выберете противника, по которому нанести урон";

	private const string WinText = "Вы победили";

	private const string LoseText = "Вы проиграли";

	public Action<bool> FightEndedEvent;
	
	[SerializeField] 
	private Transform[] _friendsSpots;

	[SerializeField] 
	private Transform[] _enemiesSpots;

	[SerializeField] 
	private GameObject[] _friendsPrefabs;

	[SerializeField] 
	private GameObject[] _enemiesPrefabs;

	[Header("UI")] 
	public GameObject Panel;

	public RandomWindow RandomWindow;
	
	public Text YourTurnText;

	public GameObject DamageButton;

	public Text TipText;

	public GameObject SpecialAbilityButton;

	public Text SpecialAbilityButtonText;


	private Queue<Fighter> _friendlyFighters;

	private Queue<Fighter> _enemyFighters;
	private bool _targetSelected;
	private int _generatedRandomNumber;
	private SelectAbilityState _abilitySelect;
	private Fighter _currentTurnFighter;
	private Fighter.FighterType _abilityFighterType;
	private Fighter _target;
	private List<GameObject> _destoyOnDisable;

	private void Awake()
	{
		AppController.FightController = this;
	}

	private void OnDisable()
	{
		if (_destoyOnDisable != null)
		{
			foreach (var obj in _destoyOnDisable)
			{
				Destroy(obj);
			}
		}
		
	}

	public void SetData(GameObject[] enemies)
	{
		_destoyOnDisable = new List<GameObject>();
		SpawnFighters(_friendsPrefabs, _enemiesPrefabs);
		_currentTurnFighter = _friendlyFighters.Dequeue();
		_friendlyFighters.Enqueue(_currentTurnFighter);
		Panel.SetActive(true);
		RandomWindow.gameObject.SetActive(false);
		UpdateUI();
		StartCoroutine(ProcessFight());
	}
	
	private void SpawnFighters(GameObject[] friends, GameObject[] enemy)
	{
		_friendlyFighters = new Queue<Fighter>();
		for (int i = 0; i < friends.Length; i++)
		{
			var friend = Instantiate(friends[i], _friendsSpots[i].position, Quaternion.identity);
			_destoyOnDisable.Add(friend);
			if (friend.TryGetComponent<Fighter>(out var fighter))
			{
				_friendlyFighters.Enqueue(fighter);
				fighter.DeathEvent += OnFighterDeathEvent;
				fighter.ClickEvent += OnFighterClickEvent;
			}
		}

		_enemyFighters = new Queue<Fighter>();
		for (int i = 0; i < enemy.Length; i++)
		{
			var en = Instantiate(enemy[i], _enemiesSpots[i].position, Quaternion.identity);
			_destoyOnDisable.Add(en);
			if (en.TryGetComponent<Fighter>(out var fighter))
			{
				_enemyFighters.Enqueue(fighter);
				fighter.DeathEvent += OnFighterDeathEvent;
				fighter.ClickEvent += OnFighterClickEvent;
			}
		}
	}

	private IEnumerator ProcessFight()
	{
		Random r = new Random();
		_generatedRandomNumber = -1;
		while (_enemyFighters.Count > 0 && _friendlyFighters.Count > 0)
		{
			if (_currentTurnFighter.GetFighterType() == Fighter.FighterType.FRIEND)
			{
				_abilityFighterType = Fighter.FighterType.ENEMY;
				yield return new WaitUntil(() => _abilitySelect != SelectAbilityState.NONE);
				
				DamageButton.SetActive(false);
				SpecialAbilityButton.SetActive(false);
				TipText.gameObject.SetActive(true);
				TipText.text = Tip1;
				
				if (_abilitySelect == SelectAbilityState.SPECIAL &&
				    _currentTurnFighter.GetSpecialAbilityType() == Fighter.AbilityType.HEAL)
				{
					_abilityFighterType = Fighter.FighterType.FRIEND;
				}
				
				
				yield return new WaitUntil(() => _target != null);
				
				ShowRandomWindow();
				yield return new WaitUntil(() => _generatedRandomNumber >= 0);
				yield return new WaitForSeconds(2f);
				HideRandomWindow();
				
				if(_abilitySelect == SelectAbilityState.DAMAGE)
					_currentTurnFighter.Damage(_target, _generatedRandomNumber);
				else if(_abilitySelect == SelectAbilityState.SPECIAL)
					_currentTurnFighter.UseSpecialAbility(_target, _generatedRandomNumber);

				_generatedRandomNumber = -1;
				_abilitySelect = SelectAbilityState.NONE;
				NextTurn();
			}
			else
			{
				_abilityFighterType = Fighter.FighterType.ENEMY;
				yield return new WaitForSeconds(1f);
				var fighterArray = _friendlyFighters.ToArray();
				_currentTurnFighter.Damage(fighterArray[r.Next(0, fighterArray.Length)], r.Next(_currentTurnFighter.DamageFrom, _currentTurnFighter.DamageTo));
				NextTurn();
			}
			
		}

		if (_enemyFighters.Count == 0)
		{
			TipText.text = WinText;
		}
		else
		{
			TipText.text = LoseText;
		}

		yield return new WaitForSeconds(3f);
		Panel.SetActive(false);
		AppController.SceneController.SwitchToOpenWorldScene();
		FightEndedEvent.Invoke(_enemyFighters.Count == 0);
		yield return true;
	}

	private void OnFighterDeathEvent(Fighter fighter)
	{
		if (fighter.GetFighterType() == Fighter.FighterType.FRIEND)
		{
			_friendlyFighters = new Queue<Fighter>(_friendlyFighters.Where(x => x != fighter));
		}
		else
		{
			_enemyFighters = new Queue<Fighter>(_enemyFighters.Where(x => x != fighter));
		}
	}

	private void NextTurn()
	{
		if(_enemyFighters.Count == 0 || _friendlyFighters.Count == 0) 
			return;
		
		if (_currentTurnFighter.GetFighterType() == Fighter.FighterType.ENEMY)
		{
			_currentTurnFighter = _friendlyFighters.Dequeue();
			_friendlyFighters.Enqueue(_currentTurnFighter);
		}
		else
		{
			_currentTurnFighter = _enemyFighters.Dequeue();
			_enemyFighters.Enqueue(_currentTurnFighter);
			_target = null;
		}
		
		UpdateUI();
	}

	private void UpdateUI()
	{
		if (_currentTurnFighter.GetFighterType() == Fighter.FighterType.FRIEND)
		{
			YourTurnText.gameObject.SetActive(true);
			YourTurnText.text = string.Format(YourTurnString, _currentTurnFighter.Name);
			TipText.gameObject.SetActive(false);
			DamageButton.SetActive(true);
			var specialAbilityAvailableUse = _currentTurnFighter.AvailableSpecialAbilityUse();
			if (specialAbilityAvailableUse > 0 && _currentTurnFighter.GetSpecialAbilityType() != Fighter.AbilityType.NONE)
			{
				SpecialAbilityButton.SetActive(true);
				SpecialAbilityButtonText.text = _currentTurnFighter.GetSpecialAbilityName() + " (" + specialAbilityAvailableUse+ ")";
			}
		}
		else
		{
			YourTurnText.gameObject.SetActive(false);
			DamageButton.gameObject.SetActive(false);
			SpecialAbilityButton.SetActive(false);
			TipText.gameObject.SetActive(true);
			TipText.text = EnemyTurn;
		}
	}

	private void ShowRandomWindow()
	{
		Panel.SetActive(false);
		RandomWindow.gameObject.SetActive(true);
		RandomWindow.SetData(_currentTurnFighter.DamageFrom, _currentTurnFighter.DamageTo);
		RandomWindow.RandomNumberGenerated += RandomNumberGenerated;
	}



	private void HideRandomWindow()
	{
		Panel.SetActive(true);
		RandomWindow.gameObject.SetActive(false);
		RandomWindow.RandomNumberGenerated -= RandomNumberGenerated;
	}

	private void OnFighterClickEvent(Fighter fighter)
	{
		_target = fighter;
	}

	public Fighter[] GetAllEnemyFighters()
	{
		return _enemyFighters.ToArray();
	}
	
	private void RandomNumberGenerated(int number)
	{
		_generatedRandomNumber = number;
	}

	public Fighter.FighterType GetCurrentAbilityFighterType()
	{
		return _abilityFighterType;
	}

	public void OnDamageButtonClick()
	{
		_abilitySelect = SelectAbilityState.DAMAGE;
	}

	public void OnSpecialAbilityClick()
	{
		_abilitySelect = SelectAbilityState.SPECIAL;
	}
}
