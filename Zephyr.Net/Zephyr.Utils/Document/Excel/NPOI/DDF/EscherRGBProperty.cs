
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

namespace Zephyr.Utils.NPOI.DDF
{
    using System;
    using System.IO;
    using System.Collections;
    using Zephyr.Utils.NPOI.Util;

    /// <summary>
    /// A color property.
    /// @author Glen Stampoultzis (glens at apache.org)
    /// </summary>
    public class EscherRGBProperty : EscherSimpleProperty
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="EscherRGBProperty"/> class.
        /// </summary>
        /// <param name="propertyNumber">The property number.</param>
        /// <param name="rgbColor">Color of the RGB.</param>
        public EscherRGBProperty(short propertyNumber, int rgbColor):base(propertyNumber, rgbColor)
        {
            
        }

        /// <summary>
        /// Gets the color of the RGB.
        /// </summary>
        /// <value>The color of the RGB.</value>
        public int RgbColor
        {
            get { return propertyValue; }
        }

        /// <summary>
        /// Gets the red.
        /// </summary>
        /// <value>The red.</value>
        public byte Red
        {
            get { return (byte)(propertyValue & 0xFF); }
        }

        /// <summary>
        /// Gets the green.
        /// </summary>
        /// <value>The green.</value>
        public byte Green
        {
            get{return (byte)((propertyValue >> 8) & 0xFF);}
        }

        /// <summary>
        /// Gets the blue.
        /// </summary>
        /// <value>The blue.</value>
        public byte Blue
        {
            get{return (byte)((propertyValue >> 16) & 0xFF);}
        }

    }
}