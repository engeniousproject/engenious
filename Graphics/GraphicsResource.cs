using System;

namespace engenious.Graphics
{
	public abstract class GraphicsResource : IDisposable
	{
		internal GraphicsResource()
		{
		}
		internal GraphicsResource (GraphicsDevice graphicsDevice)
		{
			this.GraphicsDevice = graphicsDevice;
		}
		public GraphicsDevice GraphicsDevice{ get; internal set;}
		public string Name { get; set; }
		public object Tag{get;set;}
		public bool IsDisposed{ get; private set;}=false;

		public virtual void Dispose()
		{
			if (IsDisposed)
				return;
			
			IsDisposed = true;
		}
	}
}

