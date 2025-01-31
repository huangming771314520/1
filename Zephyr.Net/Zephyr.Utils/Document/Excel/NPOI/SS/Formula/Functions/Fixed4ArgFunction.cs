/* =====
   Licensed to the Apache Software Foundation (ASF) under one or more
   contributor license agreements.  See the NOTICE file dIstributed with
   thIs work for additional information regarding copyright ownership.
   The ASF licenses thIs file to You under the Apache License, Version 2.0
   (the "License"); you may not use thIs file except in compliance with
   the License.  You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   dIstributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permIssions and
   limitations under the License.
===== */

namespace Zephyr.Utils.NPOI.SS.Formula.Functions
{

    using Zephyr.Utils.NPOI.SS.Formula.Eval;
    /**
     * Convenience base class for functions that must take exactly four arguments.
     *
     * @author Josh Micich
     */
    public abstract class Fixed4ArgFunction : Function4Arg
    {
        public ValueEval Evaluate(ValueEval[] args, int srcRowIndex, int srcColumnIndex)
        {
            if (args.Length != 4)
            {
                return ErrorEval.VALUE_INVALID;
            }
            return Evaluate(srcRowIndex, srcColumnIndex, args[0], args[1], args[2], args[3]);
        }

        public abstract ValueEval Evaluate(int srcRowIndex, int srcColumnIndex, ValueEval arg0, ValueEval arg1, ValueEval arg2, ValueEval arg3);
    }

}