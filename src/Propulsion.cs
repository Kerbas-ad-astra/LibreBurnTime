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
using UnityEngine;

namespace LibreBurnTime
{
    /// <summary>
    /// Keeps track of engines and propellant tanks on the active ship.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    class Propulsion : MonoBehaviour
    {
        private static Propulsion instance = null;

        private Guid lastVesselId;
        private int lastVesselPartCount;
        private List<Part> engines;
        private List<Part> tanks;

        public void Start()
        {
            instance = this;
            lastVesselId = Guid.Empty;
            lastVesselPartCount = -1;
            engines = new List<Part>();
            tanks = new List<Part>();
        }

        /// <summary>
        /// Called on every frame.
        /// </summary>
        public void Update()
        {
            // Track whether the vessel has changed since last update
            Vessel vessel = FlightGlobals.ActiveVessel;
            if (vessel == null) return;
            bool needRefresh = false;
            if (vessel.id != lastVesselId)
            {
                lastVesselId = vessel.id;
                lastVesselPartCount = vessel.parts.Count;
                needRefresh = true;
            }
            else if (vessel.parts.Count != lastVesselPartCount)
            {
                lastVesselPartCount = vessel.parts.Count;
                needRefresh = true;
            }

            if (needRefresh)
            {
                // Yes, it's changed. Update our status.
                ListEngines(vessel);
                ListFuelTanks(vessel);
            }
        }

        public static bool ShouldIgnorePropellant(string propellantName)
        {
            return "ElectricCharge".Equals(propellantName);
        }

        /// <summary>
        /// Gets all engines on the current ship (regardless of whether they're active
        /// or have available fuel).
        /// </summary>
        public static List<Part> Engines
        {
            get { return instance.engines; }
        }

        /// <summary>
        /// Gets all fuel tanks on the current ship (regardless of whether they are
        /// active or contain any fuel) that are capable of containing any of the
        /// fuel that the ship's engines use.
        /// </summary>
        public static List<Part> Tanks
        {
            get { return instance.tanks; }
        }

        /// <summary>
        /// Build a list of all engines on the vessel.
        /// </summary>
        /// <param name="vessel"></param>
        private void ListEngines(Vessel vessel)
        {
            engines.Clear();
            for (int index = 0; index < vessel.parts.Count; ++index)
            {
                Part part = vessel.parts[index];
                if (part.HasModule<ModuleEngines>())
                {
                    engines.Add(part);
                }
            }
        }

        /// <summary>
        /// Build a list of all fuel tanks that could potentially supply our engines.
        /// </summary>
        /// <param name="vessel"></param>
        private void ListFuelTanks(Vessel vessel)
        {
            // Build a list of all tanks that contain any resources of nonzero mass
            tanks.Clear();
            for (int index = 0; index < vessel.parts.Count; ++index)
            {
                Part part = vessel.parts[index];
                if (HasAnyResources(part))
                {
                    tanks.Add(part);
                }
            }
        }

        /// <summary>
        /// Returns true if the part contains any resources of nonzero mass.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="propellants"></param>
        /// <returns></returns>
        private static bool HasAnyResources(Part part)
        {
            for (int index = 0; index < part.Resources.Count; ++index)
            {
                if (part.Resources[index].info.density > 0) return true;
            }
            return false;
        }
    }
}
