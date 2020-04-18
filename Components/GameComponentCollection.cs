using System.Collections;
using System.Collections.Generic;

namespace engenious
{
    /// <summary>
    /// Defines a collection of game components.
    /// </summary>
    public sealed class GameComponentCollection : ICollection<GameComponent>
    {
        private readonly UpdateComparer _updateComparer;
        private readonly DrawComparer _drawComparer;
        private readonly List<GameComponent> _components;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameComponentCollection"/> class.
        /// </summary>
        public GameComponentCollection()
        {
            Drawables = new List<IDrawable>();
            Updatables = new List<IUpdateable>();
            _components = new List<GameComponent>();
            _updateComparer = new UpdateComparer();
            _drawComparer = new DrawComparer();
        }

        /// <summary>
        /// Gets the list of updateable components.
        /// </summary>
        internal List<IUpdateable> Updatables { get; }

        /// <summary>
        /// Gets the list of drawable components.
        /// </summary>
        internal List<IDrawable> Drawables { get; }


        internal void Sort()
        {
            Updatables.Sort(_updateComparer);
            Drawables.Sort(_drawComparer);
        }

        private class UpdateComparer : IComparer<IUpdateable>
        {
            public int Compare(IUpdateable x,IUpdateable y)
            {
                if (x == null || y == null)
                    return 0;
                if (x.Enabled && y.Enabled)
                    return x.UpdateOrder.CompareTo(y.UpdateOrder);
                return -x.Enabled.CompareTo(y.Enabled);
            }
        }
        private class DrawComparer : IComparer<IDrawable>
        {
            public int Compare(IDrawable x,IDrawable y)
            {
                if (x == null || y == null)
                    return 0;
                if (x.Visible && y.Visible)
                    return x.DrawOrder.CompareTo(y.DrawOrder);
                return -x.Visible.CompareTo(y.Visible);
            }
        }

        #region ICollection implementation

        /// <inheritdoc />
        public void Add(GameComponent item)
        {
            var drawable = item as IDrawable;
            if (drawable != null)
                Drawables.Add(drawable);
            IUpdateable updateable = item;
            if (updateable != null)
                Updatables.Add(updateable);
            _components.Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            Drawables.Clear();
            Updatables.Clear();
            _components.Clear();
        }

        /// <inheritdoc />
        public bool Contains(GameComponent item)
        {
            return _components.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(GameComponent[] array, int arrayIndex)
        {
            _components.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public bool Remove(GameComponent item)
        {

            var drawable = item as IDrawable;
            if (drawable != null)
                Drawables.Remove(drawable);
            IUpdateable updateable = item;
            if (updateable != null)
                Updatables.Remove(updateable);
            
            return _components.Remove(item);
        }

        /// <inheritdoc />
        public int Count => _components.Count;

        /// <inheritdoc />
        public bool IsReadOnly => false;

        #endregion

        #region IEnumerable implementation

        /// <inheritdoc />
        public IEnumerator<GameComponent> GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _components.GetEnumerator();
        }

        #endregion
    }
}

