using BusinessLogicLayer.Payment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace UnitTests {

    [TestClass]
    public class HUB3PaymentBuilderTests {

        [TestMethod]
        public void SetAmount_PostaviVrijednost_VrijednostIspravnoFormatirana() {

            var builder = new HUB3PaymentBuilder();

            var inputs = new List<(decimal amount, string expected)>() {
                
                (1m, AmountOf("1", "00")),
                (10m, AmountOf("10", "00")),
                (3842m, AmountOf("3842", "00")),
               
                (0.25m, AmountOf("0", "25")),
                (0.10m, AmountOf("0", "10")),
                (0.08m, AmountOf("0", "08")),

                (1.05m, AmountOf("1", "05")),
                (1.10m, AmountOf("1", "10")),
                (1.32m, AmountOf("1", "32")),

                (200.05m, AmountOf("200", "05")),
                (401.429m, AmountOf("401", "43")),
                (222.222m, AmountOf("222", "22")),
                (9.9999999m, AmountOf("10", "00")),
                (8.493m, AmountOf("8", "49")),
                (8.495m, AmountOf("8", "50")),
            };

            var actual = new List<string>();
            var expected = new List<string>();

            foreach(var input in inputs) {

                builder.SetAmount((decimal)input.amount);

                actual.Add(builder.Amount);
                expected.Add(input.expected);
            }

            CollectionAssert.AreEqual(expected, actual);
        }
        
        private string AmountOf(string intPart, string decimalPart) {
            return (intPart + decimalPart).PadLeft(15, '0');
        }

    }
}
