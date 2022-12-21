using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SunLibrary.SunFileLib.Structure
{
    [DataContract]
    public struct SunBool
    {
        public const byte NotExist = 0;
        public const byte False = 1;
        public const byte True = 2;

        [DataMember]
        private byte val { get; set; }
        public static implicit operator SunBool(byte value)
        {
            return new SunBool
            {
                val = value
            };
        }

        public static implicit operator SunBool(bool? value)
        {
            return new SunBool
            {
                val = value == null ? SunBool.NotExist : (bool)value ? SunBool.True : SunBool.False
            };
        }

        public static implicit operator bool(SunBool value)
        {
            return value == SunBool.True;
        }

        public static implicit operator byte(SunBool value)
        {
            return value.val;
        }

        public override bool Equals(object obj)
        {
            return obj is SunBool ? ((SunBool)obj).val.Equals(val) : false;
        }

        public override int GetHashCode()
        {
            return val.GetHashCode();
        }

        public static bool operator ==(SunBool a, SunBool b)
        {
            return a.val.Equals(b.val);
        }

        public static bool operator ==(SunBool a, bool b)
        {
            return (b && (a.val == SunBool.True)) || (!b && (a.val == SunBool.False));
        }

        public static bool operator !=(SunBool a, SunBool b)
        {
            return !a.val.Equals(b.val);
        }

        public static bool operator !=(SunBool a, bool b)
        {
            return (b && (a.val != SunBool.True)) || (!b && (a.val != SunBool.False));
        }

        public bool HasValue
        {
            get
            {
                return val != NotExist;
            }
        }

        public bool Value
        {
            get
            {
                switch (val)
                {
                    case False:
                        return false;
                    case True:
                        return true;
                    case NotExist:
                    default:
                        throw new Exception("Tried to get value of nonexistant bool");
                }
            }
        }
    }
}
