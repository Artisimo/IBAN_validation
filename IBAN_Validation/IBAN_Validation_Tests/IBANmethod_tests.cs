using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using IBAN_Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IBAN_Validation_Tests
{
    public class IBANmethod_tests
    {
        [Theory]
        [InlineData("LT075951119546221883", true)]
        [InlineData("", false)]
        [InlineData("StringLongerThanMaximumIBANnumberLength", false)]
        private void IBAN_contains_only_letters_digits(string IBANstring, bool expected)
        {
            IBAN target = new IBAN(IBANstring);
            PrivateObject obj = new PrivateObject(target);
            var actual = obj.Invoke("IsIBANValidForChecking");
            Xunit.Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("LT075951119546221883", true)]
        [InlineData("xLT075951119546221883", false)]
        [InlineData("", false)]
        [InlineData("075951119546221883", false)]
        private void IBAN_first_two_letters_for_country(string IBANstring, bool expected)
        {
            IBAN target = new IBAN(IBANstring);
            PrivateObject obj = new PrivateObject(target);
            var actual = obj.Invoke("AreFirstTwoLettersForCountry");
            Xunit.Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("LT075951119546221883", true)]
        [InlineData("", false)]
        [InlineData("StringLongerThanMaximumIBANnumberLength", false)]
        private void IBAN_lentgth_match_country(string IBANstring, bool expected)
        {
            IBAN target = new IBAN(IBANstring);
            PrivateObject obj = new PrivateObject(target);
            var actual = obj.Invoke("lengthMatchesCountryLength");
            Xunit.Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("LT075951119546221883", "5951119546221883LT07")]
        [InlineData("AA", "AA")]
        [InlineData("Aa", "Aa")]
        [InlineData("", "")]
        private void IBAN_AppendToEnd_correct(string IBANstring, string expected)
        {
            IBAN target = new IBAN(IBANstring);
            PrivateObject obj = new PrivateObject(target);
            var actual = obj.Invoke("AppendCountryAndCheckDigitsToEnd", IBANstring);
            Xunit.Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("LT075951119546221883", "5951119546221883212907")]
        private void IBAN_convert_to_digits(string IBANstring, string expected)
        {
            IBAN target = new IBAN(IBANstring);
            PrivateObject obj = new PrivateObject(target);
            var actual = obj.Invoke("ConvertToDigits", IBANstring);
            Xunit.Assert.Equal(actual, expected);
        }

        [Theory]
        [InlineData("LT075951119546221883", true)]
        [InlineData("LT075951119546221884", false)] // Added 1 to the last number
        private void IBAN_checksum_correct(string IBANstringDigits, bool expected)
        {
            IBAN target = new IBAN(IBANstringDigits);
            PrivateObject obj = new PrivateObject(target);
            var actual = obj.Invoke("IsCheckSumCorrect");
            Xunit.Assert.Equal(actual, expected);
        }
    }
}
