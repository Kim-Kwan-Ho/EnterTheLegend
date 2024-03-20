using System.Collections.Generic;
using StandardData;
using UnityEngine;

[CreateAssetMenu(fileName = "GameRooms", menuName = "GameRooms")]
public class GameRoomSO : ScriptableObject
{
    public List<GameRoomType> RoomTypeList;
    public List<GameObject> RoomTypePrefabList;
}
