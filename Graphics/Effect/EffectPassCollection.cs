using System.Collections;
using System.Collections.Generic;

namespace engenious.Graphics
{
	/// <summary>
	/// A collection of <see cref="EffectPass"/>.
	/// </summary>
	public sealed class EffectPassCollection : IEnumerable<EffectPass>
	{
		private readonly List<EffectPass> _passesList;
		private readonly Dictionary<string,EffectPass> _passes;

		/// <summary>
		/// Initializes a new instance of the <see cref="EffectPassCollection"/> class.
		/// </summary>
		public EffectPassCollection ()
		{
			_passesList = new List<EffectPass> ();
			_passes = new Dictionary<string, EffectPass> ();
		}
		internal void Add(EffectPass pass)
		{
			_passesList.Add (pass);
			_passes.Add (pass.Name, pass);
		}

		/// <summary>
		/// Gets an element in the collection by using an index value.
		/// </summary>
		/// <param name="index">The element index.</param>
		public EffectPass this [int index] => _passesList[index];

		/// <summary>
		/// Gets an element in the collection by using a name.
		/// </summary>
		/// <param name="name">The pass name to search for.</param>
	    public EffectPass this [string name] => _passes[name];

		IEnumerator<EffectPass> IEnumerable<EffectPass>.GetEnumerator()
		{
			return _passesList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _passesList.GetEnumerator();
		}

		/// <inheritdoc cref="IEnumerable.GetEnumerator"/>
		public List<EffectPass>.Enumerator GetEnumerator()
		{
			return _passesList.GetEnumerator();
		}
	}
}

