
using System;
using UnityEngine;

namespace _2023_GC_A2_Partiel_POO.Level_2
{
    public class Fight
    {
        //Pour savoir qui doit attaquer, dans quel ordre
        private enum FightOrder { CHARACTER1, CHARACTER2 }
        private FightOrder _currentOrder;

        public Fight(Character character1, Character character2)
        {
            if(character1 == null || character2 == null)
            {
                throw new ArgumentNullException("Miss one or both fighter");
            }

            Character1 = character1;
            Character2 = character2;
        }

        public Character Character1 { get; }
        public Character Character2 { get; }
        /// <summary>
        /// Est-ce la condition de victoire/défaite a été rencontré ?
        /// </summary>
        public bool IsFightFinished => (Character1.CurrentHealth == 0 || Character2.CurrentHealth == 0);

        public event Action OnEndTurn;

        /// <summary>
        /// Jouer l'enchainement des attaques. Attention à bien gérer l'ordre des attaques par apport à la speed des personnages
        /// </summary>
        /// <param name="skillFromCharacter1">L'attaque selectionné par le joueur 1</param>
        /// <param name="skillFromCharacter2">L'attaque selectionné par le joueur 2</param>
        /// <exception cref="ArgumentNullException">si une des deux attaques est null</exception>
        public void ExecuteTurn(Skill skillFromCharacter1, Skill skillFromCharacter2)
        {
            _currentOrder = CheckWhoAttackingFirst();

            for (int i = 0; i < 2; i++) //for Loop so Both pokemon can attack
            {
                switch (_currentOrder)
                {
                    case FightOrder.CHARACTER1: //Character 1 attack phase
                        _currentOrder = FightOrder.CHARACTER2;
                        if(!Character1.IsAlive)
                        {
                            return;
                        }

                        if (Character1.CurrentStatus != null) //Check for status
                        {
                            if (!Character1.CurrentStatus.CanAttack)  //Stop if can't attack
                                break;

                            if (Character1.CurrentStatus is CrazyStatus) // Attack self if crazy
                            {
                                Character1.AttackSelf();
                                return;
                            }
                        }
                        Character2.ReceiveAttack(skillFromCharacter1);
                        break;

                    case FightOrder.CHARACTER2: //Character 2 attack phase
                        _currentOrder = FightOrder.CHARACTER1;
                        if (!Character2.IsAlive)
                        {
                            return;
                        }

                        if (Character2.CurrentStatus != null) //Check for status
                        {
                            if (!Character2.CurrentStatus.CanAttack) //Stop if can't attack
                                break;

                            if (Character2.CurrentStatus is CrazyStatus) // Attack self if crazy
                            {
                                Character2.AttackSelf();
                                return;
                            }
                        }
                        Character1.ReceiveAttack(skillFromCharacter2);
                        break;
                };
            }

            //Apply Burn Effect
            OnEndTurn?.Invoke();
        }

        //True = Character1
        //False = Character2
        //Renvoie celui qui attaque en premier
        FightOrder CheckWhoAttackingFirst()
        {
            if (Character1.CurrentEquipment != null)
            {
                if (Character1.CurrentEquipment.BonusFirstAttack)
                {
                    return FightOrder.CHARACTER1;
                }
            }
            else if (Character2.CurrentEquipment != null)
            {
                if (Character2.CurrentEquipment.BonusFirstAttack)
                {
                    return FightOrder.CHARACTER2;
                }
            }

            return Character1.Speed > Character2.Speed ? FightOrder.CHARACTER1 : FightOrder.CHARACTER2;
        }
    }
}
