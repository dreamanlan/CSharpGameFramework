using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Unity.MemoryProfilerForExtension.Editor.NativeArrayExtensions
{
    static public class NativeArrayExt
    {
        public static void MemClear<T>(this NativeArray<T> arr) where T : struct
        {
            unsafe
            {
                UnsafeUtility.MemClear(arr.GetUnsafePtr(), arr.Length * UnsafeUtility.SizeOf<T>());
            }
        }
    }
}
