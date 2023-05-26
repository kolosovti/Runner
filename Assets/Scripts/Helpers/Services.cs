using UnityEngine;

namespace Game.Helpers
{
    public class Services : Singleton<Services>
    {
        [SerializeField] private Configs.Configs _configs;

        public static Configs.Configs Configs => Instance._configs;
    }
}