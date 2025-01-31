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

    public enum FontUnderlineType : byte
    {
        /**
         * not underlined
         */
        NONE = 0,
        /**
         * single (normal) underline
         */
        SINGLE = 1,
        /**
         * double underlined
         */
        DOUBLE = 2,
        /**
         * accounting style single underline
         */
        SINGLE_ACCOUNTING = 0x21,
        /**
         * accounting style double underline
         */
        DOUBLE_ACCOUNTING = 0x22
    }

    public enum FontSuperScript:short
    { 
    
        /**
         * no type Offsetting (not super or subscript)
         */

        NONE = 0,

        /**
         * superscript
         */

        SUPER = 1,

        /**
         * subscript
         */

        SUB = 2,
    }
    public enum FontColor:short
    {
        /// <summary>
        /// Allow accessing the Initial value.
        /// </summary>
        None = 0,

        /**
         * normal type of black color.
         */

        NORMAL = 0x7fff,

        /**
         * Dark Red color
         */

        RED = 0xa,
    }
    public enum FontBoldWeight:short
    {
        /// <summary>
        /// Allow accessing the Initial value.
        /// </summary>
        None = 0,

        /**
         * Normal boldness (not bold)
         */

        NORMAL = 0x190,

        /**
         * Bold boldness (bold)
         */

        BOLD = 0x2bc,
    }


    public interface IFont
    {

        /**
         * get the name for the font (i.e. Arial)
         * @return String representing the name of the font to use
         */

        String FontName { get; set; }

        /**
         * get the font height in unit's of 1/20th of a point.  Maybe you might want to
         * use the GetFontHeightInPoints which matches to the familiar 10, 12, 14 etc..
         * @return short - height in 1/20ths of a point
         * @see #GetFontHeightInPoints()
         */

        short FontHeight { get; set; }

        /**
         * get the font height
         * @return short - height in the familiar unit of measure - points
         * @see #GetFontHeight()
         */

        short FontHeightInPoints { get; set; }

        /**
         * get whether to use italics or not
         * @return italics or not
         */

        bool IsItalic { get; set; }


        /**
         * get whether to use a strikeout horizontal line through the text or not
         * @return strikeout or not
         */

        bool IsStrikeout { get; set; }

        /**
         * get the color for the font
         * @return color to use
         * @see #COLOR_NORMAL
         * @see #COLOR_RED
         * @see Zephyr.Utils.NPOI.HSSF.usermodel.HSSFPalette#GetColor(short)
         */
        short Color { get; set; }


        /**
         * get normal,super or subscript.
         * @return offset type to use (none,super,sub)
         * @see #SS_NONE
         * @see #SS_SUPER
         * @see #SS_SUB
         */

        short TypeOffset { get; set; }

        /**
         * get type of text underlining to use
         * @return underlining type
         * @see #U_NONE
         * @see #U_SINGLE
         * @see #U_DOUBLE
         * @see #U_SINGLE_ACCOUNTING
         * @see #U_DOUBLE_ACCOUNTING
         */

        byte Underline { get; set; }

        /**
         * get character-set to use.
         * @return character-set
         * @see #ANSI_CHARSET
         * @see #DEFAULT_CHARSET
         * @see #SYMBOL_CHARSET
         */
        short Charset { get; set; }

        /**
         * get the index within the XSSFWorkbook (sequence within the collection of Font objects)
         * 
         * @return unique index number of the underlying record this Font represents (probably you don't care
         *  unless you're comparing which one is which)
         */
        short Index { get; }

        short Boldweight { get; set; }
    }
}