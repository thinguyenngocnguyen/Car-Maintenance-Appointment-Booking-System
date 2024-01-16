using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Assignment2_ThiNguyenNgocNguyen
{
    public static class ValidationHelper
    {
        public static string Capitalize(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            string trimmedInput = input.Trim();
            string[] words = trimmedInput.Split(' ');

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];

                if (i == 0 || word.Length > 1)
                {
                    string capitalizedWord = char.ToUpper(word[0]) + word.Substring(1).ToLower();
                    result.Append(capitalizedWord);
                }
                else
                {
                    result.Append(word.ToLower());
                }

                if (i < words.Length - 1)
                    result.Append(' ');
            }

            return result.ToString();
        }

        public static bool IsValidPostalCode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string pattern = @"^[A-Za-z]\d[A-Za-z]\d[A-Za-z]\d$";
            return Regex.IsMatch(input, pattern);
        }

        public static bool IsValidProvinceCode(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string[] validCodes = { "AB", "BC", "MB", "NB", "NL", "NS", "NT", "NU", "ON", "PE", "QC", "SK", "YT" };
            return validCodes.Contains(input.ToUpper());
        }

        public static bool IsValidPhoneNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            string pattern = @"^\d{3}-\d{3}-\d{4}$";
            return Regex.IsMatch(input, pattern);
        }
    }





}
