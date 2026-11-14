using System;
using System.Collections.Generic;
using ScriptRuntime;

namespace ScriptableFramework
{
    public interface IImpactLogic
    {
        void StartImpact(EntityInfo obj, int impactId);
        void Tick(EntityInfo obj, int impactId);
        void StopImpact(EntityInfo obj, int impactId);
        void OnInterrupted(EntityInfo obj, int impactId);
        int RefixHpDamage(EntityInfo obj, int impactId, int hpDamage, int senderId, ref bool isCritical, int impactOwnerId);
        void OnAddImpact(EntityInfo obj, int impactId, int addImpactId);
        bool CanInterrupt(EntityInfo obj, int impactId);
    }
}
