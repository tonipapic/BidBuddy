using BusinessLogicLayer.Exceptions;
using EntitiesLayer.Entities;
using EntitiesLayer.Models;
using System;
using System.Text.RegularExpressions;

namespace BusinessLogicLayer.Payment {

    /// <summary>
    /// Fake implementation of bank for transactions.
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class FakeBank : IBank {
        public void MakeCardPayment(Auction auction, AuctionBid selectedBid, CardPaymentInfo info) {

            string cardNumber = Regex.Replace(info.CardNumber, @"\s+", "");
            if(Regex.IsMatch(cardNumber, @"^[0-9]{8,20}$") == false) {
                throw new CardPaymentException("Broj kartice nije valjani!");
            }

            string expireDate = info.ExpireDate.Trim().Replace(' ', '/');
            if (Regex.IsMatch(expireDate, @"[0-9]{1,2}(\/)[0-9]{2}$") == false) {
                throw new CardPaymentException("Datum isteka kartice nije u valjanom obliku! Valjani oblik je MM/YY (npr. 04/28).");
            }

            string[] expireDateParts = expireDate.Split('/');
            int month = int.Parse(expireDateParts[0]);
            int year = 2000 + int.Parse(expireDateParts[1]);

            if(month < 1 || month > 12) {
                throw new CardPaymentException("Pogrešna vrijednost mjeseca isteka kartice!");
            }

            DateTime now = DateTime.Now;

            if(year < now.Year || (year == now.Year && month < now.Month)) {
                throw new CardPaymentException("Kartica je istekla!");
            }

            if(info.CVV == 0) {
                throw new CardPaymentException("Podaci o kartici nisu valjani!");
            }

        }
    }
}
