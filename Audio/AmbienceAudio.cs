using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace MonoFarming.Audio {
    public class AmbienceAudio {

        public static float ambienceVolume = 1f;

        private static Dictionary<string, SoundEffect> AmbienceSounds;
        public static Dictionary<string, SoundEffectInstance> AmbienceSoundInstances;

        public static void Load() {

            AmbienceAudio.AmbienceSounds = new Dictionary<string, SoundEffect> {

                {"Day", Main.contentManager.Load<SoundEffect>("Sounds\\Day")},
                {"Night", Main.contentManager.Load<SoundEffect>("Sounds\\Night")},
                {"Rain", Main.contentManager.Load<SoundEffect>("Sounds\\Rain")}
            };

            AmbienceAudio.AmbienceSoundInstances = new Dictionary<string, SoundEffectInstance>();

            foreach (var s in AmbienceAudio.AmbienceSounds) {

                AmbienceAudio.AmbienceSoundInstances.Add(s.Key, s.Value.CreateInstance());
            }
        }

        public static void Update() {

            AmbienceAudio.ambienceVolume = MathHelper.Clamp(AmbienceAudio.ambienceVolume, 0f, 1f);
        }
    }
}