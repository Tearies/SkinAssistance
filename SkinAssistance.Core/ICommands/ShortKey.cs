// ***********************************************************************
// Assembly         : VBI.Player.Core
// Author           : Tearies
// Created          : 08-21-2017
//
// Last Modified By : Tearies
// Last Modified On : 08-21-2017
// ***********************************************************************
// <copyright file="ShortKey.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Windows.Input;

namespace SkinAssistance.Core.ICommands
{
    /// <summary>
    /// Class ShortKey.
    /// </summary>
    public class ShortKey
    {
        /// <summary>
        /// The modifier keys value serializer
        /// </summary>
        private static readonly ModifierKeysValueSerializer ModifierKeysValueSerializer = new ModifierKeysValueSerializer();
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        private ShortKey()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortKey"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="modifierKey">The modifier key.</param>
        public ShortKey(Key key, ModifierKeys modifierKey) : this()
        {
            Key = key;
            ModifierKey = modifierKey;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortKey"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public ShortKey(Key key) : this(key, ModifierKeys.None)
        {

        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public Key Key { get; private set; }

        /// <summary>
        /// Gets the modifier key.
        /// </summary>
        /// <value>The modifier key.</value>
        public ModifierKeys ModifierKey { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ModifierKeysValueSerializer.ConvertToString(ModifierKey, null) + "+" + Key;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var tempobj = obj as ShortKey;
            if (tempobj == null)
                return false;
            return this.Key == tempobj.Key && this.ModifierKey == tempobj.ModifierKey;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool Equals(ShortKey other)
        {
            return Key == other.Key && ModifierKey == other.ModifierKey;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((int)Key * 397) ^ (int)ModifierKey;
            }
        }
    }
}