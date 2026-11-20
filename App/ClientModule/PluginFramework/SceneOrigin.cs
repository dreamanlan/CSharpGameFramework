using UnityEngine;

namespace ScriptableFramework
{
    /// <summary>
    /// World-space origin of the battlefield region.
    ///
    /// World-tile scenes MUST stay at the origin: the room server owns their
    /// coordinates (WorldSyncSystem, WorldEvent.txt, AOI, battle_pos/explore
    /// reports). Battlefield scenes are authored at a fixed, far-away origin so
    /// both can be loaded at once without overlap: the world tile keeps running
    /// while the camera simply moves to the battlefield region.
    ///
    /// Logical entity coordinates (MovementStateInfo, story DSL, server tables)
    /// are ALWAYS local 0..512. The offset is applied only at the logic-to-view
    /// boundary (GameObject.transform, NavMesh, camera, raycast, minimap) so the
    /// server never sees a shifted coordinate.
    ///
    /// 8192 > 4096 (the full 8x8 x 512 world) and is well beyond the camera's
    /// far clip plane, so the two regions never z-fight or share a raycast.
    /// </summary>
    public static class SceneOrigin
    {
        public const float BattleOffsetX = 8192f;
        public static readonly Vector3 BattleOffset = new Vector3(BattleOffsetX, 0f, 0f);
        public static readonly Vector3 Zero = Vector3.zero;

        /// <summary>
        /// Origin for an entity. World-layer actors are server-owned and stay
        /// at zero; client-local battle actors live in the offset region.
        /// </summary>
        public static Vector3 ForEntity(EntityInfo info)
        {
            if (null != info && !info.IsServerEntity && PluginFramework.Instance.IsBattleState)
                return BattleOffset;
            return Zero;
        }

        public static Vector3 ForEntity(int objId)
        {
            return ForEntity(PluginFramework.Instance.GetEntityById(objId));
        }

        public static Vector3 ForGameObject(GameObject go)
        {
            if (null == go) return Zero;
            return ForEntity(PluginFramework.Instance.GetGameObjectId(go));
        }

        /// <summary>
        /// Origin of the region the camera/logic is currently in. Use this only
        /// for view-to-logical conversions with no entity context (raycast hit,
        /// camera look target, minimap dots).
        /// </summary>
        public static Vector3 Current
        {
            get { return PluginFramework.Instance.IsBattleState ? BattleOffset : Zero; }
        }

        public static Vector3 ToWorld(Vector3 local, Vector3 origin) { return local + origin; }
        public static Vector3 ToLocal(Vector3 world, Vector3 origin) { return world - origin; }
    }
}
