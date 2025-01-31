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

namespace Zephyr.Utils.NPOI.HSSF.Record
{

    using System;
    using System.Text;
    using Zephyr.Utils.NPOI.Util;


    /**
     * Record for the left margin.
     * NOTE: This source was automatically generated.
     * @author Shawn Laubach (slaubach at apache dot org)
     */
    public class LeftMarginRecord : StandardRecord, Margin
    {
        public const short sid = 0x26;
        private double field_1_margin;

        public LeftMarginRecord() { }

        /**
         * Constructs a LeftMargin record and Sets its fields appropriately.
         *
         * @param in the RecordInputstream to Read the record from
         */
        public LeftMarginRecord(RecordInputStream in1)
        {
            field_1_margin = in1.ReadDouble();
        }

        public override String ToString()
        {
            StringBuilder buffer = new StringBuilder();
            buffer.Append("[LeftMargin]\n");
            buffer.Append("    .margin               = ").Append(" (").Append(Margin).Append(" )\n");
            buffer.Append("[/LeftMargin]\n");
            return buffer.ToString();
        }

        public override void Serialize(ILittleEndianOutput out1)
        {
            out1.WriteDouble(field_1_margin);
        }

        protected override int DataSize
        {
            get
            {
                return 8;
            }
        }

        public override short Sid
        {
            get { return sid; }
        }

        /**
         * Get the margin field for the LeftMargin record.
         */
        public double Margin
        {
            get { return field_1_margin; }
            set { this.field_1_margin = value; }
        }


        public override Object Clone()
        {
            LeftMarginRecord rec = new LeftMarginRecord();
            rec.field_1_margin = this.field_1_margin;
            return rec;
        }
    }
}