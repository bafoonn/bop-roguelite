namespace Pasta
{
    public enum StatType
    {
        Health,
        Damage,
        AttackSpeed,
        MovementSpeed,
        DodgeCount,
        DodgeCooldown
    }

    public static class StatTypeExtension
    {
        public static string ReadableStr(this StatType statType)
        {
            switch (statType)
            {
                case StatType.Health: return "Health";
                case StatType.Damage: return "Damage";
                case StatType.AttackSpeed: return "Attack Speed";
                case StatType.MovementSpeed: return "Movement Speed";
                case StatType.DodgeCount: return "Dodge Count";
                case StatType.DodgeCooldown: return "Dodge Cooldown";
                default: return statType.ToString();
            }
        }
    }
}
