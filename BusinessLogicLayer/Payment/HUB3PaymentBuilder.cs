using BusinessLogicLayer.Exceptions;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Payment {

    /// <summary>
    /// Builder class for HUB3 payment payload.
    /// Source: (Hrvatska udruga banaka) https://www.hub.hr/sites/default/files/inline-files/2dbc_0.pdf
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class HUB3PaymentBuilder {

        public string Header { get; private set; } = "";

        public string Currency { get; private set; } = "";
        public string Amount { get; private set; } = "";

        public string PayerName { get; private set; } = "";
        public string PayerAddress1 { get; private set; } = "";
        public string PayerAddress2 { get; private set; } = "";

        public string RecipientName { get; private set; } = "";
        public string RecipientAddress1 { get; private set; } = "";
        public string RecipientAddress2 { get; private set; } = "";

        public string RecipientAccount { get; private set; } = "";

        public string CallingModelToRecipientNumber { get; private set; } = "";
        public string CallToRecipientNumber { get; private set; } = "";

        public string PurposeCode { get; private set; } = "";
        public string PaymentDescription { get; private set; } = "";

        public HUB3PaymentBuilder() {
            Header = "HRVHUB30";
        }

        public HUB3PaymentBuilder SetCurrency(string currency) {

            if(currency.Length != 3) {
                throw new HUB3PaymentException("Oznaka valute mora biti duljine 3 znaka!");
            }

            Currency = currency;
            return this;
        }

        public HUB3PaymentBuilder SetAmount(decimal amount) {

            if(amount < 0) {
                throw new HUB3PaymentException("Iznos plaćanja mora biti pozitivan!");
            }

            string value = amount.ToString("0.00");
            value = Regex.Replace(value, @"\.|\,", "");
            value = value.PadLeft(15, '0');

            if (value.Length != 15) {
                throw new HUB3PaymentException("Iznos plaćanja mora sadržavati 15 znakova!");
            }

            Amount = value;
            return this;
        }

        public HUB3PaymentBuilder SetRecipientName(string name) {

            if(name.Length > 25) {
                throw new HUB3PaymentException("Naziv primatelja može imati najviše 25 znakova!");
            }

            RecipientName = name;
            return this;
        }

        public HUB3PaymentBuilder SetRecipientIBAN(string iban) {

            if (iban.Length > 21) {
                throw new HUB3PaymentException("IBAN primatelja može imati najviše 21 znak!");
            }

            RecipientAccount = iban;
            return this;
        }

        public HUB3PaymentBuilder SetPaymentDescription(string description) {

            if (description.Length > 35) {
                throw new HUB3PaymentException("Opis plaćanja može imati najviše 35 znakova!");
            }

            PaymentDescription = description;
            return this;
        }

        public string Build() {

            var stringBuilder = new StringBuilder();

            stringBuilder.Append(Header + "\n");
            stringBuilder.Append(Currency + "\n");
            stringBuilder.Append(Amount + "\n");
            stringBuilder.Append(PayerName + "\n");
            stringBuilder.Append(PayerAddress1 + "\n");
            stringBuilder.Append(PayerAddress2 + "\n");
            stringBuilder.Append(RecipientName + "\n");
            stringBuilder.Append(RecipientAddress1 + "\n");
            stringBuilder.Append(RecipientAddress2 + "\n");
            stringBuilder.Append(RecipientAccount + "\n");
            stringBuilder.Append(CallingModelToRecipientNumber + "\n");
            stringBuilder.Append(CallToRecipientNumber + "\n");
            stringBuilder.Append(PurposeCode + "\n");
            stringBuilder.Append(PaymentDescription + "\n");

            return stringBuilder.ToString();
        }

    }
}
