// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using SimpleJson;
using System.Collections.Generic;

namespace Microsoft.JSInterop
{
    internal class InteropArgSerializerStrategy : PocoJsonSerializerStrategy
    {
        private object _storageLock = new object();
        private long _nextId = 1; // Start at 1, because 0 signals "no object"
        private Dictionary<long, object> _trackedObjects = new Dictionary<long, object>();

        protected override bool TrySerializeKnownTypes(object input, out object output)
        {
            if (input is DotNetObject marshalByRefValue)
            {
                long id;

                lock (_storageLock)
                {
                    id = _nextId++;
                    _trackedObjects.Add(id, marshalByRefValue.Value);
                }

                // Special value format recognized by the code in Microsoft.JSInterop.js
                // If we have to make it more clash-resistant, we can do
                output = $"__dotNetObject:{id}";

                return true;
            }
            else
            {
                return base.TrySerializeKnownTypes(input, out output);
            }
        }

        public object FindDotNetObject(long dotNetObjectId)
        {
            lock (_storageLock)
            {
                // TODO: Clearer exception if it's an unknown ID
                return _trackedObjects[dotNetObjectId];
            }
        }

        public void ReleaseDotNetObject(long dotNetObjectId)
        {
            lock (_storageLock)
            {
                _trackedObjects.Remove(dotNetObjectId);
            }
        }
    }
}
