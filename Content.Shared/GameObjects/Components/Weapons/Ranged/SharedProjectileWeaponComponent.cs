using System;
using Robust.Shared.Serialization;

namespace Content.Shared.GameObjects.Components.Weapons.Ranged
{
    [Serializable, NetSerializable]
    public enum BallisticCaliber
    {
        Unspecified = 0,
        // .32
        A32,
        // .357
        A357,
        // .44
        A44,
        // .45mm
        A45mm,
        // .50 cal
        A50,
        // 5.56mm
        A556mm,
        // 6.5mm
        A65mm,
        // 7.62mm
        A762mm,
        // 9mm
        A9mm,
        // 10mm
        A10mm,
        // 20mm
        A20mm,
        // 24mm
        A24mm,
    }
}
