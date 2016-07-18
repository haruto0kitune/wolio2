[System.Serializable]
public class PlayerParameter
{
    public PlayerBasics PlayerBasics;
}

[System.Serializable]
public class PlayerBasics
{
    public Run Run;
    public Creep Creep;
    public ActionModeJump ActionModeJump;
    public FightingModeJump FightingModeJump;
    public AirMove AirMove;
    public WallKickJump WallKickJump;
    public DoubleJump DoubleJump;
    public AirDash AirDash;
} 

[System.Serializable]
public class Run
{
    public float MaxSpeed;
}

[System.Serializable]
public class Creep
{
    public float Speed;
}

[System.Serializable]
public class ActionModeJump
{
    public float JumpForce;
    public int Active;
}

[System.Serializable]
public class FightingModeJump
{
    public float JumpForce;
}

[System.Serializable]
public class AirMove
{
    public float MaxSpeed;
    public float Force;
}

[System.Serializable]
public class WallKickJump
{
    public int Angle;
    public float JumpForce;
    public int Recovery;
}

[System.Serializable]
public class DoubleJump
{
    public float DoubleJumpForce;
}

[System.Serializable]
public class AirDash
{
    public float Speed;
    public int Recovery;
}