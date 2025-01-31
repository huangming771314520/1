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
namespace Zephyr.Utils.NPOI.SS.Format
{
    using System;
    using System.Text.RegularExpressions;
    using System.Text;
    using Zephyr.Utils.NPOI.SS.Util;

    /**
     * Formats a date value.
     *
     * @author Ken Arnold, Industrious Media LLC
     */
    public class CellDateFormatter : CellFormatter
    {
        private bool amPmUpper;
        private bool ShowM;
        private bool ShowAmPm;
        private FormatBase dateFmt;
        private String sFmt;

        private static TimeSpan EXCEL_EPOCH_TIME;
        private static DateTime EXCEL_EPOCH_DATE;

        private static CellFormatter SIMPLE_DATE = new CellDateFormatter(
                "mm/d/y");

        static CellDateFormatter()
        {
            DateTime c = new DateTime(1904, 1, 1);
            EXCEL_EPOCH_DATE = c;
            EXCEL_EPOCH_TIME = c.TimeOfDay;
        }

        private class DatePartHandler : CellFormatPart.IPartHandler
        {
            private CellDateFormatter _formatter;
            private int mStart = -1;
            private int mLen;
            private int hStart = -1;
            private int hLen;
            public DatePartHandler(CellDateFormatter formatter)
            {
                this._formatter = formatter;
            }
            public String HandlePart(Match m, String part, CellFormatType type, StringBuilder desc)
            {

                int pos = desc.Length;
                char firstCh = part[0];
                switch (firstCh)
                {
                    case 's':
                    case 'S':
                        if (mStart >= 0)
                        {
                            for (int i = 0; i < mLen; i++)
                                desc[mStart + i] = 'm';
                            mStart = -1;
                        }
                        return part.ToLower();

                    case 'h':
                    case 'H':
                        mStart = -1;
                        hStart = pos;
                        hLen = part.Length;
                        return part.ToLower();

                    case 'd':
                    case 'D':
                        mStart = -1;
                        if (part.Length <= 2)
                            return part.ToLower();
                        else
                            return part.ToLower().Replace('d', 'E');

                    case 'm':
                    case 'M':
                        mStart = pos;
                        mLen = part.Length;
                        return part.ToUpper();

                    case 'y':
                    case 'Y':
                        mStart = -1;
                        if (part.Length == 3)
                            part = "yyyy";
                        return part.ToLower();

                    case '0':
                        mStart = -1;
                        int sLen = part.Length;
                        _formatter.sFmt = "%0" + (sLen + 2) + "." + sLen + "f";
                        return part.Replace('0', 'S');

                    case 'a':
                    case 'A':
                    case 'p':
                    case 'P':
                        if (part.Length > 1)
                        {
                            // am/pm marker
                            mStart = -1;
                            _formatter.ShowAmPm = true;
                            _formatter.ShowM = char.ToLower(part[1]) == 'm';
                            // For some reason "am/pm" becomes AM or PM, but "a/p" becomes a or p
                            _formatter.amPmUpper = _formatter.ShowM || char.IsUpper(part[0]);

                            return "a";
                        }
                    //noinspection fallthrough
                        return null;
                    default:
                        return null;
                }
            }

            public void Finish(StringBuilder toAppendTo)
            {
                if (hStart >= 0 && !_formatter.ShowAmPm)
                {
                    for (int i = 0; i < hLen; i++)
                    {
                        toAppendTo[hStart + i] = 'H';
                    }
                }
            }
        }

        /**
         * Creates a new date formatter with the given specification.
         *
         * @param format The format.
         */
        public CellDateFormatter(String format)
            : base(format)
        {
            ;
            DatePartHandler partHandler = new DatePartHandler(this);
            StringBuilder descBuf = CellFormatPart.ParseFormat(format,
                    CellFormatType.DATE, partHandler);
            partHandler.Finish(descBuf);
            dateFmt = new SimpleDateFormat(descBuf.ToString());
        }

        /** {@inheritDoc} */
        public override void FormatValue(StringBuilder toAppendTo, Object value)
        {
            if (value == null)
                value = 0.0;
            if (value.GetType().IsPrimitive/* is Number*/)
            {
                double num = (double)value;
                double v = num;
                if (v == 0.0)
                    value = EXCEL_EPOCH_DATE;
                else
                    value = new DateTime((long)(EXCEL_EPOCH_TIME.Ticks + v));
            }
            
            throw new NotImplementedException();
            //AttributedCharacterIterator it = dateFmt.FormatToCharacterIterator(
            //        value);
            //bool doneAm = false;
            //bool doneMillis = false;

            //it.First();
            //for (char ch = it.First();
            //     ch != CharacterIterator.DONE;
            //     ch = it.Next())
            //{
            //    if (it.GetAttribute(DateFormat.Field.MILLISECOND) != null)
            //    {
            //        if (!doneMillis)
            //        {
            //            Date dateObj = (Date)value;
            //            int pos = toAppendTo.Length();
            //            Formatter formatter = new Formatter(toAppendTo);
            //            long msecs = dateObj.Time % 1000;
            //            formatter.Format(LOCALE, sFmt, msecs / 1000.0);
            //            toAppendTo.Remove(pos, 2);
            //            doneMillis = true;
            //        }
            //    }
            //    else if (it.GetAttribute(DateFormat.Field.AM_PM) != null)
            //    {
            //        if (!doneAm)
            //        {
            //            if (ShowAmPm)
            //            {
            //                if (amPmUpper)
            //                {
            //                    toAppendTo.Append(char.ToUpper(ch));
            //                    if (ShowM)
            //                        toAppendTo.Append('M');
            //                }
            //                else
            //                {
            //                    toAppendTo.Append(char.ToLower(ch));
            //                    if (ShowM)
            //                        toAppendTo.Append('m');
            //                }
            //            }
            //            doneAm = true;
            //        }
            //    }
            //    else
            //    {
            //        toAppendTo.Append(ch);
            //    }
            //}
        }

        /**
         * {@inheritDoc}
         * <p/>
         * For a date, this is <tt>"mm/d/y"</tt>.
         */
        public override void SimpleValue(StringBuilder toAppendTo, Object value)
        {
            SIMPLE_DATE.FormatValue(toAppendTo, value);
        }
    }
}