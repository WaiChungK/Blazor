// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Microsoft.JSInterop
{
    /// <summary>
    /// Wraps a JS interop argument, indicating that the value should not be serialized as JSON
    /// but instead should be passed as a reference.
    ///
    /// To avoid leaking memory, the reference must later be disposed by JS code.
    /// </summary>
    public struct DotNetObject
    {
        /// <summary>
        /// Gets the object instance represented by this wrapper.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Constructs an instance of <see cref="DotNetObject"/>.
        /// </summary>
        /// <param name="value">The value being wrapped.</param>
        public DotNetObject(object value)
        {
            Value = value;
        }
    }
}
