using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBAN_Validation
{
    class IBAN
    {
        public string IBANstring { get; set; }
        public bool IBANvalid { get; set; }
        public bool containsOnlyLettersDigits { get; set; }
        public bool checksumCorrect { get; set; }
        public string country { get; set; }
        public bool lengthMatchesCountryRules { get; set; }
        private bool areFirstTwoLettersCorrect;
        private int supposedLength;

        public IBAN(string IBAN)
        {

            IBANstring = IBAN;
            containsOnlyLettersDigits = DoesIBANContainOnlyLettersDigits();
            areFirstTwoLettersCorrect = AreFirstTwoLettersForCountry();
            lengthMatchesCountryRules = lengthMatchesCountryLength();
            checksumCorrect = isCheckSumCorrect();
            if (checksumCorrect && lengthMatchesCountryRules && containsOnlyLettersDigits)
            {
                IBANvalid = true;
            }
            else
            {
                IBANvalid = false;
            }
        }

        private bool DoesIBANContainOnlyLettersDigits()
        {
            if (IBANstring.Length == 0 || IBANstring.Length < 15 || IBANstring.Length > 35) { return false; }
            foreach (char c in IBANstring)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        private bool AreFirstTwoLettersForCountry()
        {
            var countryIBANLength = new Dictionary<string, int>(){
                    {"AL", 28},{"AD", 24},{"AT", 20},{"AZ", 28},{"BH", 22},{"BY", 28},
                    {"BE", 16},{"BA", 20},{"BR", 29},{"BG", 22},{"CR", 22},{"HR", 21},
                    {"CY", 28},{"CZ", 24},{"DK", 18},{"DO", 28},{"EG", 29},{"SV", 28},
                    {"EE", 20},{"FO", 18},{"FI", 18},{"FR", 27},{"GE", 22},{"DE", 22},
                    {"GI", 23},{"GR", 27},{"GL", 18},{"GT", 28},{"VA", 22},{"HU", 28},
                    {"IS", 26},{"IQ", 23},{"IE", 22},{"IL", 23},{"IT", 27},{"JO", 30},
                    {"KZ", 20},{"XK", 20},{"KW", 30},{"LV", 21},{"LB", 28},{"LY", 25},
                    {"LI", 21},{"LT", 20},{"LU", 20},{"MT", 31},{"MR", 27},{"MU", 30},
                    {"MD", 24},{"MC", 27},{"ME", 22},{"NL", 18},{"MK", 19},{"NO", 15},
                    {"PK", 24},{"PS", 29},{"PL", 28},{"PT", 25},{"QA", 29},{"RO", 24},
                    {"LC", 32},{"SM", 27},{"ST", 25},{"SA", 24},{"RS", 22},{"SC", 31},
                    {"SK", 24},{"SI", 19},{"ES", 24},{"SD", 18},{"SE", 24},{"CH", 21},
                    {"TL", 23},{"TN", 24},{"TR", 26}, {"UA", 29},{"AE", 23}, {"GB", 22},
                    {"VG", 24}};

            if(IBANstring.Length >= 2)
            {
                string firstTwoLetters = IBANstring.Substring(0, 2);
                if (countryIBANLength.ContainsKey(firstTwoLetters))
                {
                    country = firstTwoLetters;
                    supposedLength = countryIBANLength[firstTwoLetters];
                    return true;
                }
            }
            else
            {
                supposedLength = -1;
            }
            return false;
        }

        private bool lengthMatchesCountryLength()
        {
            if (IBANstring.Length == supposedLength)
            {
                return true;
            }
            return false;
        }

        private bool isCheckSumCorrect()
        {
            string IBANdigits = convertToDigits(IBANstring);
            if (mod97(IBANdigits) == 1)
            {
                return true;
            }
            return false;
        }

        private string convertToDigits(string IBAN)
        {

            if (areFirstTwoLettersCorrect)
            {
                string appendToEnd = IBAN.Substring(0, 4);
                IBAN += (appendToEnd);
                IBAN = IBAN.Substring(4);

                char symbol;
                int symbolInt;
                for (int i = 0; i < IBAN.Length; i++)
                {
                    symbol = IBAN[i];
                    if (Char.IsLetter(symbol))
                    {
                        symbolInt = (int)Char.ToUpper(symbol);
                        symbolInt -= 55;
                        IBAN = IBAN.Remove(i, 1).Insert(i, symbolInt.ToString());
                    }
                }
                return IBAN;
            }
            return "0"; // So if first 2 letters arent country letters, checksum is always wrong
        }

        private int mod97(string IBAN)
        {
            int result = 0;

            for (int i = 0; i < IBAN.Length; i++)
            {
                result = (result * 10 + IBAN[i] - '0') % 97;
            }

            return result;
        }
    }
}
