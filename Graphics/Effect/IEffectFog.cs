using System;
using OpenTK;

namespace engenious
{
	public interface IEffectFog
	{
		Vector3 FogColor{get;set;}
		float FogStart{get;set;}
		float FogEnd{get;set;}
		bool FogEnabled{get;set;}
	}
}

