namespace TeamBrookvale.Game
{

	//This is an enum of named injections used around the game.

	enum GameElement
	{
		SOUND_FX_AUDIO_SOURCE
		/*ENEMY_POOL,				//Injection names of the pools
		ENEMY_MISSILE_POOL,
		MISSILE_EXPLOSION_POOL,
		MISSILE_POOL,
		ROCK_POOL,
		GAME_FIELD,				//Injection name of the GameObject that parents the rocks/missiles/player/etc.
		PLAYER_SHIP,			//Injection name of the player's vessel*/
	}

	public enum PauseStatus
	{
		RUN = 0,
		PAUSE_IN_PROGRESS,
		PAUSE,
		RESUME_IN_PROGRESS
	}

	public enum LevelType
	{
		NothingToDemolish,
		Buoy,
		Dam,
		Bridge,
		ShipDrill
	}

	public enum GOType {
		Bridge,
		Buoy,
		Dam,
		Dock,
		Tutorial,
		ShipDrillable,
		UBoat
	}

	public enum PlayerPrefsLevelKey {
		Open,
		Completed,
		Locked
	}
}