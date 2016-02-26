using System;
using System.Collections.Generic;
using System.Collections;

namespace engenious.Graphics
{
	public sealed class EffectTechniqueCollection :IEnumerable<EffectTechnique>
	{
		private Dictionary<string,EffectTechnique> techniques;
		private List<EffectTechnique> techniqueList;

		public EffectTechniqueCollection ()
		{
			techniques = new Dictionary<string, EffectTechnique> ();
			techniqueList = new List<EffectTechnique> ();
		}

		internal void Add (EffectTechnique technique)
		{
			techniques.Add (technique.Name, technique);
			techniqueList.Add (technique);
		}

		public EffectTechnique this [int index] { 
			get {
				return techniqueList [index];
			} 
		}

		public EffectTechnique this [string name] { 
			get {
				return techniques [name];
			} 
		}

		IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return techniqueList.GetEnumerator ();	
		}

		public IEnumerator<EffectTechnique> GetEnumerator ()
		{
			return techniqueList.GetEnumerator ();	
		}
	}
}

