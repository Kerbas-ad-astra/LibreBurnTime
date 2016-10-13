//
// This file is part of LibreBurnTime.
//
//  Copyright (c) 2016 Kerbas-ad-astra
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using UnityEngine.UI;
using TMPro;

namespace LibreBurnTime
{
    /// <summary>
    /// Provides a safe wrapper to avoid getting NullReferenceExceptions from Unity 5. See comments
    /// on their bizarre, insane implementation of operator== here:
    /// http://blogs.unity3d.com/2014/05/16/custom-operator-should-we-keep-it/
    /// ...the moral of the story being, if you have a persistent reference to a UI object,
    /// you have to check it for "== null" every time you want to use it.
    /// </summary>
    class SafeText
    {
        private TextMeshProUGUI text;

        private SafeText(TextMeshProUGUI text)
        {
            this.text = text;
        }

        /// <summary>
        /// Get a new SafeText that wraps the specified text object.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SafeText of(TextMeshProUGUI text)
        {
            // use ReferenceEquals here because Unity does weird overloading of operator ==
            if (Object.ReferenceEquals(text, null)) throw new ArgumentNullException("text cannot be null");
            return new SafeText(text);
        }

        public void Destroy()
        {
            if (text != null)
            {
                UnityEngine.Object.Destroy(text);
                text = null;
            }
        }

        /// <summary>
        /// Gets or sets the text value.
        /// </summary>
        public string Text
        {
            get { return IsNullText ? string.Empty : text.text; }
            set { if (!IsNullText) text.text = value; }
        }

        /// <summary>
        /// Gets or sets whether the text is currently enabled.
        /// </summary>
        public bool Enabled
        {
            get { return IsNullText ? false : text.enabled; }
            set { if (!IsNullText) text.enabled = value; }
        }

        /// <summary>
        /// Gets/sets whether the text is currently in an error state and shouldn't be touched.
        /// </summary>
        private bool IsNullText
        {
            get
            {
                // Note, we have to check this even though text is a readonly variable that we
                // established as non-null at construction time, due to stupid Unity design.
                // We're calling the bizarrely-overloaded operator == here, not an actual == null.
                return text == null;
            }
        }
    }
}
