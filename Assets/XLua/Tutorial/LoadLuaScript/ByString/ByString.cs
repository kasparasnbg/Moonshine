/*
 * Tencent is pleased to support the open source community by making xLua available.
 * Copyright (C) 2016 THL A29 Limited, a Tencent company. All rights reserved.
 * Licensed under the MIT License (the "License"); you may not use this file except in compliance with the License. You may obtain a copy of the License at
 * http://opensource.org/licenses/MIT
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the specific language governing permissions and limitations under the License.
*/

using UnityEngine;
using System.Collections;
using XLua;
using System.Diagnostics;
using System;

namespace Tutorial
{
    public class ByString : MonoBehaviour
    {
        LuaEnv luaenv = null;
   


        void Start()
        {
            StartCoroutine(PerformanceTests());
        }

        IEnumerator PerformanceTests()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);

                PerformanceTestEmptyLUA();
            }
        }

        private void PerformanceTestEmptyLUA()
        {
            const string code = @"    
		    local x = 10
            local y = 5
  
	        return x + y
	    ";
            luaenv = new LuaEnv();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var returnVariables = luaenv.DoString(code);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            UnityEngine.Debug.Log($"LUA RunTime {returnVariables[0]} ticks {ts.Ticks}");
        }

        // Update is called once per frame
        void Update()
        {
            if (luaenv != null)
            {
                luaenv.Tick();
            }
        }

        void OnDestroy()
        {
            luaenv.Dispose();
        }
    }
}
