using System;
using System.Collections.Generic;

namespace engenious
{
    public sealed class GameComponentCollection : ICollection<GameComponent>
    {
        private List<IDrawable> drawables;
        private List<IUpdateable> updateables;
        private List<GameComponent> components;

        public GameComponentCollection()
        {
            drawables = new List<IDrawable>();
            updateables = new List<IUpdateable>();
            components = new List<GameComponent>();
        }

        internal List<IUpdateable> Updatables
        {
            get
            {
                return updateables;
            }
        }

        internal List<IDrawable> Drawables
        {
            get
            {
                return drawables;
            }
        }


        internal void Sort()
        {
            updateables.Sort((x, y) =>
                {
                    if (x.Enabled && y.Enabled)
                        return x.UpdateOrder.CompareTo(y.UpdateOrder);
                    return -x.Enabled.CompareTo(y.Enabled);
                });
            drawables.Sort((x, y) =>
                {
                    if (x.Visible && y.Visible)
                        return x.DrawOrder.CompareTo(y.DrawOrder);
                    return -x.Visible.CompareTo(y.Visible);
                });
        }

        #region ICollection implementation

        public void Add(GameComponent item)
        {
            IDrawable drawable = item as IDrawable;
            if (drawable != null)
                drawables.Add(drawable);
            IUpdateable updateable = item as IUpdateable;
            if (updateable != null)
                updateables.Add(updateable);
            components.Add(item);
        }

        public void Clear()
        {
            drawables.Clear();
            updateables.Clear();
            components.Clear();
        }

        public bool Contains(GameComponent item)
        {
            return components.Contains(item);
        }

        public void CopyTo(GameComponent[] array, int arrayIndex)
        {
            components.CopyTo(array, arrayIndex);
        }

        public bool Remove(GameComponent item)
        {

            IDrawable drawable = item as IDrawable;
            if (drawable != null)
                drawables.Remove(drawable);
            IUpdateable updateable = item as IUpdateable;
            if (updateable != null)
                updateables.Remove(updateable);
            
            return components.Remove(item);
        }

        public int Count
        {
            get
            {
                return components.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable implementation

        public IEnumerator<GameComponent> GetEnumerator()
        {
            return components.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return components.GetEnumerator();
        }

        #endregion
    }
}

