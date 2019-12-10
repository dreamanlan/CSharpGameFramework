using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


namespace Unity.MemoryProfilerForExtension.Containers
{
    namespace Unsafe
    {
        internal static class NativeArrayAlgorithms
        {
            /// <summary>
            /// Implementation of the binary search algorithm.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="array">Pre sorted native array</param>
            /// <param name="value"></param>
            /// <remarks>Note that there are no checks in regards to the provided NativeArray's sort state.</remarks>
            /// <returns>Index of the value</returns>
            public static int BinarySearch<T>(NativeArray<T> array, T value) where T : struct, IComparable<T>
            {
                unsafe
                {
                    byte* ptr = (byte*)array.GetUnsafePtr();
                    byte* val = (byte*)UnsafeUtility.AddressOf<T>(ref value);

                    int typeSize = UnsafeUtility.SizeOf<T>();

                    switch (array.Length)
                    {
                        case 1:
                            if (UnsafeUtility.MemCmp(ptr, val, typeSize) == 0)
                            {
                                return 0;
                            }
                            return -1;

                        case 2:
                            if (UnsafeUtility.MemCmp(ptr + typeSize, val, typeSize) == 0)
                            {
                                return 1;
                            }

                            if (UnsafeUtility.MemCmp(ptr, val, typeSize) == 0)
                            {
                                return 0;
                            }
                            return -1;
                    }

                    int left = 0;
                    int right = array.Length - 1;
                    while (left <= right)
                    {
                        int mid = (left + right) >> 1;
                        T compareVar;
                        UnsafeUtility.CopyPtrToStructure(ptr + mid * typeSize, out compareVar);
                        int cmpResult = compareVar.CompareTo(value);

                        switch (cmpResult)
                        {
                            case -1:
                                left = mid + 1;
                                break;
                            case 1:
                                right = mid - 1;
                                break;
                            case 0:
                                return mid;
                        }
                    }
                    return -1;
                }
            }

            /// <summary>
            /// Port of MSDN's internal method for QuickSort, can work with native array containers inside a jobified environment.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="array"></param>
            /// <param name="startIndex"></param>
            /// <param name="length"></param>
            /// <remarks>
            /// ~10% slower than it's counterpart when using Mono 3.5, and 10% faster when using Mono 4.x
            /// </remarks>
#if NET_4_6
            public static void IntrospectiveSort<T>(NativeArray<T> array, int startIndex, int length) where T : unmanaged, IComparable<T>
#else
            public static void IntrospectiveSort<T>(NativeArray<T> array, int startIndex, int length) where T : struct, IComparable<T>
#endif
            {
                if (length < 0 || length > array.Length)
                    throw new ArgumentOutOfRangeException("length should be in the range [0, array.Length].");
                if (startIndex < 0 || startIndex > length - 1)
                    throw new ArgumentOutOfRangeException("startIndex should in the range [0, length).");

#if !NET_4_6
                if (!UnsafeUtility.IsBlittable<T>())
                {
                    throw new ArgumentException(string.Format("Type: {0}, must be a blittable type.", typeof(T).Name));
                }
#endif
                if (length < 2)
                    return;

                unsafe
                {
                    NativeArrayData<T> data = new NativeArrayData<T>((byte*)array.GetUnsafePtr());
                    IntroSortInternal(ref data, startIndex, length + startIndex - 1, GetMaxDepth(array.Length), GetPartitionThreshold());
                }
            }

#if NET_4_6
            unsafe struct NativeArrayData<T> where T : unmanaged
            {
                [NativeDisableUnsafePtrRestriction]
                public readonly T* ptr;
                public T aux_first;
                public T aux_second;

                public NativeArrayData(void* nativeArrayPtr)
                {
                    ptr = (T*)nativeArrayPtr;
                    aux_first = default(T);
                    aux_second = aux_first;
                }
            }
#else
            unsafe struct NativeArrayData<T> where T : struct
            {
                [NativeDisableUnsafePtrRestriction]
                public readonly byte* ptr;
                public T aux_first;
                public T aux_second;
                public T aux_third;
                public NativeArrayData(byte * nativeArrayPtr)
                {
                    aux_first = default(T);
                    aux_second = aux_first;
                    aux_third = aux_first;

                    ptr = nativeArrayPtr;
                }
            }
#endif

#if NET_4_6
            static void IntroSortInternal<T>(ref NativeArrayData<T> array, int low, int high, int depth, int partitionThreshold) where T : unmanaged, IComparable<T>
#else
            static void IntroSortInternal<T>(ref NativeArrayData<T> array, int low, int high, int depth, int partitionThreshold) where T : struct, IComparable<T>

#endif
            {
                while (high > low)
                {
                    int partitionSize = high - low + 1;
                    if (partitionSize <= partitionThreshold)
                    {
                        switch (partitionSize)
                        {
                            case 1:
                                return;
                            case 2:
                                SwapIfGreater(ref array, low, high);
                                return;
                            case 3:
                                SwapSortAscending(ref array, low, high - 1, high);
                                return;
                            default:
                                InsertionSort(ref array, low, high);
                                return;
                        }
                    }
                    else if (depth == 0)
                    {
                        Heapsort(ref array, low, high);
                        return;
                    }
                    --depth;

                    int pivot = PartitionRangeAndPlacePivot(ref array, low, high);
                    IntroSortInternal(ref array, pivot + 1, high, depth, partitionThreshold);
                    high = pivot - 1;
                }
            }

#if NET_4_6
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            static void Heapsort<T>(ref NativeArrayData<T> array, int low, int high) where T : unmanaged, IComparable<T>
#else
            static void Heapsort<T>(ref NativeArrayData<T> array, int low, int high) where T : struct, IComparable<T>
#endif
            {
                int rangeSize = high - low + 1;
                for (int i = rangeSize / 2; i >= 1; --i)
                {
                    DownHeap(ref array, i, rangeSize, low);
                }
                for (int i = rangeSize; i > 1; --i)
                {
                    Swap(ref array, low, low + i - 1);

                    DownHeap(ref array, 1, i - 1, low);
                }
            }

#if NET_4_6
            unsafe static void DownHeap<T>(ref NativeArrayData<T> array, int i, int n, int low) where T : unmanaged, IComparable<T>
            {
                var typeSize = UnsafeUtility.SizeOf<T>();
                array.aux_first = *(array.ptr + (low + i - 1));

                int child;
                while (i <= n / 2)
                {
                    child = 2 * i;
                    T* cChildAddr = array.ptr + (low + child - 1);
                    T* nChildAddr = array.ptr + (low + child);

                    if (child < n && cChildAddr->CompareTo(*nChildAddr) < 0)
                    {
                        ++child;
                        cChildAddr = nChildAddr;
                        if (!(array.aux_first.CompareTo(*nChildAddr) < 0))
                            break;
                    }
                    else
                    {
                        if (!(array.aux_first.CompareTo(*cChildAddr) < 0))
                            break;
                    }

                    UnsafeUtility.MemCpy(array.ptr + (low + i - 1), cChildAddr, typeSize);
                    i = child;
                }
                *(array.ptr + (low + i - 1)) = array.aux_first;
            }

#else
            unsafe static void DownHeap<T>(ref NativeArrayData<T> array, int i, int n, int low) where T : struct, IComparable<T>
            {
                var typeSize = UnsafeUtility.SizeOf<T>();
                UnsafeUtility.CopyPtrToStructure(array.ptr + ((low + i - 1) * typeSize), out array.aux_first);

                int child;
                while (i <= n / 2)
                {
                    child = 2 * i;
                    void* cChildAddr = array.ptr + ((low + child - 1) * typeSize);
                    void* nChildAddr = array.ptr + ((low + child) * typeSize);

                    UnsafeUtility.CopyPtrToStructure(cChildAddr, out array.aux_second);
                    UnsafeUtility.CopyPtrToStructure(nChildAddr, out array.aux_third);

                    if (child < n && array.aux_second.CompareTo(array.aux_third) < 0)
                    {
                        ++child;
                        cChildAddr = nChildAddr;
                        if (!(array.aux_first.CompareTo(array.aux_third) < 0))
                            break;
                    }
                    else
                    {
                        if (!(array.aux_first.CompareTo(array.aux_second) < 0))
                            break;
                    }

                    UnsafeUtility.MemCpy(array.ptr + ((low + i - 1) * typeSize), cChildAddr, typeSize);
                    i = child;
                }
                UnsafeUtility.CopyStructureToPtr(ref array.aux_first, array.ptr + ((low + i - 1) * typeSize));
            }

#endif

#if NET_4_6
            unsafe static void InsertionSort<T>(ref NativeArrayData<T> array, int low, int high) where T : unmanaged, IComparable<T>
            {
                int i, j;
                var typeSize = UnsafeUtility.SizeOf<T>();

                for (i = low; i < high; ++i)
                {
                    j = i;
                    array.aux_first = *(array.ptr + (i + 1));
                    while (j >= low)
                    {
                        if (!(array.aux_first.CompareTo(*(array.ptr + j)) < 0))
                            break;
                        UnsafeUtility.MemCpy(array.ptr + (j + 1), array.ptr + j, typeSize);
                        j--;
                    }
                    *(array.ptr + (j + 1)) = array.aux_first;
                }
            }

#else
            unsafe static void InsertionSort<T>(ref NativeArrayData<T> array, int low, int high) where T : struct, IComparable<T>
            {
                int i, j;
                var typeSize = UnsafeUtility.SizeOf<T>();
                for (i = low; i < high; ++i)
                {
                    j = i;
                    UnsafeUtility.CopyPtrToStructure(array.ptr + ((i + 1) * typeSize), out array.aux_first);
                    while (j >= low)
                    {
                        UnsafeUtility.CopyPtrToStructure(array.ptr + (j * typeSize), out array.aux_second);
                        if (!(array.aux_first.CompareTo(array.aux_second) < 0))
                            break;

                        UnsafeUtility.CopyStructureToPtr(ref array.aux_second, array.ptr + ((j + 1) * typeSize));
                        j--;
                    }
                    UnsafeUtility.CopyStructureToPtr(ref array.aux_first, array.ptr + ((j + 1) * typeSize));
                }
            }

#endif

#if NET_4_6
            unsafe static int PartitionRangeAndPlacePivot<T>(ref NativeArrayData<T> array, int low, int high) where T : unmanaged, IComparable<T>
            {
                int mid = low + (high - low) / 2;

                // Sort low/high/mid in order to have the correct pivot.
                SwapSortAscending(ref array, low, mid, high);

                array.aux_second = *(array.ptr + mid);

                Swap(ref array, mid, high - 1);
                int left = low, right = high - 1;

                while (left < right)
                {
                    do { ++left; }
                    while ((array.ptr + left)->CompareTo(array.aux_second) < 0);

                    do { --right; }
                    while (array.aux_second.CompareTo(*(array.ptr + right)) < 0);

                    if (left >= right)
                        break;

                    Swap(ref array, left, right);
                }

                Swap(ref array, left, (high - 1));
                return left;
            }

#else
            unsafe static int PartitionRangeAndPlacePivot<T>(ref NativeArrayData<T> array , int low, int high) where T : struct, IComparable<T>
            {
                int mid = low + (high - low) / 2;
                var typeSize = UnsafeUtility.SizeOf<T>();
                // Sort low/high/mid in order to have the correct pivot.
                SwapSortAscending(ref array, low, mid, high);

                UnsafeUtility.CopyPtrToStructure(array.ptr + (mid * typeSize), out array.aux_second); // we use for swap only aux_first thus second and third are free to use

                Swap(ref array, mid, high - 1);
                int left = low, right = high - 1;

                while (left < right)
                {
                    while (true)
                    {
                        UnsafeUtility.CopyPtrToStructure(array.ptr + (++left * typeSize), out array.aux_first);
                        if (!(array.aux_first.CompareTo(array.aux_second) < 0))
                            break;
                    }

                    while (true)
                    {
                        UnsafeUtility.CopyPtrToStructure(array.ptr + (--right * typeSize), out array.aux_first);
                        if (!(array.aux_second.CompareTo(array.aux_first) < 0))
                            break;
                    }

                    if (left >= right)
                        break;

                    Swap(ref array, left, right);
                }

                Swap(ref array, left, (high - 1));
                return left;
            }

#endif

#if NET_4_6
            unsafe static void SwapSortAscending<T>(ref NativeArrayData<T> array, int left, int mid, int right) where T : unmanaged, IComparable<T>
            {
                var typeSize = UnsafeUtility.SizeOf<T>();
                T* leftAddr = array.ptr + left;
                T* midAddr = array.ptr + mid;
                T* rightAddr = array.ptr + right;

                int bitmask = 0;
                if (leftAddr->CompareTo(*midAddr) > 0)
                    bitmask = 1;
                if (leftAddr->CompareTo(*rightAddr) > 0)
                    bitmask |= 1 << 1;
                if (midAddr->CompareTo(*rightAddr) > 0)
                    bitmask |= 1 << 2;

                switch (bitmask)
                {
                    case 1:
                        array.aux_first = *leftAddr;
                        UnsafeUtility.MemCpy(leftAddr, midAddr, typeSize);
                        *midAddr = array.aux_first;
                        return;
                    case 3:
                        array.aux_first = *leftAddr;
                        UnsafeUtility.MemCpy(leftAddr, midAddr, typeSize);
                        UnsafeUtility.MemCpy(midAddr, rightAddr, typeSize);
                        *rightAddr = array.aux_first;
                        return;
                    case 4:
                        array.aux_first = *midAddr;
                        UnsafeUtility.MemCpy(midAddr, rightAddr, typeSize);
                        *rightAddr = array.aux_first;
                        return;
                    case 6:
                        array.aux_first = *leftAddr;
                        UnsafeUtility.MemCpy(leftAddr, rightAddr, typeSize);
                        UnsafeUtility.MemCpy(rightAddr, midAddr, typeSize);
                        *midAddr = array.aux_first;
                        return;
                    case 7:
                        array.aux_first = *leftAddr;
                        UnsafeUtility.MemCpy(leftAddr, rightAddr, typeSize);
                        *rightAddr = array.aux_first;
                        return;
                    default: //we are already ordered
                        return;
                }
            }

#else
            unsafe static void SwapSortAscending<T>(ref NativeArrayData<T> array, int left, int mid, int right) where T : struct, IComparable<T>
            {
                var typeSize = UnsafeUtility.SizeOf<T>();
                void* leftAddr = array.ptr + (typeSize * left);
                void* midAddr = array.ptr + (typeSize * mid);
                void* rightAddr = array.ptr + (typeSize * right);

                UnsafeUtility.CopyPtrToStructure(leftAddr, out array.aux_first);
                UnsafeUtility.CopyPtrToStructure(midAddr, out array.aux_second);
                UnsafeUtility.CopyPtrToStructure(rightAddr, out array.aux_third);

                int bitmask = 0;
                if (array.aux_first.CompareTo(array.aux_second) > 0)
                    bitmask = 1;
                if (array.aux_first.CompareTo(array.aux_third) > 0)
                    bitmask |= 1 << 1;
                if (array.aux_second.CompareTo(array.aux_third) > 0)
                    bitmask |= 1 << 2;

                switch (bitmask)
                {
                    case 1:
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_second, leftAddr);
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_first, midAddr);
                        return;
                    case 3:
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_second, leftAddr);
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_third, midAddr);
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_first, rightAddr);
                        return;
                    case 4:
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_third, midAddr);
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_second, rightAddr);
                        return;
                    case 6:
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_third, leftAddr);
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_first, midAddr);
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_second, rightAddr);
                        return;
                    case 7:
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_third, leftAddr);
                        UnsafeUtility.CopyStructureToPtr(ref array.aux_first, rightAddr);
                        return;
                    default: //we are already ordered
                        return;
                }
            }

#endif

#if NET_4_6
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            unsafe static void SwapIfGreater<T>(ref NativeArrayData<T> array, int lhs, int rhs) where T : unmanaged, IComparable<T>
            {
                if (lhs != rhs)
                {
                    T* leftAddr = array.ptr + lhs;
                    T* rightAddr = array.ptr + rhs;

                    if (leftAddr->CompareTo(*rightAddr) > 0)
                    {
                        array.aux_first = *rightAddr;
                        UnsafeUtility.MemCpy(rightAddr, leftAddr, UnsafeUtility.SizeOf<T>());
                        *leftAddr = array.aux_first;
                    }
                }
            }

#else
            static void SwapIfGreater<T>(ref NativeArrayData<T> array, int lhs, int rhs) where T : struct, IComparable<T>
            {
                if (lhs != rhs)
                {
                    unsafe
                    {
                        var typeSize = UnsafeUtility.SizeOf<T>();

                        void* leftAddr = array.ptr + (typeSize * lhs);
                        void* rightAddr = array.ptr + (typeSize * rhs);

                        UnsafeUtility.CopyPtrToStructure(leftAddr, out array.aux_first);
                        UnsafeUtility.CopyPtrToStructure(rightAddr, out array.aux_second);

                        if (array.aux_first.CompareTo(array.aux_second) > 0)
                        {
                            UnsafeUtility.MemCpy(rightAddr, leftAddr, typeSize);
                            UnsafeUtility.CopyStructureToPtr(ref array.aux_second, leftAddr);
                        }
                    }
                }
            }

#endif


#if NET_4_6
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            unsafe static void Swap<T>(ref NativeArrayData<T> array, int lhs, int rhs) where T : unmanaged, IComparable<T>
            {
                T* leftAddr = array.ptr + lhs;
                T* rightAddr = array.ptr + rhs;
                array.aux_first = *leftAddr;

                UnsafeUtility.MemCpy(leftAddr, rightAddr, UnsafeUtility.SizeOf<T>());
                *rightAddr = array.aux_first;
            }

#else
            unsafe static void Swap<T>(ref NativeArrayData<T> array, int lhs, int rhs) where T : struct, IComparable<T>
            {
                var typeSize = UnsafeUtility.SizeOf<T>();
                void* leftAddr = array.ptr + (typeSize * lhs);
                void* rightAddr = array.ptr + (typeSize * rhs);

                UnsafeUtility.CopyPtrToStructure(leftAddr, out array.aux_first);
                UnsafeUtility.MemCpy(leftAddr, rightAddr, typeSize);
                UnsafeUtility.CopyStructureToPtr(ref array.aux_first, rightAddr);
            }

#endif


            static int GetMaxDepth(int length)
            {
                return 2 * UnityEngine.Mathf.FloorToInt((float)Math.Log(length, 2));
            }

            static int GetPartitionThreshold()
            {
                return 16;
            }
        }
    }
}
