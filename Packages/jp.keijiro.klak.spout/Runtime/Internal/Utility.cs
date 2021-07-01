// KlakSpout - Spout video frame sharing plugin for Unity
// https://github.com/keijiro/KlakSpout

using UnityEngine;
using UnityEngine.Rendering;

namespace Klak.Spout
{
    // Internal utilities
    static class Util
    {
        internal static void Destroy(Object obj)
        {
            if (obj == null) return;

            if (Application.isPlaying)
                Object.Destroy(obj);
            else
                Object.DestroyImmediate(obj);
        }

        static CommandBuffer _commandBuffer;

        internal static void
            IssuePluginEvent(PluginEntry.Event pluginEvent, System.IntPtr ptr)
        {
            if (_commandBuffer == null) _commandBuffer = new CommandBuffer();

            _commandBuffer.IssuePluginEventAndData(
                PluginEntry.GetRenderEventFunc(), (int)pluginEvent, ptr
            );

            Graphics.ExecuteCommandBuffer(_commandBuffer);

            _commandBuffer.Clear();
        }

        private static int frameCount = 0;

        // Calls a function in the native plugin once per frame,
        // on the render thread
        internal static void
            UpdateWrapCache(int newFrame, CommandBuffer buffer)
        {
            if (frameCount <= newFrame)
            {
                frameCount = newFrame;

                buffer.IssuePluginEventAndData(
                    PluginEntry.GetRenderEventFunc(),
                    (int)PluginEntry.Event.UpdateWrapCache,
                    System.IntPtr.Zero // This event doesn't need data
                );
            }
        }
    }
}
