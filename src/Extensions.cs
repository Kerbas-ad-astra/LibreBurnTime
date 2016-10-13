//
// This file is part of LiberatedBurnTime.
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

using UnityEngine;

namespace LiberatedBurnTime
{
    /// <summary>
    /// Various utility extension methods.
    /// </summary>
    static class Extensions
    {
        /// <summary>
        /// Returns true if the part has a module of the specified class (or any
        /// subclass thereof).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="part"></param>
        /// <returns></returns>
        public static bool HasModule<T> (this Part part) where T : PartModule
        {
            if (part == null) return false;
            for (int index = 0; index < part.Modules.Count; ++index)
            {
                if (part.Modules[index] is T) return true;
            }
            return false;
        }

        /// <summary>
        /// Gets a unit vector pointing in the engine's "forward" direction
        /// (opposite its thrust direction).
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static Vector3 Forward(this ModuleEngines engine)
        {
            Vector3 sum = Vector3.zero;
            if ((engine == null) || (engine.thrustTransforms.Count == 0)) return sum;
            for (int index = 0; index < engine.thrustTransforms.Count; ++index)
            {
                sum += engine.thrustTransforms[index].forward;
            }
            return sum.normalized;
        }

        /// <summary>
        /// Gets the current max thrust of the engine in kilonewtons, taking thrust limiter into account.
        /// </summary>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static double ThrustLimit(this ModuleEngines engine)
        {
            if (engine == null) return 0.0;
            return engine.minThrust + (engine.maxThrust - engine.minThrust) * engine.thrustPercentage * 0.01;
        }

        /// <summary>
        /// Determines whether the specified vessel is a kerbal on EVA.
        /// </summary>
        public static bool IsEvaKerbal(this Vessel vessel)
        {
            if (vessel == null) return false;
            if (vessel.parts.Count != 1) return false;
            return vessel.parts[0].HasModule<KerbalEVA>();
        }
    }
}
