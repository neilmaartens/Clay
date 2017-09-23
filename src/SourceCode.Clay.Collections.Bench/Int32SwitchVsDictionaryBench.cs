﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using SourceCode.Clay.Collections.Generic;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Bench
{
    public class Int32SwitchVsDictionaryBench
    {
        private const int ItemCount = 50;
        private const int InvokeCount = 1000;

        private readonly Dictionary<int, int> dict;
        private readonly IDynamicSwitch<int, int> @switch;

        public Int32SwitchVsDictionaryBench()
        {
            // Build dictionary
            dict = new Dictionary<int, int>(ItemCount);
            for (var i = 0; i < ItemCount; i++)
                dict[i] = i * 2;

            // Build switch
            @switch = dict.ToDynamicSwitch();
        }

        [Benchmark(Baseline = true, OperationsPerInvoke = ItemCount * InvokeCount)]
        public int Lookup()
        {
            var total = 0;
            for (var j = 0; j < InvokeCount; j++)
            {
                for (var i = dict.Count - 1; i >= 0; i--)
                {
                    unchecked
                    {
                        total += dict[i];
                    }
                }
            }

            return total;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = ItemCount * InvokeCount)]
        public int Switch()
        {
            var total = 0;
            for (var j = 0; j < InvokeCount; j++)
            {
                for (var i = dict.Count - 1; i >= 0; i--)
                {
                    unchecked
                    {
                        total += @switch[i];
                    }
                }
            }

            return total;
        }
    }
}