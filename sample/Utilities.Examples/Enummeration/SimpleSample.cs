namespace Utilities.Examples.Enummeration
{
    public class SimpleSample : EnumAsClass<SimpleSample>
    {
        public static readonly SimpleSample SimpleSampleIndex1 = new SimpleSample(1, "Index A");
        public static readonly SimpleSample SimpleSampleIndex2 = new SimpleSample(2, "Index B");

        private SimpleSample(int index, string name) : base(index, name)
        {
            Default = () => SimpleSampleIndex1;
            //MultipleResultSameAsNoMatch = false;
            //ExecuteWhenNoMatch = () => NoMatchExecuted = true;
        }

        public bool NoMatchExecuted { get; private set; }


        public static implicit operator SimpleSample(int index) => FromIndex(index);
        public static implicit operator SimpleSample(string name) => FromName(name);
    }
}