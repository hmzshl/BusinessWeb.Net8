namespace BusinessWeb
{
    using System;
    using System.Globalization;

    public class DecimalToFrench
    {
        private static readonly string[] UnitsMap = {
        "zéro", "un", "deux", "trois", "quatre", "cinq", "six", "sept", "huit", "neuf"
    };

        private static readonly string[] TeensMap = {
        "dix", "onze", "douze", "treize", "quatorze", "quinze", "seize", "dix-sept", "dix-huit", "dix-neuf"
    };

        private static readonly string[] TensMap = {
        "zéro", "dix", "vingt", "trente", "quarante", "cinquante", "soixante", "soixante", "quatre-vingt", "quatre-vingt"
    };

        private static readonly string Hundred = "cent";
        private static readonly string Thousand = "mille";
        private static readonly string Million = "million";
        private static readonly string Billion = "milliard";

        public string ConvertToFrench(decimal number)
        {
            // Convert the decimal number to its textual representation in French
            return ConvertToFrench((long)number);
        }

        private string ConvertToFrench(long number)
        {
            if (number == 0)
                return UnitsMap[0];

            if (number < 0)
                return "moins " + ConvertToFrench(Math.Abs(number));

            string words = "";

            if ((number / 1000000000) > 0)
            {
                words += ConvertToFrench(number / 1000000000) + " " + Billion + " ";
                number %= 1000000000;
            }

            if ((number / 1000000) > 0)
            {
                words += ConvertToFrench(number / 1000000) + " " + Million + " ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertToFrench(number / 1000) + " " + Thousand + " ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                if (number == 100)
                {
                    words += Hundred + " ";
                }
                else
                {
                    words += UnitsMap[number / 100] + " " + Hundred + " ";
                }

                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "et ";

                if (number < 10)
                    words += UnitsMap[number];
                else if (number < 20)
                    words += TeensMap[number - 10];
                else
                {
                    words += TensMap[number / 10];

                    if ((number % 10) > 0)
                        words += "-" + UnitsMap[number % 10];
                }
            }

            return words.Trim();
        }
    }


}
