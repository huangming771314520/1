/* =====
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
===== */
namespace Zephyr.Utils.NPOI.SS.UserModel
{
    using System;
    using Zephyr.Utils.NPOI.SS.Util;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Globalization;





    /**
     * A wrapper around a {@link SimpleDateFormat} instance,
     * which handles a few Excel-style extensions that
     * are not supported by {@link SimpleDateFormat}.
     * Currently, the extensions are around the handling
     * of elapsed time, eg rendering 1 day 2 hours
     * as 26 hours.
     */
    public class ExcelStyleDateFormatter : SimpleDateFormat
    {
        public static char MMMMM_START_SYMBOL = '\ue001';
        public static char MMMMM_TRUNCATE_SYMBOL = '\ue002';
        public static char H_BRACKET_SYMBOL = '\ue010';
        public static char HH_BRACKET_SYMBOL = '\ue011';
        public static char M_BRACKET_SYMBOL = '\ue012';
        public static char MM_BRACKET_SYMBOL = '\ue013';
        public static char S_BRACKET_SYMBOL = '\ue014';
        public static char SS_BRACKET_SYMBOL = '\ue015';
        public static char L_BRACKET_SYMBOL = '\ue016';
        public static char LL_BRACKET_SYMBOL = '\ue017';

        private DecimalFormat format1digit = new DecimalFormat("0");
        private DecimalFormat format2digits = new DecimalFormat("00");

        private DecimalFormat format3digit = new DecimalFormat("0");
        private DecimalFormat format4digits = new DecimalFormat("00");

        static ExcelStyleDateFormatter()
        {
            //DataFormatter.SetExcelStyleRoundingMode(format1digit, RoundingMode.DOWN);
            //DataFormatter.SetExcelStyleRoundingMode(format2digits, RoundingMode.DOWN);
            //DataFormatter.SetExcelStyleRoundingMode(format3digit);
            //DataFormatter.SetExcelStyleRoundingMode(format4digits);
        }

        private double dateToBeFormatted = 0.0;

        public ExcelStyleDateFormatter()
            : base()
        {
        }

        public ExcelStyleDateFormatter(String pattern)
            : base(ProcessFormatPattern(pattern))
        {

        }


        //public ExcelStyleDateFormatter(String pattern,
        //                           DateFormatSymbols formatSymbols)
        //{
        //    super(processFormatPattern(pattern), formatSymbols);
        //}

        //public ExcelStyleDateFormatter(String pattern, Locale locale)
        //{
        //    super(processFormatPattern(pattern), locale);
        //}
        private static string DateTimeMatchEvaluator(Match match)
        {
            return match.Groups[1].Value;
        }
        /**
         * Takes a format String, and Replaces Excel specific bits
         * with our detection sequences
         */
        private static String ProcessFormatPattern(String f)
        {
            String t = f.Replace("MMMMM", MMMMM_START_SYMBOL + "MMM" + MMMMM_TRUNCATE_SYMBOL);
            t = Regex.Replace(t, "\\[H\\]", (H_BRACKET_SYMBOL).ToString(), RegexOptions.IgnoreCase);
            t = Regex.Replace(t, "\\[HH\\]", (HH_BRACKET_SYMBOL).ToString(), RegexOptions.IgnoreCase);
            t = Regex.Replace(t, "\\[m\\]", (M_BRACKET_SYMBOL).ToString(), RegexOptions.IgnoreCase);
            t = Regex.Replace(t, "\\[mm\\]", (MM_BRACKET_SYMBOL).ToString(), RegexOptions.IgnoreCase);
            t = Regex.Replace(t, "\\[s\\]", (S_BRACKET_SYMBOL).ToString(), RegexOptions.IgnoreCase);
            t = Regex.Replace(t, "\\[ss\\]", (SS_BRACKET_SYMBOL).ToString(), RegexOptions.IgnoreCase);
            t = t.Replace("s.000", "s.fff");
            t = t.Replace("s.00", "s." + LL_BRACKET_SYMBOL);
            t = t.Replace("s.0", "s." + L_BRACKET_SYMBOL);
            //only one char 'M'
            //see http://msdn.microsoft.com/en-us/library/8kb3ddd4.aspx#UsingSingleSpecifiers
            t = Regex.Replace(t, "(?<![M%])M(?!M)", "%M");
            return t;
        }

        /**
         * Used to let us know what the date being
         * formatted is, in Excel terms, which we
         * may wish to use when handling elapsed
         * times.
         */
        public void SetDateToBeFormatted(double date)
        {
            this.dateToBeFormatted = date;
        }
        public override string Format(object obj)
        {
            return this.Format((DateTime)obj, new StringBuilder()).ToString();
        }

        public StringBuilder Format(DateTime date, StringBuilder paramStringBuilder)
        {
            // Do the normal format
            string s = string.Empty;
            if (Regex.IsMatch(pattern, "[yYmMdDhHsS\\-/,. :\"\\\\]+0?[ampAMP/]*"))
            {
                s = date.ToString(pattern, DateTimeFormatInfo.InvariantInfo);
            }
            else
                s = pattern;
            
            // Now handle our special cases
            if (s.IndexOf(MMMMM_START_SYMBOL) != -1)
            {
                Regex reg = new Regex(MMMMM_START_SYMBOL + "(\\w)\\w+" + MMMMM_TRUNCATE_SYMBOL, RegexOptions.IgnoreCase);
                Match m = reg.Match(s);
                if (m.Success)
                {
                    s = reg.Replace(s, m.Groups[1].Value);
                }
            }

            if (s.IndexOf(H_BRACKET_SYMBOL) != -1 ||
                    s.IndexOf(HH_BRACKET_SYMBOL) != -1)
            {
                double hours = dateToBeFormatted * 24 + 0.01;
                //get the hour part of the time
                hours = Math.Floor(hours);
                s = s.Replace(
                        (H_BRACKET_SYMBOL).ToString(),
                        format1digit.Format(hours)
                );
                s = s.Replace(
                        (HH_BRACKET_SYMBOL).ToString(),
                        format2digits.Format(hours)
                );
            }

            if (s.IndexOf(M_BRACKET_SYMBOL) != -1 ||
                    s.IndexOf(MM_BRACKET_SYMBOL) != -1)
            {
                double minutes = dateToBeFormatted * 24 * 60 + 0.01;
                minutes = Math.Floor(minutes);
                s = s.Replace(
                        (M_BRACKET_SYMBOL).ToString(),
                        format1digit.Format(minutes)
                );
                s = s.Replace(
                        (MM_BRACKET_SYMBOL).ToString(),
                        format2digits.Format(minutes)
                );
            }
            if (s.IndexOf(S_BRACKET_SYMBOL) != -1 ||
                    s.IndexOf(SS_BRACKET_SYMBOL) != -1)
            {
                double seconds = (dateToBeFormatted * 24.0 * 60.0 * 60.0) + 0.01;
                s = s.Replace(
                        (S_BRACKET_SYMBOL).ToString(),
                        format1digit.Format(seconds)
                );
                s = s.Replace(
                        (SS_BRACKET_SYMBOL).ToString(),
                        format2digits.Format(seconds)
                );
            }

            if (s.IndexOf(L_BRACKET_SYMBOL) != -1 ||
                    s.IndexOf(LL_BRACKET_SYMBOL) != -1)
            {
                float millisTemp = (float)((dateToBeFormatted - Math.Floor(dateToBeFormatted)) * 24.0 * 60.0 * 60.0);
                float millis = (millisTemp - (int)millisTemp);
                s = s.Replace(
                        (L_BRACKET_SYMBOL).ToString(),
                        format3digit.Format(millis * 10)
                );
                s = s.Replace(
                        (LL_BRACKET_SYMBOL).ToString(),
                        format4digits.Format(millis * 100)
                );
            }

            return new StringBuilder(s);
        }
    }

}