// (c) IMT - Information Management Technology AG, CH-9470 Buchs, www.imt.ch.
// SW guideline: Technote Coding Guidelines Ver. 1.6

using System;

namespace Utilities.Examples.Enummeration {

    /// <summary>
    /// Store the different File extension types for easy referencing
    /// To standardize, make sure to add a . in front of the extension
    /// </summary>
    public class FileExtensions: EnumAsClass {
       
        public static FileExtensions Zip => new FileExtensions(1,".zip");

        public static FileExtensions _7zip => new FileExtensions(2,".7z");

        public static FileExtensions Json => new FileExtensions(3,".json");

        public static FileExtensions Config => new FileExtWithDefaultName(3, ".config", "App");

        /// <summary>Add the wild card symbol to the extension <example>*.{extension}</example> </summary>
        public string WithWildCard => "*" + Name;

        public virtual string WithDefaultFileName => throw new NotImplementedException();

        /// <summary> Auto cast to string </summary>
        public static implicit operator string(FileExtensions fileExtensions) => fileExtensions.Name;

        private FileExtensions(int index, string name) : base(index, name)
        {
            NoMatchResult = () => throw new NotImplementedException();
            MultipleResultSameAsNoMatch = true;
            ExecuteWhenNoMatch = () => throw new NotImplementedException();
        }

        private class FileExtWithDefaultName : FileExtensions
        {
            private readonly string _defaultFilename;
            internal FileExtWithDefaultName(int index, string name, string defaultFilename) : base(index, name)
            {
                _defaultFilename = defaultFilename;
            }

            public override string WithDefaultFileName => _defaultFilename + Name;
        }
    }

}
