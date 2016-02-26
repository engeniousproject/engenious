using System;
using System.Collections.Generic;
using System.Collections;

namespace engenious.Graphics
{
	public sealed class EffectPassCollection : IEnumerable<EffectPass>
	{
		private List<EffectPass> passesList;
		private Dictionary<string,EffectPass> passes;
		public EffectPassCollection ()
		{
			passesList = new List<EffectPass> ();
			passes = new Dictionary<string, EffectPass> ();
		}
		internal void Add(EffectPass pass)
		{
			passesList.Add (pass);
			passes.Add (pass.Name, pass);
		}
		public EffectPass this [int index] { 
			get {
				return passesList[index];
			} 
		}
		public EffectPass this [string name] { 
			get {
				return passes[name];
			} 
		}
		IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return passesList.GetEnumerator ();
		}
		public IEnumerator<EffectPass> GetEnumerator ()
		{
			return passesList.GetEnumerator ();
		}
	}
}

