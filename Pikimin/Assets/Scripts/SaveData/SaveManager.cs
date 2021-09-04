using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Audio;
using Camera;
using Items;
using Players;
using UI;
using UnityEngine;

namespace SaveData
{
    public static class SaveManager
    {
        #region Path
        private static readonly string PlayerFilePath = Application.persistentDataPath + "/player.data";
        private static readonly string PlayerControllerFilePath = Application.persistentDataPath + "/playerController.data";
        private static readonly string PikiminInventoryFilePath = Application.persistentDataPath + "/pikiminInventory.data";
        private static readonly string ItemInventoryFilePath = Application.persistentDataPath + "/itemInventory.data";
        private static readonly string PikiminBoxFilePath = Application.persistentDataPath + "/pikiminBox.data";
        private static readonly string PikidexFilePath = Application.persistentDataPath + "/pikidex.data";
        private static readonly string TrainerFilePath = Application.persistentDataPath + "/trainer.data";
        private static readonly string FoundItemFilePath = Application.persistentDataPath + "/foundItem.data";
        private static readonly string ProfAleiFilePath = Application.persistentDataPath + "/profAlei.data";
        private static readonly string GiftFilePath = Application.persistentDataPath + "/gift.data";
        private static readonly string AudioFilePath = Application.persistentDataPath + "/audio.data";
        private static readonly string SettingsFilePath = Application.persistentDataPath + "/settings.data";
        private static readonly string TimeFilePath = Application.persistentDataPath + "/time.data";
        #endregion
        
        #region Player
        public static void SavePlayer(Player player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PlayerFilePath, FileMode.Create);

            PlayerData data = new PlayerData(player);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static PlayerData LoadPlayer()
        {
            if (File.Exists(PlayerFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(PlayerFilePath, FileMode.Open);

                PlayerData data = formatter.Deserialize(stream) as PlayerData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + PlayerFilePath);
            return null;
        }
        #endregion
        
        #region PlayerController
        public static void SavePlayerController(PlayerController player)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PlayerControllerFilePath, FileMode.Create);

            PlayerControllerData data = new PlayerControllerData(player);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static PlayerControllerData LoadPlayerController()
        {
            if (File.Exists(PlayerControllerFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(PlayerControllerFilePath, FileMode.Open);

                PlayerControllerData data = formatter.Deserialize(stream) as PlayerControllerData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + PlayerControllerFilePath);
            return null;
        }
        #endregion

        #region PikiminInventory
        public static void SavePikiminInventory(PikiminInventory pikiminInventory)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PikiminInventoryFilePath, FileMode.Create);

            PikiminInventoryData data = new PikiminInventoryData(pikiminInventory);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static PikiminInventoryData LoadPikiminInventory()
        {
            if (File.Exists(PikiminInventoryFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(PikiminInventoryFilePath, FileMode.Open);

                PikiminInventoryData data = formatter.Deserialize(stream) as PikiminInventoryData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + PikiminInventoryFilePath);
            return null;
        }
        #endregion

        #region ItemInventory
        public static void SaveItemInventory(ItemInventory itemInventory)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(ItemInventoryFilePath, FileMode.Create);

            ItemInventoryData data = new ItemInventoryData(itemInventory);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static ItemInventoryData LoadItemInventory()
        {
            if (File.Exists(ItemInventoryFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(ItemInventoryFilePath, FileMode.Open);

                ItemInventoryData data = formatter.Deserialize(stream) as ItemInventoryData;

                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + ItemInventoryFilePath);
            return null;
        }
        #endregion

        #region PikiminBox
        public static void SavePikiminBox(PikiminBox pikiminBox)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PikiminBoxFilePath, FileMode.Create);

            PikiminBoxData data = new PikiminBoxData(pikiminBox);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static PikiminBoxData LoadPikiminBox()
        {
            if (File.Exists(PikiminBoxFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(PikiminBoxFilePath, FileMode.Open);

                PikiminBoxData data = formatter.Deserialize(stream) as PikiminBoxData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + PikiminBoxFilePath);
            return null;
        }
        #endregion

        #region Pikidex
        public static void SavePikidex(PikidexBehaviour pikidex)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(PikidexFilePath, FileMode.Create);

            PikidexData data = new PikidexData(pikidex);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static PikidexData LoadPikidex()
        {
            if (File.Exists(PikidexFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(PikidexFilePath, FileMode.Open);

                PikidexData data = formatter.Deserialize(stream) as PikidexData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + PikidexFilePath);
            return null;
        }
        #endregion

        #region Trainer
        public static void SaveTrainer(NpcManager trainer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(TrainerFilePath, FileMode.Create);

            TrainerData data = new TrainerData(trainer);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static TrainerData LoadTrainer()
        {
            if (File.Exists(TrainerFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(TrainerFilePath, FileMode.Open);

                TrainerData data = formatter.Deserialize(stream) as TrainerData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + TrainerFilePath);
            return null;
        }
        #endregion

        #region FoundItem
        public static void SaveFoundItem(ItemManager itemManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(FoundItemFilePath, FileMode.Create);

            FoundItemData data = new FoundItemData(itemManager);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static FoundItemData LoadFoundItem()
        {
            if (File.Exists(FoundItemFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(FoundItemFilePath, FileMode.Open);

                FoundItemData data = formatter.Deserialize(stream) as FoundItemData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + FoundItemFilePath);
            return null;
        }
        #endregion

        #region ProfAlei
        public static void SaveProfAlei(AleiBehaviour prof)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(ProfAleiFilePath, FileMode.Create);

            ProfAleiData data = new ProfAleiData(prof);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static ProfAleiData LoadProfAlei()
        {
            if (File.Exists(ProfAleiFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(ProfAleiFilePath, FileMode.Open);

                ProfAleiData data = formatter.Deserialize(stream) as ProfAleiData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + ProfAleiFilePath);
            return null;
        }
        #endregion

        #region Gifts
        public static void SaveGifts(GiftPikiminAndItemManager gift)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(GiftFilePath, FileMode.Create);

            GiftPikiminAndItemData data = new GiftPikiminAndItemData(gift);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static GiftPikiminAndItemData LoadGifts()
        {
            if (File.Exists(GiftFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(GiftFilePath, FileMode.Open);

                GiftPikiminAndItemData data = formatter.Deserialize(stream) as GiftPikiminAndItemData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + GiftFilePath);
            return null;
        }
        #endregion

        #region Audio
        public static void SaveAudioClip(AudioManager audioManager)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(AudioFilePath, FileMode.Create);

            AudioData data = new AudioData(audioManager);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static AudioData LoadAudioClip()
        {
            if (File.Exists(AudioFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(AudioFilePath, FileMode.Open);

                AudioData data = formatter.Deserialize(stream) as AudioData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + AudioFilePath);
            return null;
        }
        #endregion

        #region Settings
        public static void SaveSettings(Settings settings)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(SettingsFilePath, FileMode.Create);

            SettingsData data = new SettingsData(settings);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static SettingsData LoadSettings()
        {
            if (File.Exists(SettingsFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(SettingsFilePath, FileMode.Open);

                SettingsData data = formatter.Deserialize(stream) as SettingsData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + SettingsFilePath);
            return null;
        }
        #endregion

        #region Time
        public static void SaveTime(Timer timer)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(TimeFilePath, FileMode.Create);

            TimerData data = new TimerData(timer);
        
            formatter.Serialize(stream, data);
        
            stream.Close();
        }

        public static TimerData LoadTime()
        {
            if (File.Exists(TimeFilePath))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(TimeFilePath, FileMode.Open);

                TimerData data = formatter.Deserialize(stream) as TimerData;
            
                stream.Close();

                return data;
            }
            Debug.LogError("Save File not found in " + TimeFilePath);
            return null;
        }
        #endregion

        #region DeleteFiles
        public static void DeleteSaveFiles()
        {
            if (File.Exists(PlayerFilePath))
            {
                File.Delete(PlayerFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + PlayerFilePath);
            }
            
            if (File.Exists(PlayerControllerFilePath))
            {
                File.Delete(PlayerControllerFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + PlayerControllerFilePath);
            }
            
            if (File.Exists(PikiminInventoryFilePath))
            {
                File.Delete(PikiminInventoryFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + PikiminInventoryFilePath);
            }
            
            if (File.Exists(ItemInventoryFilePath))
            {
                File.Delete(ItemInventoryFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + ItemInventoryFilePath);
            }
            
            if (File.Exists(PikiminBoxFilePath))
            {
                File.Delete(PikiminBoxFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + PikiminBoxFilePath);
            }
            
            if (File.Exists(PikidexFilePath))
            {
                File.Delete(PikidexFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + PikidexFilePath);
            }
            
            if (File.Exists(TrainerFilePath))
            {
                File.Delete(TrainerFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + TrainerFilePath);
            }
            
            if (File.Exists(FoundItemFilePath))
            {
                File.Delete(FoundItemFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + FoundItemFilePath);
            }
            
            if (File.Exists(ProfAleiFilePath))
            {
                File.Delete(ProfAleiFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + ProfAleiFilePath);
            }
            
            if (File.Exists(GiftFilePath))
            {
                File.Delete(GiftFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + GiftFilePath);
            }
            
            if (File.Exists(AudioFilePath))
            {
                File.Delete(AudioFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + AudioFilePath);
            }
            
            if (File.Exists(SettingsFilePath))
            {
                File.Delete(SettingsFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + SettingsFilePath);
            }
            
            if (File.Exists(TimeFilePath))
            {
                File.Delete(TimeFilePath);
            }
            else
            {
                Debug.LogError("Save File not found in " + TimeFilePath);
            }
        }
        #endregion

        #region CheckSaveData
        public static bool CanLoadSaveData()
        {
            if (File.Exists(PlayerFilePath) 
                && File.Exists(PlayerControllerFilePath) && File.Exists(TimeFilePath)
                && File.Exists(PikiminInventoryFilePath) && File.Exists(ItemInventoryFilePath)
                && File.Exists(PikiminBoxFilePath) && File.Exists(PikidexFilePath) 
                && File.Exists(TrainerFilePath) && File.Exists(FoundItemFilePath) 
                && File.Exists(ProfAleiFilePath) && File.Exists(GiftFilePath)
                && File.Exists(AudioFilePath) && File.Exists(SettingsFilePath))
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}
