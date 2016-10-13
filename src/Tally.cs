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
using System.Collections.Generic;

namespace LibreBurnTime
{
    class Tally
    {
        private readonly Dictionary<String, double> values = new Dictionary<string, double>();

        /// <summary>
        /// Sets all values to zero.
        /// </summary>
        public void Zero()
        {
            foreach (String key in values.Keys)
            {
                values[key] = 0.0F;
            }
        }

        /// <summary>
        /// Adds the specified amount to the key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="amount"></param>
        public void Add(String key, double amount)
        {
            if (values.ContainsKey(key))
            {
                values[key] += amount;
            } else
            {
                values[key] = amount;
            }
        }

        public double Sum
        {
            get
            {
                double total = 0.0;
                foreach (double value in values.Values)
                {
                    total += value;
                }
                return total;
            }
        }

        /// <summary>
        /// Returns true if the specified key is present in a nonzero amount.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Has(String key)
        {
            return values.ContainsKey(key) && (values[key] > 0.0);
        }

        /// <summary>
        /// Gets all the keys.
        /// </summary>
        public ICollection<String> Keys
        {
            get { return values.Keys;  }
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public double this[String key]
        {
            get { return values[key]; }
        }

    }
}
