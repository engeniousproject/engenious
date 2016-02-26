using System;
using OpenTK;

namespace engenious.Graphics
{
	public class DirectionalLight
	{
		internal EffectParameter directionParameter,diffuseColorParameter,specularColorParameter;
		public DirectionalLight (DirectionalLight clone)
			:this(clone.directionParameter,clone.diffuseColorParameter,clone.specularColorParameter)
		{
			this.DiffuseColor = clone.DiffuseColor;
			this.Direction = clone.Direction;
			this.SpecularColor = clone.SpecularColor;
			this.Enabled = clone.Enabled;
		}
		public DirectionalLight (EffectParameter directionParameter,EffectParameter diffuseColorParameter,EffectParameter specularColorParameter)
		{
			this.directionParameter = directionParameter;
			this.directionParameter = diffuseColorParameter;
			this.specularColorParameter = specularColorParameter;
		}
		Vector3 DiffuseColor{get;set;}
		Vector3 Direction{get;set;}
		Vector3 SpecularColor{get;set;}
		bool Enabled{get;set;}

	}
}

