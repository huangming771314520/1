﻿/* =====
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file distributed with
   this work for additional information regarding copyright ownership.
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

using System;
using System.Collections;

/* =
 * About NPOI
 * Author: Tony Qu 
 * Author's email: tonyqus (at) gmail.com 
 * Author's Blog: tonyqus.wordpress.com.cn (wp.tonyqus.cn)
 * HomePage: http://www.codeplex.com/npoi
 * Contributors:
 * 
 * ======*/

namespace Zephyr.Utils.NPOI.Util
{
    /// <summary>
    /// Returns immutable Btfield instances.
    /// @author Jason Height (jheight at apache dot org)
    /// </summary>
    public class BitFieldFactory
    {
        //use Hashtable to replace HashMap
        private static Hashtable instances = new Hashtable();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <param name="mask">The mask.</param>
        /// <returns></returns>
        public static BitField GetInstance(int mask)
        {
            BitField f = (BitField)instances[mask];
            if (f == null)
            {
                f = new BitField(mask);
                instances[mask] = f;
            }
            return f;
        }
    }
}
