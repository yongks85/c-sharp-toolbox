using System.Collections.Generic;

namespace Utilities.Examples.Enummeration
{
    public class BitFlagSample: Utilities.Enummeration
    {
        public static readonly BitFlagSample UnexpectedError = new BitFlagSample(-1, "Unexpected Error has occurred");
        public static readonly BitFlagSample Ok = new BitFlagSample(0, "OK");
        public static readonly BitFlagSample Uninitialized = new BitFlagSample(1, "Not Initialized");
        public static readonly BitFlagSample InvalidArgument = new BitFlagSample(2, "Invalid Argument");
        public static readonly BitFlagSample ValueOutOfRange = new BitFlagSample(4, "Value out of range");
        public static readonly BitFlagSample InvalidState = new BitFlagSample(8, "Invalid state");

        /// <inheritdoc />
        private BitFlagSample(int index, string name) : base(index, name)
        {
            Default = () => UnexpectedError;
        }

        public static implicit operator BitFlagSample(int index) => FromIndex<BitFlagSample>(index);
        public static implicit operator BitFlagSample(string name) => FromName<BitFlagSample>(name);

        public IEnumerable<BitFlagSample> this[uint index] => AsBitFlags<BitFlagSample>(index);
        public BitFlagSample this[int index] => FromIndex<BitFlagSample>(index); 
        public BitFlagSample this[string name] => FromName<BitFlagSample>(name);

    }
}