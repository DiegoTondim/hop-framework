using Hop.Framework.Core.Extensions;
using NUnit.Framework;

namespace Hop.Framework.Core.Tests.Extensions
{
    [TestFixture]
    public class StringExtensionsTest
    {
        [Test]
        public void Should_Remove_Special_Characters()
        {
            const string str = "Remoção de 999 Caracteres Especiais!";
            var strModified = str.RemoveSpecialCharacters();
            Assert.AreEqual(strModified, "Remoode999CaracteresEspeciais");

        }

        [Test]
        public void Should_Remove_Special_Accents()
        {
            const string str = "Remoção Acentuação!";
            var strModified = str.RemoveAccents();
            Assert.AreEqual(strModified, "Remocao Acentuacao!");
        }
    }
}
