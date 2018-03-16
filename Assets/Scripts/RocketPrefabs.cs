using UnityEngine;

[CreateAssetMenu]
public class RocketPrefabs : ScriptableObject {
	public GameObject stage1, stage2, stage3, fuel1, fuel2, fuel3, fuel4, fuel5, fuel6, fuel;

	public GameObject GetPrefab(Rocket.ConstructionStage constructionStage) {
		switch (constructionStage) {
			case Rocket.ConstructionStage.Stage1:
				return stage1;
			case Rocket.ConstructionStage.Stage2:
				return stage2;
			case Rocket.ConstructionStage.Stage3:
				return stage3;
			case Rocket.ConstructionStage.Fuel1:
				return fuel1;
			case Rocket.ConstructionStage.Fuel2:
				return fuel2;
			case Rocket.ConstructionStage.Fuel3:
				return fuel3;
			case Rocket.ConstructionStage.Fuel4:
				return fuel4;
			case Rocket.ConstructionStage.Fuel5:
				return fuel5;
			case Rocket.ConstructionStage.Fuel6:
				return fuel6;
			default:
				return null;
		}
	}
}