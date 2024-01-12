
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using System.Runtime.InteropServices;
using UnityEngine;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - Gérer la notion de force/faiblesse avec les différentes attaques à disposition (skills.cs)
        // - Cumuler les force/faiblesses en ajoutant un type pour l'équipement qui rendrait plus sensible/résistant à un type
        [Test]
        public void CanHealCharacter()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);

            // Character starts full life
            Assert.That(c.CurrentHealth, Is.EqualTo(100));

            //Remove some health then heal player at is Maximum
            var punch = new Punch();
            c.ReceiveAttack(punch);
            c.Heal(40);
            Assert.That(c.CurrentHealth, Is.EqualTo(100));
        }

        [Test]
        public void CurrentHealthFollowMaxHealth()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);

            Assert.IsTrue(c.MaxHealth == c.CurrentHealth);

            c.MaxHealth = 50;
            //Check if current health isn't greater than maxHealth
            Assert.IsTrue(c.MaxHealth == c.CurrentHealth);
        }

        [Test]
        public void HealDoesNotExceedMaxHealth()
        {
            var c = new Character(100, 50, 30, 20, TYPE.NORMAL);

            // Character starts full life
            Assert.That(c.CurrentHealth, Is.EqualTo(100));

            //heal player at is Maximum
            c.Heal(100);
            Assert.That(c.CurrentHealth, Is.EqualTo(100));
        }

        [Test]
        public void PokemonCanHavePriorityWithEquipement()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(30, 6000, 0, 200, TYPE.NORMAL); //Higher speed & attack to one-shot pickachu in normal circumstances
            Fight f = new Fight(pikachu, bulbizarre);
            Punch p = new Punch();

            //Equip item to pickachu
            Equipment equipment = new Equipment(0, 10, 10, 10, true);
            pikachu.Equip(equipment);

            // Both uses punch
            f.ExecuteTurn(p, p);

            Assert.That(pikachu.IsAlive, Is.EqualTo(true));
            Assert.That(bulbizarre.IsAlive, Is.EqualTo(false));
        }

        [Test]
        public void PokemonCanHaveStatus()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(30, 30, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, bulbizarre);
            MagicalGrass m = new MagicalGrass();

            f.ExecuteTurn(m, m);

            Assert.That(pikachu.CurrentStatus is SleepStatus);
        }

        [Test]
        public void PokemonCanBeSleepyAndDontAttack()
        {
            Character pikachu = new Character(100, 50, 30, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(30, 10, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, bulbizarre);
            MagicalGrass m = new MagicalGrass();

            int _oldHealth = bulbizarre.CurrentHealth;

            f.ExecuteTurn(m, m);

            //bulbizarre didn't took damage
            Assert.That(bulbizarre.CurrentHealth, Is.EqualTo(_oldHealth));
        }

        [Test]
        public void PokemonCanBeBurnedAndReceiveAutoDamage()
        {
            Character pikachu = new Character(100, 50, 50, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(30, 0, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, bulbizarre);
            FireBall fB = new FireBall();

            f.OnEndTurn += pikachu.EndTurn;
            f.OnEndTurn += bulbizarre.EndTurn;

            int _oldHealth = pikachu.CurrentHealth;

            f.ExecuteTurn(fB, fB);

            f.OnEndTurn -= pikachu.EndTurn;
            f.OnEndTurn -= bulbizarre.EndTurn;


            //pikachu took only the fireball damage (100 - 10 => 90 hp)
            Assert.IsTrue(pikachu.CurrentHealth == _oldHealth - pikachu.CurrentStatus.DamageEachTurn);
        }

        public void PokemonCanBeCrazyAndAttackSelf()
        {
            Character pikachu = new Character(100, 50, 50, 20, TYPE.NORMAL);
            Character bulbizarre = new Character(30, 0, 0, 200, TYPE.NORMAL);
            Fight f = new Fight(pikachu, bulbizarre);
            TwitterIsNoMore c = new TwitterIsNoMore();
            Punch p = new Punch();

            f.OnEndTurn += pikachu.EndTurn;
            f.OnEndTurn += bulbizarre.EndTurn;

            int _oldHealth = pikachu.CurrentHealth;

            f.ExecuteTurn(p, c);

            f.OnEndTurn -= pikachu.EndTurn;
            f.OnEndTurn -= bulbizarre.EndTurn;


            //bulbizarre didn't recieve any attack & pickachu took both bulbizare & self damage
            Debug.Log(_oldHealth - pikachu.CurrentStatus.DamageOnAttack);
            Assert.IsTrue(pikachu.CurrentHealth == _oldHealth - pikachu.CurrentStatus.DamageOnAttack);
        }
    }
}
