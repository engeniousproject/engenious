﻿using System;
using System.Collections;
using System.Collections.Generic;
using engenious.Helper;

namespace engenious.Graphics
{
	/// <summary>
	/// A collection of <see cref="EffectParameter"/>.
	/// </summary>
	public sealed class EffectParameterCollection : IEnumerable<EffectParameter>
	{
		private readonly Dictionary<string,EffectParameter> _parameters;
		private readonly List<EffectParameter> _parameterList;

		/// <summary>
		/// Initializes a new instance of the <see cref="EffectParameterCollection"/> class.
		/// </summary>
		/// <param name="techniques">A collection of techniques these parameters apply to.</param>
		public EffectParameterCollection (EffectTechniqueCollection techniques)
		{
		    using (Execute.OnUiContext)
		    {
		        _parameters = new Dictionary<string, EffectParameter>();
		        _parameterList = new List<EffectParameter>();

		        foreach (var technique in techniques)
		        {
		            foreach (var pass in technique.Passes)
		            {
		                pass.CacheParameters();

		                foreach (var param in pass.Parameters)
		                {
		                    EffectParameter current;
		                    if (!_parameters.TryGetValue(param.Name, out current))
		                    {
		                        current = new EffectParameter(param.Name);
		                        Add(current);
		                    }
		                    current.Add(param);
		                }
		            }
		        }
		    }
		}

		/// <summary>
		/// Adds a new parameter to this collection.
		/// </summary>
		/// <param name="parameter">The parameter to add.</param>
		internal void Add (EffectParameter parameter)
		{
			_parameterList.Add (parameter);
			_parameters.Add (parameter.Name, parameter);
		}

		/// <summary>
		/// Gets an element in the collection by using an index value.
		/// </summary>
		/// <param name="index">The element index.</param>
		public EffectParameter this [int index] { 
			get {
				return _parameterList [index];
			} 
		}

		/// <summary>
		/// Gets an element in the collection by using a name.
		/// </summary>
		/// <param name="name">The name to search for.</param>
		public EffectParameter this [string name] { 
			get {
				return _parameters [name];
			} 
		}

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _parameterList.GetEnumerator();
        }
        IEnumerator<EffectParameter> IEnumerable<EffectParameter>.GetEnumerator()
        {
            return _parameterList.GetEnumerator();
        }

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
		public List<EffectParameter>.Enumerator GetEnumerator()
		{
			return _parameterList.GetEnumerator();
		}
    }
}

