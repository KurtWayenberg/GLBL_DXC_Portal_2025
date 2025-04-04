using System;

namespace DXC.Technology.Entity
{
    public interface ICursor
    {
        /// <summary>
        /// Gets or sets the selected index. Throws if set to an invalid index.
        /// </summary>
        int SelectedIndex { get; set; }

        /// <summary>
        /// Checks if the currently selected index can be moved down.
        /// </summary>
        bool CanMoveDown();

        /// <summary>
        /// Checks if the currently selected index can be moved up.
        /// </summary>
        bool CanMoveUp();

        /// <summary>
        /// Checks if the currently selected index can be moved to the top.
        /// </summary>
        bool CanMoveTop();

        /// <summary>
        /// Checks if the currently selected index can be moved to the bottom.
        /// </summary>
        bool CanMoveBottom();

        /// <summary>
        /// Gets the total number of fetched elements.
        /// </summary>
        int EntitiesCount { get; }

        /// <summary>
        /// Moves the selection to the next item if possible.
        /// </summary>
        void MoveUp();

        /// <summary>
        /// Moves the selection to the previous item if possible.
        /// </summary>
        void MoveDown();

        /// <summary>
        /// Moves the selection to the first item if possible.
        /// </summary>
        void MoveTop();

        /// <summary>
        /// Moves the selection to the last item if possible.
        /// </summary>
        void MoveBottom();
    }

    public class EntityCursor<T> : ICursor where T : class
    {
        #region Private Fields

        /// <summary>
        /// The array of items managed by the cursor.
        /// </summary>
        private T[] items;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityCursor{T}"/> class.
        /// </summary>
        /// <param name="items">The array of items to manage.</param>
        public EntityCursor(T[] items = null)
        {
            this.items = items;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the selected index. Throws if set to an invalid index.
        /// </summary>
        public int SelectedIndex { get; set; } = -1; // No item selected by default

        /// <summary>
        /// Gets the total number of elements. Returns 0 if items are null.
        /// </summary>
        public int EntitiesCount => items?.Length ?? 0;

        #endregion

        #region Public Methods

        /// <summary>
        /// Checks if the currently selected index is valid.
        /// </summary>
        /// <returns>True if the selected index is valid; otherwise, false.</returns>
        public bool IsItemSelected()
        {
            return SelectedIndex >= 0 && items != null && SelectedIndex < items.Length;
        }

        /// <summary>
        /// Checks if the currently selected item can be moved up.
        /// </summary>
        /// <returns>True if the selected item can be moved up; otherwise, false.</returns>
        public bool CanMoveUp()
        {
            return IsItemSelected() && (SelectedIndex > 0);
        }

        /// <summary>
        /// Checks if the currently selected item can be moved down.
        /// </summary>
        /// <returns>True if the selected item can be moved down; otherwise, false.</returns>
        public bool CanMoveDown()
        {
            return IsItemSelected() && (SelectedIndex < items.Length - 1);
        }

        /// <summary>
        /// Checks if the currently selected item can be moved to the top.
        /// </summary>
        /// <returns>True if the selected item can be moved to the top; otherwise, false.</returns>
        public bool CanMoveTop()
        {
            return IsItemSelected() && (SelectedIndex > 0);
        }

        /// <summary>
        /// Checks if the currently selected item can be moved to the bottom.
        /// </summary>
        /// <returns>True if the selected item can be moved to the bottom; otherwise, false.</returns>
        public bool CanMoveBottom()
        {
            return IsItemSelected() && (SelectedIndex < items.Length - 1);
        }

        /// <summary>
        /// Gets the currently selected element. Throws if no item is selected.
        /// </summary>
        /// <returns>The currently selected element.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no item is currently selected.</exception>
        public T GetSelectedElement()
        {
            if (!IsItemSelected())
                throw new InvalidOperationException("No item is currently selected.");
            return items[SelectedIndex];
        }

        /// <summary>
        /// Moves the selection to the next item if possible.
        /// </summary>
        public void MoveDown()
        {
            if (IsItemSelected() && SelectedIndex < items.Length - 1)
                SelectedIndex++;
        }

        /// <summary>
        /// Moves the selection to the previous item if possible.
        /// </summary>
        public void MoveUp()
        {
            if (IsItemSelected() && SelectedIndex > 0)
                SelectedIndex--;
        }

        /// <summary>
        /// Moves the selection to the first item if possible.
        /// </summary>
        public void MoveTop()
        {
            if (items != null && items.Length > 0)
                SelectedIndex = 0;
        }

        /// <summary>
        /// Moves the selection to the last item if possible.
        /// </summary>
        public void MoveBottom()
        {
            if (items != null && items.Length > 0)
                SelectedIndex = items.Length - 1;
        }

        /// <summary>
        /// Updates the items managed by the cursor and optionally sets a new selected index.
        /// </summary>
        /// <param name="items">The new array of items to manage.</param>
        /// <param name="newSelectedIndex">The new selected index. Defaults to -1.</param>
        public void UpdateItems(T[] items, int newSelectedIndex = -1)
        {
            this.items = items;
            if (this.items == null || this.items.Length == 0 || this.items.Length < SelectedIndex)
                SelectedIndex = -1; // No item selected if mismatch
        }

        #endregion
    }
}