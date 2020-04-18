using System;
using System.Collections.Generic;
using System.Collections;

namespace engenious.Graphics
{
	/// <summary>
	/// A collection of <see cref="EffectTechnique"/>.
	/// </summary>
    public sealed class EffectTechniqueCollection : IEnumerable<EffectTechnique>
    {
		private readonly Dictionary<string,EffectTechnique> _techniques;
	    private readonly List<EffectTechnique> _techniqueList;

	    /// <summary>
	    /// Initializes a new instance of the <see cref="EffectTechniqueCollection"/> class.
	    /// </summary>
		public EffectTechniqueCollection ()
		{
			_techniques = new Dictionary<string, EffectTechnique> ();
			_techniqueList = new List<EffectTechnique> ();
		}

		internal void Add (EffectTechnique technique)
		{
			_techniques.Add (technique.Name, technique);
			_techniqueList.Add (technique);
		}

		/// <summary>
		/// Gets an element in the collection by using an index value.
		/// </summary>
		/// <param name="index">The element index.</param>
		public EffectTechnique this [int index] => _techniqueList [index];

		/// <summary>
		/// Gets an element in the collection by using a name.
		/// </summary>
		/// <param name="name">The name to search for.</param>
        public EffectTechnique this [string name] => _techniques [name];

	    IEnumerator<EffectTechnique> IEnumerable<EffectTechnique>.GetEnumerator()
	    {
		    return _techniqueList.GetEnumerator();
	    }

	    IEnumerator IEnumerable.GetEnumerator()
	    {
		    return _techniqueList.GetEnumerator();
	    }

	    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
	    public List<EffectTechnique>.Enumerator GetEnumerator()
	    {
		    return _techniqueList.GetEnumerator();
	    }
    }
}

