
/* =====
   Licensed to the Apache Software Foundation (ASF) Under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for Additional information regarding copyright ownership.
   The ASF licenses this file to You Under the Apache License, Version 2.0
   (the "License"); you may not use this file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed Under the License is distributed on an "AS Is" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations Under the License.
===== */


namespace Zephyr.Utils.NPOI.HSSF.Record.Chart
{
    using System;
    using System.Text;
    using Zephyr.Utils.NPOI.Util;


    /**
     * The font index record indexes into the font table for the text record.
     * NOTE: This source is automatically generated please do not modify this file.  Either subclass or
     *       Remove the record in src/records/definitions.

     * @author Glen Stampoultzis (glens at apache.org)
     */
    public class FontIndexRecord
       : StandardRecord
    {
        public const short sid = 0x1026;
        private short field_1_fontIndex;


        public FontIndexRecord()
        {

        }

        /**
         * Constructs a FontIndex record and Sets its fields appropriately.
         *
         * @param in the RecordInputstream to Read the record from
         */

        public FontIndexRecord(RecordInputStream in1)
        {
            field_1_fontIndex = in1.ReadShort();
        }


        public override String ToString()
        {
            StringBuilder buffer = new StringBuilder();

            buffer.Append("[FONTX]\n");
            buffer.Append("    .fontIndex            = ")
                .Append("0x").Append(HexDump.ToHex(FontIndex))
                .Append(" (").Append(FontIndex).Append(" )");
            buffer.Append(Environment.NewLine);

            buffer.Append("[/FONTX]\n");
            return buffer.ToString();
        }

        public override void Serialize(ILittleEndianOutput out1)
        {
            out1.WriteShort(field_1_fontIndex);
        }

        /**
         * Size of record (exluding 4 byte header)
         */
        protected override int DataSize
        {
            get { return  2; }
        }

        public override short Sid
        {
            get { return sid; }
        }

        public override Object Clone()
        {
            FontIndexRecord rec = new FontIndexRecord();

            rec.field_1_fontIndex = field_1_fontIndex;
            return rec;
        }




        /**
         * Get the font index field for the FontIndex record.
         */
        public short FontIndex
        {
            get { return field_1_fontIndex; }
            set { this.field_1_fontIndex = value; }
        }



    }
}


