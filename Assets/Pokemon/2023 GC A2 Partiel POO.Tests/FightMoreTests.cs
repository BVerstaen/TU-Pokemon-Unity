
using _2023_GC_A2_Partiel_POO.Level_2;
using NUnit.Framework;
using UnityEditor.VersionControl;

namespace _2023_GC_A2_Partiel_POO.Tests.Level_2
{
    public class FightMoreTests
    {
        // Tu as probablement remarqué qu'il y a encore beaucoup de code qui n'a pas été testé ...
        // À présent c'est à toi de créer les TU sur le reste et de les implémenter

        // Ce que tu peux ajouter:
        // - Ajouter davantage de sécurité sur les tests apportés
        // - ajouter un equipement qui rend les attaques prioritaires puis l'enlever et voir que l'attaque n'est plus prioritaire etc)
        // - Le support des status (sleep et burn) qui font des effets à la fin du tour et/ou empeche le pkmn d'agir
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
    }
}
