using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using XLua;

public class MoonshineTest : MonoBehaviour
{
    // Start is called before the first frame update
    LuaEnv luaenv;
    void Start()
    {
        luaenv = new LuaEnv();
        StartCoroutine(PerformanceTests());
    }

    IEnumerator PerformanceTests()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            PerformanceTestCSharp();
            PerformanceTestFactorialLUA();
            PerformanceTestEmptyLUA();
            PerformanceTestAdditionLUA();
            PerformanceTestAdditionCSharp();
        }
    }
    void PerformanceTestAdditionCSharp()
    {
        // Vars
        double Add(double x, double y)
        {
            return x + y;
        }
        const int iterations = 100;
        double counter = 0.0;

        // Run the script
        var time = Time.realtimeSinceStartupAsDouble;
        for (int i = 0; i < iterations; ++i)
        {
            double result = Add(10, 5);
            counter += result;
        }
        time = Time.realtimeSinceStartupAsDouble - time;


        UnityEngine.Debug.Log($"C# addition RunTime {time * 1000.0} ms. Result (x{iterations}) {counter}");
    }
    void PerformanceTestAdditionLUA()
    {
        // Vars
        const string code = @"    
		    local x = 10
            local y = 5
  
	        return x + y
	    ";
        const int iterations = 100;
        long counter = 0;

        // Parse the script
        var chunk = luaenv.LoadString(code);

        // Run the script
        GC.Collect();

        var time = Time.realtimeSinceStartupAsDouble;
        for (int i = 0; i < iterations; ++i)
        {
            var results = chunk.Call();
            counter += (long)results[0];
        }
        time = Time.realtimeSinceStartupAsDouble - time;

        UnityEngine.Debug.Log($"LUA addition RunTime {time * 1000.0} ms. Result (x{iterations}) {counter}");
    }
    void PerformanceTestEmptyLUA()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        string script = @"    
		-- defines a factorial function
	    ";

        luaenv.DoString(script);

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        UnityEngine.Debug.Log($"LUA empty RunTime {ts.Ticks}");
    }

    void PerformanceTestFactorialLUA()
    {
        string code = @"    
		-- defines a factorial function
		function fact (n)
			if (n == 0) then
				return 1
			else
				return n*fact(n - 1)
			end
		end

    
        local total = 0
        local i = 1000
        while(i > 0)
        do
    	    total = total + fact(5)
    	    i = i-1
	    end
	
	    return total";

        var chunk = luaenv.LoadString(code);
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        
        long total = (long)chunk.Call()[0];

        stopWatch.Stop();

        TimeSpan ts = stopWatch.Elapsed;

        UnityEngine.Debug.Log($"LUA RunTime {ts.Ticks} total {total}");

    }


    void PerformanceTestCSharp()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        double total = 0;
        for (int i = 0; i < 1000; i++)
        {
            total += Factorial(5);
        }
        stopWatch.Stop();

        TimeSpan ts = stopWatch.Elapsed;
        UnityEngine.Debug.Log($"CSharp RunTime {ts.Ticks} total {total}");

    }

    double Factorial(double n)
    {
        if (n == 0)
            return 1;
        else
            return n * Factorial(n - 1);
    }
}
