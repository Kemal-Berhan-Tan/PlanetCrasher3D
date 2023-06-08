namespace GameFolders.Scripts.Enums
{
    public enum SoundTypesEnum
    {
    }

    public enum HapticTypesEnum
    {
    }

    public enum AnalyticsEventTypesEnum
    {
        StartEvent,
        CompleteEvent,
        FailEvent,
        DesignEvent
    }

    public enum CameraTransitionTypesEnum
    {
        MainCamera,
        LevelEndTransition,
        FallingCamera,
        FightCamera,
        RunnerCamera
    }

    public enum CharacterBodyPartTypes
    {
        None,
        Arm,
        Leg
    }

    public enum CharacterBodyPartDirectionTypes
    {
        None,
        Arm_Right,
        Arm_Left,
        Leg_Right,
        Leg_Left
    }

    public enum PlayerAnimtionTypes
    {
        UpgradeRegionIdle,
        FallingIdle,
        LandDown,
        Run,
        Jump,
        Die,
    }

    public enum TapTimingBarTypes
    {
        None,
        Green,
        LightGreen,
        Red
    }

    public enum InputTypes
    {
        None,
        Tap
    }

    public enum InteractableItemTypes
    {
        Obstacle,
        Alien,
        Coin,
        Fuel,
        TitleUpgradeItems
    }

    public enum PowerTitles
    {
        Normal_Person,
        Sidekick,
        Hero,
        Super_Hero,
        God
    }

    public enum VFXTypesEnum
    {
        PlanetCrash,
        PlayerBossFightRegionLanded,
        BodyPartItemMerge,
        BodyPartBroked,
        AlienHit,
        ItemCollected,
        CoinCollected,
    }
}