using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace MonoFarming.Audio {
    public class SoundEffectAudio {

        public static float musicVolume = 1.0f; //background music volume
        public static float volume = 1f; //general volume

        private static Dictionary<string, SoundEffect> SoundEffects;
        public static Dictionary<string, SoundEffectInstance> SoundEffectInstances;

        public static void Load() {

            SoundEffectAudio.SoundEffects = new Dictionary<string, SoundEffect> {

                {"BreakStone1", Main.contentManager.Load<SoundEffect>("Sounds\\BreakStone1")},
                {"BreakStone2", Main.contentManager.Load<SoundEffect>("Sounds\\BreakStone2")},
                {"Cell", Main.contentManager.Load<SoundEffect>("Sounds\\Cell")},
                {"ChangeTab", Main.contentManager.Load<SoundEffect>("Sounds\\ChangeTab")},
                {"CloseDialog", Main.contentManager.Load<SoundEffect>("Sounds\\CloseDialog")},
                {"CloseMenu", Main.contentManager.Load<SoundEffect>("Sounds\\CloseMenu")},
                {"Cut", Main.contentManager.Load<SoundEffect>("Sounds\\Cut")},
                {"Hover", Main.contentManager.Load<SoundEffect>("Sounds\\Hover")},
                {"Harvest", Main.contentManager.Load<SoundEffect>("Sounds\\Harvest")},
                {"OpenDialog", Main.contentManager.Load<SoundEffect>("Sounds\\OpenDialog")},
                {"OpenDoor", Main.contentManager.Load<SoundEffect>("Sounds\\OpenDoor")},
                {"OpenMenu", Main.contentManager.Load<SoundEffect>("Sounds\\OpenMenu")},
                {"Pickup", Main.contentManager.Load<SoundEffect>("Sounds\\Pickup")},
                {"Place", Main.contentManager.Load<SoundEffect>("Sounds\\Place")},
                {"Plant", Main.contentManager.Load<SoundEffect>("Sounds\\Plant")},
                {"Step", Main.contentManager.Load<SoundEffect>("Sounds\\Step")},
                {"Step2", Main.contentManager.Load<SoundEffect>("Sounds\\Step2")},
                {"StoneStep", Main.contentManager.Load<SoundEffect>("Sounds\\StoneStep")},
                {"Swap", Main.contentManager.Load<SoundEffect>("Sounds\\Swap")},
                {"Swing", Main.contentManager.Load<SoundEffect>("Sounds\\Swing")},
                {"ToolMissHit", Main.contentManager.Load<SoundEffect>("Sounds\\ToolMissHit")},
                {"UseAxe", Main.contentManager.Load<SoundEffect>("Sounds\\UseAxe")},
                {"UseHoe", Main.contentManager.Load<SoundEffect>("Sounds\\UseHoe")},
            };

            SoundEffectAudio.SoundEffectInstances = new Dictionary<string, SoundEffectInstance>();

            foreach (var s in SoundEffectAudio.SoundEffects) {

                SoundEffectAudio.SoundEffectInstances.Add(s.Key, s.Value.CreateInstance());
            }
        }

        public static void Update() {

            //foreach (var s in AudioManager.SoundEffectInstances) {

            //    s.Value.Volume = AudioManager.volume;
            //}

            SoundEffectAudio.volume = MathHelper.Clamp(SoundEffectAudio.volume, 0f, 1f);
            SoundEffectAudio.musicVolume = MathHelper.Clamp(SoundEffectAudio.musicVolume, 0f, 1f);
        }
    }
}