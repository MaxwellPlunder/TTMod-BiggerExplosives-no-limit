using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using static PlayerDigger;

namespace BiggerExplosives.Patches
{
    public class ExplodeActionInfoPatch
    {
        [HarmonyPatch(typeof(ExplodeActionInfo), "Explode")]
        [HarmonyPrefix]
        static bool setBiggerSize(ExplodeActionInfo __instance, out bool madeChanges, ref PlayerDigger.DigResultInfo digResultInfo) {
            madeChanges = false;
            foreach (uint index in __instance.explosiveIDs) {
                MachineInstanceRef<ExplosiveInstance> machineInstanceRef;
                if (MachineManager.instance.GetRefFromId<ExplosiveInstance>(index, out machineInstanceRef)) {
                    ref ExplosiveInstance ptr = ref machineInstanceRef.Get();
                    IStreamedMachineInstance streamedMachineInstance;
                    if (machineInstanceRef.AsGeneric().GetStreamedMachineInstance(out streamedMachineInstance)) {
                        ptr.myDef.TriggerDetonationFx(ref ptr);
                    }
                    ptr.detonated = true;
                    ptr.myDef.explosionRadius = BiggerExplosivesPlugin.ExplosionRadius.Value;
                    ptr.myDef.explosionDepth = BiggerExplosivesPlugin.ExplosionDepth.Value;
                    madeChanges = PlayerDigger.instance.ExplodeArea(ptr.myDef.shape, ptr.myDef.explosionRadius, ptr.myDef.explosionDepth, ptr.myDef.maxHardness, -1, ptr.blastDirection, ptr.gridInfo.Center, ptr.visualPositionOffset, ref digResultInfo);
                    return false;
                }
            }

            return false;
        }
    }
}
