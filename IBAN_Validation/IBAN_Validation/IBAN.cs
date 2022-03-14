using System;
using System.Collections.Generic;

namespace IBAN_Validation
{
    public class IBAN
    {
        public string IBANstring { get; set; }
        public bool IBANvalid { get; set; }
        public bool IBANValidForChecking { get; set; }
        public bool checksumCorrect { get; set; }
        public string country { get; set; }
        public bool lengthMatchesCountryRules { get; set; }
        private bool FirstTwoLettersAreCountryLetters;
        private int supposedLength;

        public IBAN(string IBAN)                                                                // constructor which initializes class property values when called
        {
            IBANstring = IBAN;
            IBANValidForChecking = IsIBANValidForChecking();
            FirstTwoLettersAreCountryLetters = AreFirstTwoLettersForCountry();
            lengthMatchesCountryRules = lengthMatchesCountryLength();
            checksumCorrect = IsCheckSumCorrect();
            IBANvalid = IsIBANValid();
        }

        private bool IsIBANValid()
        {
            if (checksumCorrect && lengthMatchesCountryRules && IBANValidForChecking && FirstTwoLettersAreCountryLetters)
            {
                return true;
            }
            return false;
        }

        private bool IsIBANValidForChecking()                                                   // Returns true if 15 < IBAN number length < 35 and if 
                                                                                                // there are no symbols that aren't either digits or letters in the IBAN number
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

        private bool AreFirstTwoLettersForCountry()                     // Checks whether or not the first two Letters for IBAN number are for country. 
                                                                        // If they are, it then sets suposedLength to match.
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

            if(IBANValidForChecking)
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

        private bool IsCheckSumCorrect()
        {
            string IBANdigits = ConvertToDigits(IBANstring);
            if (mod97(IBANdigits) == 1)
            {
                return true;
            }
            return false;
        }

        private string ConvertToDigits(string IBANstr)                                  
        {                                                                               // Converts letters to digits so that A - 10, B - 11 and so on.
            if (IBANValidForChecking && FirstTwoLettersAreCountryLetters)
            {
                IBANstr = AppendCountryAndCheckDigitsToEnd(IBANstr);
                char symbol;
                int symbolInt;
                for (int i = 0; i < IBANstr.Length; i++)
                {
                    symbol = IBANstr[i];
                    if (Char.IsLetter(symbol))
                    {
                        symbolInt = (int)Char.ToUpper(symbol);
                        symbolInt -= 55;
                        IBANstr = IBANstr.Remove(i, 1).Insert(i, symbolInt.ToString());
                    }
                }
                return IBANstr;
            }
            return "0"; // So if first 2 letters arent country letters, checksum is always wrong
        }

        private string AppendCountryAndCheckDigitsToEnd(string IBANstr)             // Removes first 4 symbols of IBAN string and appends them to the end of the string.
        {
            if (FirstTwoLettersAreCountryLetters)
            {
                string appendToEnd = IBANstr.Substring(0, 4);
                IBANstr += (appendToEnd);
                IBANstr = IBANstr.Substring(4);
            }
            return IBANstr;
        }

        private int mod97(string IBANstrDigits)
        {
            int result = 0;

            for (int i = 0; i < IBANstrDigits.Length; i++)
            {
                result = (result * 10 + IBANstrDigits[i] - '0') % 97;
            }
            return result;
        }
    }
}
