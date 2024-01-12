using System;
using UnityEngine;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    /// <summary>
    /// Définition d'un personnage
    /// </summary>
    public class Character
    {
        /// <summary>
        /// Stat de base, HP
        /// </summary>
        int _baseHealth;
        /// <summary>
        /// Stat de base, ATK
        /// </summary>
        int _baseAttack;
        /// <summary>
        /// Stat de base, DEF
        /// </summary>
        int _baseDefense;
        /// <summary>
        /// Stat de base, SPE
        /// </summary>
        int _baseSpeed;
        /// <summary>
        /// Type de base
        /// </summary>
        TYPE _baseType;

        public Character(int baseHealth, int baseAttack, int baseDefense, int baseSpeed, TYPE baseType)
        {
            _baseHealth = baseHealth;
            _baseAttack = baseAttack;
            _baseDefense = baseDefense;
            _baseSpeed = baseSpeed;
            _baseType = baseType;

            MaxHealth = _baseHealth;
            CurrentHealth = _baseHealth;
            CurrentStatus = null;
        }
        /// <summary>
        /// HP actuel du personnage
        /// </summary>
        int _currentHealth;
        public int CurrentHealth { get => _currentHealth; private set
            {
                _currentHealth = value;
                _currentHealth = Mathf.Clamp(_currentHealth, 0, MaxHealth);
            }
        }
        public TYPE BaseType { get => _baseType;}
        /// <summary>
        /// HPMax, prendre en compte base et equipement potentiel
        /// </summary>
        int _maxHealth;
        public int MaxHealth
        {
            get
            {
                if(CurrentEquipment == null)
                {
                    return _maxHealth;
                }

                return _maxHealth + CurrentEquipment.BonusHealth;
            }

            set
            {
                _maxHealth = value;

                if (CurrentHealth > MaxHealth)
                {
                    CurrentHealth = MaxHealth;
                }
            }
        }
        /// <summary>
        /// ATK, prendre en compte base et equipement potentiel
        /// </summary>
        public int Attack
        {
            get
            {
                if (CurrentEquipment == null)
                {
                    return _baseAttack;
                }

                return _baseAttack + CurrentEquipment.BonusAttack;
            }
        }
        /// <summary>
        /// DEF, prendre en compte base et equipement potentiel
        /// </summary>
        public int Defense
        {
            get
            {
                if (CurrentEquipment == null)
                {
                    return _baseDefense;
                }

                return _baseDefense + CurrentEquipment.BonusDefense;
            }
        }
        /// <summary>
        /// SPE, prendre en compte base et equipement potentiel
        /// </summary>
        public int Speed
        {
            get
            {
                if (CurrentEquipment == null)
                {
                    return _baseSpeed;
                }

                return _baseSpeed + CurrentEquipment.BonusSpeed;
            }
        }
        /// <summary>
        /// Equipement unique du personnage
        /// </summary>
        public Equipment CurrentEquipment { get; private set; }
        /// <summary>
        /// null si pas de status
        /// </summary>
        public StatusEffect CurrentStatus { get; private set; }

        public bool IsAlive => CurrentHealth > 0;


        /// <summary>
        /// Application d'un skill contre le personnage
        /// On pourrait potentiellement avoir besoin de connaitre le personnage attaquant,
        /// Vous pouvez adapter au besoin
        /// </summary>
        /// <param name="s">skill attaquant</param>
        /// <exception cref="NotImplementedException"></exception>
        public void ReceiveAttack(Skill s)
        {
            if(s == null)
                throw new ArgumentNullException("No skill to recieve damage !");

            int finalAttack = s.Power - Defense;
            if (finalAttack < 0) // could heal the character
            {
                finalAttack = 0;
            }

            CurrentHealth -= finalAttack;
            CurrentStatus = StatusEffect.GetNewStatusEffect(s.Status);
        }
        /// <summary>
        /// Ajoute une certaine quantité de vie au personnage
        /// </summary>
        public void Heal(int amount)
        {
            if(amount < 0)
            {
                throw new ArgumentException("heal amount can't be negative !");
            }

            CurrentHealth += amount;
        }

        /// <summary>
        /// Equipe un objet au personnage
        /// </summary>
        /// <param name="newEquipment">equipement a appliquer</param>
        /// <exception cref="ArgumentNullException">Si equipement est null</exception>
        public void Equip(Equipment newEquipment)
        {
            if(newEquipment == null)
            {
                throw new ArgumentNullException("No equipement to equip !");
            }

            CurrentEquipment = newEquipment;
        }
        /// <summary>
        /// Desequipe l'objet en cours au personnage
        /// </summary>
        public void Unequip()
        {
            CurrentEquipment = null;
        }

        public void AttackSelf()
        {
            if(CurrentStatus == null)
                throw new Exception("Current status is null");

            if ((CurrentStatus.DamageOnAttack <= 0.0f) && (CurrentStatus.DamageOnAttack > 1.0f))
                throw new Exception("Current status doesn't have a correct DamageOnAttack value");


            CurrentHealth -= Mathf.RoundToInt(Attack * CurrentStatus.DamageOnAttack);
        }

        public void EndTurn()
        {
            if (CurrentStatus == null)
                return;

            CurrentStatus.EndTurn();
            if (CurrentStatus.RemainingTurn == 0)
            {
                CurrentStatus = null;
            }

            //Check Burn Effect
            if (CurrentStatus is BurnStatus)
            {
                CurrentHealth -= CurrentStatus.DamageEachTurn;
            }
        }
    }
}
