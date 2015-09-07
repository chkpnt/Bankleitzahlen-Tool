﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bankleitzahlen.Models
{
    public class Bank
    {
        public string Name { get; set; }
        public BLZ Bankleitzahl { get; set; }
        public CivicAddress Adresse { get; set; }
    }

    public class BLZ
    {
        private int _value;

        public BLZ(int blz)
        {
            this._value = blz;
        }

        public static implicit operator BLZ(int blz) => new BLZ(blz);

        public static explicit operator int(BLZ blz) => blz._value;
    }

}