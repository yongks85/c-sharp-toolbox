// (c) IMT - Information Management Technology AG, CH-9470 Buchs, www.imt.ch
// SW guideline: Technote Coding Guidelines Ver. 1.6

using System;

namespace Utilities
{

    /// <summary>
    /// Object that implements the dispose pattern
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {

        /// <summary>
        /// Is true is this object is disposed.
        /// </summary>
        // Enable if object is serializable
        //[Serialize(Ignore = true)]
        //[DynamicColumn(Ignore = true)]
        public bool IsDisposed { get; private set; }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        /// <summary>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees any unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanaged() { }

        /// <summary>
        /// Frees any managed resources, your derived class has to override this in case:
        /// - the class manages any IDisposable objects
        /// - the class adds any event handlers
        /// - need to set references to to null to facilitate memory leak analysis via memory snapshot reference counts.
        /// Here, any event that has been registered through RegisterPropertyChangeHandler will be released.
        /// </summary>
        protected virtual void DisposeManaged() { }

        private void Dispose(bool isDisposing)
        {
            if (IsDisposed) return;

            // Free any managed resources in this section
            if (isDisposing) DisposeManaged();

            // Free any unmanaged resources in this section
            DisposeUnmanaged();
            IsDisposed = true;
        }
    }
}