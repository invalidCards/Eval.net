﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eval.net
{
    public partial class EvalConfiguration
    {
        public static readonly EvalConfiguration FloatConfiguration = new EvalConfiguration(typeof(float));
        public static readonly EvalConfiguration DoubleConfiguration = new EvalConfiguration(typeof(double));
        public static readonly EvalConfiguration DecimalConfiguration = new EvalConfiguration(typeof(decimal));

        private Type _NumericType;
        public Type NumericType { get { return _NumericType; } }

        internal string[] _AllOperators;

        private string[][] _OperatorOrder;
        public string[][] OperatorOrder
        {
            get { return _OperatorOrder; }
            set
            {
                _OperatorOrder = value;

                var ops = new List<string>();
                foreach (var ops2 in _OperatorOrder)
                {
                    ops.AddRange(ops2);
                }
                _AllOperators = ops.ToArray();
            }
        }
        
        public string[] PrefixOperators { get; set; }
        public string[] SuffixOperators { get; set; }

        public Dictionary<string, object> GenericConstants { get; set; }
        public Dictionary<string, EvalFunction> GenericFunctions { get; set; }
        public Dictionary<string, object> Constants { get; set; }
        public Dictionary<string, EvalFunction> Functions { get; set; }

        public void SetConstant(string name, object value)
        {
            if (Constants == null)
            {
                Constants = new Dictionary<string, object>();
            }

            Constants[name] = value;
        }

        public void RemoveConstant(string name)
        {
            if (Constants == null) return;

            Constants.Remove(name);
        }

        public void SetFunction(string name, EvalFunction func)
        {
            if (Functions == null)
            {
                Functions = new Dictionary<string, EvalFunction>();
            }

            Functions[name] = func;
        }

        public void RemoveFunction(string name)
        {
            if (Functions == null) return;

            Functions.Remove(name);
        }

        public void ClearConstants()
        {
            Constants.Clear();
        }

        public void ClearFunctions()
        {
            Functions.Clear();
        }

        public EvalConfiguration(Type numericType)
        {
            _NumericType = numericType;

            OperatorOrder = DefaultOperatorOrder;
            PrefixOperators = DefaultPrefixOperators;
            SuffixOperators = DefaultSuffixOperators;
            GenericConstants = new Dictionary<string, object>(DefaultGenericConstants);
            GenericFunctions = new Dictionary<string, EvalFunction>(GetDefaultGenericFunctions(_NumericType));
            Constants = new Dictionary<string, object>();
            Functions = new Dictionary<string, EvalFunction>();
        }

        public EvalConfiguration Clone(bool deep = false)
        {
            var config = new EvalConfiguration(NumericType);
            config.OperatorOrder = OperatorOrder;
            config.PrefixOperators = PrefixOperators;
            config.SuffixOperators = SuffixOperators;
            config.GenericConstants = deep ? new Dictionary<string, object>(GenericConstants) : GenericConstants;
            config.GenericFunctions = deep ? new Dictionary<string, EvalFunction>(GenericFunctions) : GenericFunctions;
            config.Constants = new Dictionary<string, object>(Constants);
            config.Functions = new Dictionary<string, EvalFunction>(Functions);
            return config;
        }
    }
}
