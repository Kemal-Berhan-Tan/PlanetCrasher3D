using System;
using System.Collections.Generic;
using GameFolders.Scripts.Enums;
using UnityEngine;

namespace GameFolders.Scripts.Helpers
{
    public class CharacterBodyPartDataHelper : MonoBehaviour
    {
        [SerializeField] private CharacterBodyPartDataList characterBodyPartDataList = new CharacterBodyPartDataList();

        public CharacterBodyPartDatas GetCharacterBodyPartData(CharacterBodyPartDirectionTypes directionType)
        {
            return characterBodyPartDataList.CharacterBodyPartDatas[(int)directionType - 1];
        }

        public Sprite GetCharacterBodyPartSprite(CharacterBodyPartDirectionTypes directionType, int bodyPartLevel)
        {
            return GetCharacterBodyPartData(directionType).BodyPartSprites[bodyPartLevel];
        }
    }

    [Serializable]
    public class CharacterBodyPartDataList
    {
        public List<CharacterBodyPartDatas> CharacterBodyPartDatas = new List<CharacterBodyPartDatas>();
    }

    [Serializable]
    public class CharacterBodyPartDatas
    {
        public List<Sprite> BodyPartSprites = new List<Sprite>();
    }
}