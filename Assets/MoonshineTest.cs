using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MoonshineTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
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
        }
    }
    void PerformanceTestAdditionLUA()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        string script = @"    
		    local x = 10
            local y = 5
  
	        return x + y
	    ";
        DynValue res = Script.RunString(script);

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        UnityEngine.Debug.Log($"LUA addition RunTime {ts.Ticks} result {res.Number}");
    }
    void PerformanceTestEmptyLUA()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        string script = @"    
		-- defines a factorial function
	    ";
        Script.RunString(script);

        stopWatch.Stop();
        TimeSpan ts = stopWatch.Elapsed;

        UnityEngine.Debug.Log($"LUA empty RunTime {ts.Ticks}");
    }

    void PerformanceTestFactorialLUA()
    {
        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();
        double total = 0;

        total += MoonSharpFactorial();

        stopWatch.Stop();

        TimeSpan ts = stopWatch.Elapsed;

        UnityEngine.Debug.Log($"LUA RunTime {ts.Ticks} total {total}");

    }



    double MoonSharpFactorial()
    {
        string script = @"    
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


        DynValue res = Script.RunString(script);
        return res.Number;
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
